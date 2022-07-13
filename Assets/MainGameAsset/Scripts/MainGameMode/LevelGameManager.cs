using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
public class LevelGameManager : MonoBehaviourPunCallbacks
{
    
    public static LevelGameManager instance;//giving this script reference to itself in order to make it become static, then no need to worry about permission related problem as following variable will all become static
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
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene(0);
        }

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

        saveData.ResetData();
        //reset savefile information
       /* SaveSystem.instance.SaveGame(saveData);
        saveData.ResetData();
        SaveSystem.instance.SaveGame(saveData);
      
        saveData = SaveSystem.instance.LoadGame();//load saving information*/
      
    }

    //remaining life
    public int lifeNum = 5;

    //list of players
    public GameObject[] players;
    public List<Transform> activePlayers = new List<Transform>();
    public GameObject[] playersInScene;
  

    //list of enemies
    public List<Transform> activeEnemies = new List<Transform>();

    //list of spawn positions
    public Transform[] spawnPositions;

   
    //virtual camera
    public CinemachineVirtualCamera P1Cam;
    
    //UI
    public Slider[] healthBars;
    public TMP_Text p1CurretnPistolAmmo;
 
    public TMP_Text p1MaxPistolAmmo;
 
    public TMP_Text lifeNumText;
    public GameObject PauseMenu;
    public GameObject ControlInfo;
    public GameObject FadeOutScreen;


    //Resource Management
    public int woodNum = 0, metalNum = 0;



    //list of scripts for the game manager to reference


    //SetUp Scene
    private void Start()
    {
       
       
        SetUpScene();

        

        Destroy(GameObject.Find("Bgm"));
        
    }

    private void Update()
    {
   
        if (Input.GetKeyDown(KeyCode.Escape)&&PauseMenu) 
        {
            PauseMenu.SetActive(true);
            
        }
    }

    public void SetUpScene() 
    {
      

       
        SpawnPlayer(1);    
      
        UpdateLifeNum();
        //Invoke("UpdatePlayerList", 0.1f);



    }

   

   
    public void SpawnPlayer(int playerNumber)
    {
      
        
        //spawn the player 

        int selectedPlayerModel;
        if (PhotonNetwork.IsMasterClient)
        { 
            //p1 player model
            selectedPlayerModel = 0;
        }

        else 
        {
            //p2 player model
            selectedPlayerModel = 1;
        }


        var player = PhotonNetwork.Instantiate(players[selectedPlayerModel].name, spawnPositions[selectedPlayerModel].position, players[selectedPlayerModel].transform.rotation);


      



        //assign player number to decide this players' controller system
        var playerInputs = player.GetComponent<PlayerInput>();

        //set up camera
        P1Cam.Follow = player.transform;
      
        //setting player input

        playerInputs.playerNum = playerNumber;
        playerInputs.DeterminInput();
  
       
        //spawned player gonna be invincible for 2 secs
        var playerHealth = player.GetComponent<PlayerHealth>();
        playerHealth.canBeDamaged = false;
        playerHealth.Invoke("ResetDamage", 2f);
        UpdateAmmoUI();
        UpdateLifeNum();
        //add to list
        player.GetComponent<PhotonView>().RPC("UpdatePlayerToList", RpcTarget.All);
    }


    
    


    public void UpdateAmmoUI() 
    {


        foreach (Transform player in activePlayers)
        {


            if (player.GetComponent<PhotonView>().Owner.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                if (player.GetComponent<PlayerInteraction>().currentWeapon == null) return;
                p1CurretnPistolAmmo.text = player.GetComponent<PlayerInteraction>().currentWeapon.GetComponent<Pistol>().GetCurrentAmmo().ToString();
                p1MaxPistolAmmo.text = player.GetComponent<PlayerInteraction>().currentWeapon.GetComponent<Pistol>().GetMaxAmmo().ToString();

            }
        }

           


        }

    #region LifeSystem
    IEnumerator ToGameOverScene() 
    {
        yield return new WaitForSeconds(2f);
        if (FadeOutScreen)
        {
            FadeOutScreen.SetActive(true);
        }
        yield return new WaitForSeconds(1f);

        Debug.Log("GameOver, entering GameOver Scene");
        SceneManager.LoadScene("GameOver");

    }

   
    public void LostLife() 
    {
        if (lifeNum > 0)
        {
            lifeNum--;
            UpdateLifeNum();
            
           
        }
    
    }

    public void UpdateLifeNum() 
    {
        lifeNumText.text = lifeNum.ToString();
    
    }


    #endregion



    IEnumerator ToResultScene() 
    {
        Debug.Log("level finished");
        saveData.AddWood(woodNum);
        saveData.AddMetal(metalNum);
        SaveSystem.instance.SaveGame(saveData);
        Debug.Log("resource saved");
        yield return new WaitForSeconds(1f);
        if (FadeOutScreen)
        {
           FadeOutScreen.SetActive(true);
        }
        yield return new WaitForSeconds(2f);
        PhotonNetwork.AutomaticallySyncScene = false;
        //PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("MainModeResultScene");

    }


  

   public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        //do whatever you want
       
    }

    
    

    public void UpdatePlayerList() 
    {
        activePlayers.Clear();
        playersInScene = GameObject.FindGameObjectsWithTag("Player");
       Transform[] playerTransform =  new Transform[playersInScene.Length];
        

         for(int i = 0;i < playersInScene.Length;i++) 
        {
            playerTransform[i] = playersInScene[i].transform;
        }
        

        activePlayers.AddRange(playerTransform);
        UpdateAmmoUI();
    }


    //if a player left the room, all players should left the room as well
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(0);
       

    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        SceneManager.LoadScene(0);
    }


    #region Resource Management

    public void AddWood(int num)
    {
        woodNum += num;

    }

    public void AddMetal(int num)
    {
        metalNum += num;

    }


    #endregion


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
        PhotonNetwork.Disconnect();
       
    }

    public void ControlInfoMenu() 
    {
        ControlInfo.SetActive(true);
    }

    public void CloseControlInfoMenu()
    {
        ControlInfo.SetActive(false);
    }

    #endregion

}
