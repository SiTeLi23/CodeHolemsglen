using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.UI;
using TMPro;
public class PVPGameManager : MonoBehaviour
{
    
    public static PVPGameManager instance;//giving this script reference to itself in order to make it become static, then no need to worry about permission related problem as following variable will all become static
    GameData saveData = new GameData();
    #region IfCannotDestroyThenUseThisCode
    /*public float score;
    public bool isResetting;

    //make sure every time a new scene loaded,the awake and start method will also be called even if this instance won't be destroyed.
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene,LoadSceneMode mode) 
    {
        if (isResetting) score = 0f;
        isResetting = false;
    }*/
    #endregion

    //make sure this scripts is the only one
    private void Awake()
    {
        #region SingleTON PATTERN
        //check if there's already instance exist
        if (instance != null) 
        {
            Destroy(gameObject);
        
        }
        else 
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);// becareful with this method as the awake or start method would not run when loaded a new scene
        
        }
        #endregion

    }


    //list of players
    public GameObject[] players;
    public List<GameObject> activePlayers = new List<GameObject>();
    //list of scores
    public int[] playerScores;
    //list of spawn positions
    public Transform[] spawnPositions;
    public Transform[] spawnFxPos;
    public GameObject spawnFx;
   
    //virtual camera
    public CinemachineVirtualCamera P1Cam;
    public CinemachineVirtualCamera P2Cam;
    //UI
    public Slider[] healthBars;
    public TMP_Text p1Score;
    public TMP_Text p2Score;
    public TMP_Text p1Score2;
    public TMP_Text p2Score2;
    public TMP_Text p1CurretnPistolAmmo;
    public TMP_Text p2CurretnPistolAmmo;
    public TMP_Text p1MaxPistolAmmo;
    public TMP_Text p2MaxPistolAmmo;
    public GameObject PauseMenu;
    public GameObject ControlInfo;
    public GameObject FadeOutScreen;


    public int maxKills;
    public int winner;

    //list of scripts for the game manager to reference

    
    //SetUp Scene
    private void Start()
    {
       
        
        SetUpScene();
        SetUpCamera();
        saveData = SaveSystem.instance.LoadGame();
        
        Destroy(GameObject.Find("Bgm"));
    }

    private void Update()
    {
        //ScoringUI Update
        p1Score.text = "P1  " + playerScores[0].ToString();
        p2Score.text =  playerScores[1].ToString() + "  P2" ;
        p1Score2.text = "P1  " + playerScores[0].ToString();
        p2Score2.text = playerScores[1].ToString() + "  P2";
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            PauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void SetUpScene() 
    {
        //spawn as many player as possible within the list
       for(int i = 1; i < players.Length + 1; i++) 
        {
            SpawnPlayer(i);
           
        }
        UpdateAmmoUI();



    }

    public void SetUpCamera() 
    {
        if (P1Cam)
        {
           
            foreach(GameObject player in activePlayers) 
            {
                 if(player.GetComponent<PlayerInput>().playerNum == 1) 
                {

                    P1Cam.Follow = player.transform;
                }
            
            }

        }
        if (P2Cam)
        {
            foreach (GameObject player in activePlayers)
            {
                if (player.GetComponent<PlayerInput>().playerNum == 2)
                {

                    P2Cam.Follow = player.transform;
                }

            }
        }
       

    }


    public void SpawnPlayer(int playerNumber)
    {
        
        //spawn the player 
        var player = Instantiate(players[playerNumber-1], spawnPositions[playerNumber-1].position, players[playerNumber-1].transform.rotation);
        activePlayers.Add(player);
        //assign player number to decide this players' controller system
        var playerInputs = player.GetComponent<PlayerInput>();
        //playerInputs.playerName = MasterManager.instance.playerNames[playerNumber - 1];
        playerInputs.playerNum = playerNumber;
        playerInputs.DeterminInput();
        playerInputs.UpdateUI();
       
        //spawned player gonna be invincible for 2 secs
        var playerHealth = player.GetComponent<PlayerHealth>();
        playerHealth.canBeDamaged = false;
        playerHealth.Invoke("ResetDamage", 2f);
        UpdateAmmoUI();
    }


    public void UpdateScore(int playerScoring,int playerKilled) 
    {
        //if player is killed by non-player(environment) or by itself , we are going to reduce the score.
       if(playerScoring == 0 || playerScoring == playerKilled) 
        {
            
            playerScores[playerKilled - 1]--;
            if(playerScores[playerKilled-1]<0)
            {
                playerScores[playerKilled - 1] = 0;

            }

        }
        else 
        {

            playerScores[playerScoring - 1]++;
        }

       

        //if one of the player achieve the winning condition
        if (playerScores[playerScoring-1]>= maxKills) 
        {

            EndRound(playerScoring);
        
        }
    }


    public void UpdateAmmoUI() 
    {
        foreach (GameObject player in activePlayers)
        {
         

            if (player.GetComponent<PlayerInput>().playerNum == 1)
            {
                if(player.GetComponent<PlayerInteraction>().currentWeapon==null)return;
                p1CurretnPistolAmmo.text = player.GetComponent<PlayerInteraction>().currentWeapon.GetComponent<Pistol>().GetCurrentAmmo().ToString();
                p1MaxPistolAmmo.text = player.GetComponent<PlayerInteraction>().currentWeapon.GetComponent<Pistol>().GetMaxAmmo().ToString();

            }

            if (player.GetComponent<PlayerInput>().playerNum == 2)
            {
                if (player.GetComponent<PlayerInteraction>().currentWeapon == null) return;
                p2CurretnPistolAmmo.text = player.GetComponent<PlayerInteraction>().currentWeapon.GetComponent<Pistol>().GetCurrentAmmo().ToString();
                p2MaxPistolAmmo.text = player.GetComponent<PlayerInteraction>().currentWeapon.GetComponent<Pistol>().GetMaxAmmo().ToString();

            }

        }
       
    
    }



    public void EndRound(int WinningPlayer) 
    {
        Debug.Log("Player " + WinningPlayer + " is the winner");
        winner = WinningPlayer;
        saveData.AddScore(1000);
        SaveSystem.instance.SaveGame(saveData);
        Debug.Log("1000 points earned");
       
        if (FadeOutScreen)
        {
            FadeOutScreen.SetActive(true);
        }
        StartCoroutine("ToResultScene");
        
    
    }

    IEnumerator ToResultScene() 
    {
        yield return new WaitForSecondsRealtime(2f);
        SceneManager.LoadScene("ResultScene");
       
    }


    #region Button
    public void Resume() 
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void ExitGame() 
    {
        Application.Quit();
    
    }

    public void BackToMain() 
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void ControlInfoMenu() 
    {
        ControlInfo.SetActive(true);
    }

    #endregion

}
