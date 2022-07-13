using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class StartLauncher : MonoBehaviourPunCallbacks
{
    public static StartLauncher instance;

    private void Awake()
    {
        #region singleTon

        if (instance != null) 
        {
            Destroy(gameObject);
        
        }
        else 
        {

            instance = this;
        }


        #endregion

        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client
        // and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
        
    }

    string gameVersion = "1";

    //saving system
    GameData saveData = new GameData();


    [Header("Menus")]
    //loading 
     public GameObject loadingScreen;
     public TMP_Text loadingText;

    //main Menu
    public GameObject mainMenuButtons;

    //creat room && current Room
    public GameObject createRoomScreen;
    public TMP_InputField roomNameInput;
    public GameObject currentRoomScreen;
    public TMP_Text currentRoomName;
    public GameObject startGameButton;
    public TMP_Text waitingMessage;

    //find room
    public GameObject findRoomScreen;
    public RoomButton roomButtonPrefab;
    [SerializeField] List<RoomButton> allRoomButtons = new List<RoomButton>();
    Dictionary<string, RoomInfo> myCatchedRoomList = new Dictionary<string, RoomInfo>();

    //errorScreen
    public GameObject errorScreen;
    public TMP_Text errorText;

    //name Input Screen
    public GameObject nameInputScreen;
    public TMP_InputField nameInput;
    public static bool hasSetName;

    //Settings Screen
    public GameObject SettingScreen;

    

    void Start()
    {
        CloseAllScreens();
        loadingText.text = "Connecting To NetWork...";
        loadingScreen.SetActive(true);

        //if we are not connected to the server, connect to Photon network server first
        if (!PhotonNetwork.IsConnected) 
        {
            PhotonNetwork.ConnectUsingSettings();
        
        }

     


    }

    //close all screens
    void CloseAllScreens()
    {
        loadingScreen.SetActive(false);
        mainMenuButtons.SetActive(false);
        createRoomScreen.SetActive(false);
        currentRoomScreen.SetActive(false);
        findRoomScreen.SetActive(false);
        errorScreen.SetActive(false);
        nameInputScreen.SetActive(false);
        SettingScreen.SetActive(false);

    }


    #region  Lobby System

    // try joining a lobby and show the MainMenu button
    public override void OnConnectedToMaster()
    {
        //joined a lobby and ready for joining a room next
        PhotonNetwork.JoinLobby();

        loadingText.text = "Joining Lobby...";

    }

    // succssfully to join a lobby
    public override void OnJoinedLobby()
    {
        CloseAllScreens();
        mainMenuButtons.SetActive(true);
        //if there is no preset name , we need to input a new name
        if (!hasSetName) 
        {
            CloseAllScreens();
            nameInputScreen.SetActive(true);

            //if we have presetted name, set it as default
            if (PlayerPrefs.HasKey("playerName")) 
            {
                nameInput.text = PlayerPrefs.GetString("playerName");
            }
        
        }
        //if the name has already been set,set the default name 
        else 
        {
            PhotonNetwork.NickName = PlayerPrefs.GetString("playerName");

        }

    }
    #endregion


    #region Create Room System

    public void CreatRoom() 
    {
        //if there's input in the input field
        if (!string.IsNullOrEmpty(roomNameInput.text)) 
        {
            //define a room option
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 2;


            //creat room based on the option we set and name we input
            PhotonNetwork.CreateRoom(roomNameInput.text, options);
            CloseAllScreens();
            loadingText.text = "Creating A Room...";
            loadingScreen.SetActive(true);
            
        
        }

    
    }

    //once room creation complete , or people joined the room
    public override void OnJoinedRoom()
    {
        CloseAllScreens();
        currentRoomName.text = PhotonNetwork.CurrentRoom.Name;
        UpdateCurrentRoom();
        currentRoomScreen.SetActive(true);


    }

  
    //Joined selected room
    public void JoinRoom(RoomInfo inputInfo) 
    {
        PhotonNetwork.JoinRoom(inputInfo.Name);

        CloseAllScreens();
        loadingText.text = "Joining...";
        loadingScreen.SetActive(true);
    
    }

   

    //errorHandling
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        CloseAllScreens();
        errorText.text = message;
        errorScreen.SetActive(true);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        CloseAllScreens();
        errorText.text = message;
        errorScreen.SetActive(true);
    }

    #endregion


    #region Find Room System

    void RoomListButtonUpdate(Dictionary<string,RoomInfo> catchedRoomList) 
    {
        //make sure to clean old data evertime at the start when updating the room button
        foreach(RoomButton rb in allRoomButtons) 
        {
            Destroy(rb.gameObject);
        
        }

        allRoomButtons.Clear();

        roomButtonPrefab.gameObject.SetActive(false);

        foreach(KeyValuePair<string,RoomInfo> roomInfo in catchedRoomList) 
        {
            //create a new roombutton for every room info stored in the list

            RoomButton newRoomButton = Instantiate(roomButtonPrefab,roomButtonPrefab.transform.parent);
            newRoomButton.SetUpRoomInfo(roomInfo.Value);
            newRoomButton.gameObject.SetActive(true);
            allRoomButtons.Add(newRoomButton);
          
        
        }


    
    
    }

    //update current roomInfo to a temporary list
    public void UpdateCatchedRoomList(List<RoomInfo> roomList) 
    {
        //loop all room info in the list
        for(int i = 0; i < roomList.Count; i++) 
        {
            RoomInfo info = roomList[i];

            //make sure remove info when someone left the room 
            if (info.RemovedFromList) 
            {
                //remove the roominfo value within the dictionary
                myCatchedRoomList.Remove(info.Name);
            }
            else 
            {
                //otherwise, store the info into the dictionary
                myCatchedRoomList[info.Name] = info;
            
            }

        
        }



        //instantiate a new list for all the current existing buttons
        RoomListButtonUpdate(myCatchedRoomList);
    
    
    }


    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        //every time the roomlist is changed , clear all old room information and instantiate room buttons according to dictionary information
        UpdateCatchedRoomList(roomList);
    }








    #endregion


    #region Room Enter And Exit
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdateCurrentRoom();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdateCurrentRoom();
     
    }

    public override void OnLeftRoom()
    {
        CloseAllScreens();
        mainMenuButtons.SetActive(true);
    }

    public void UpdateCurrentRoom()
    {
       
        //check if the player is master client , only the master client can start game;
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                waitingMessage.text = "Ready to start the game";
                startGameButton.SetActive(true);
            }
            else
            {
                waitingMessage.text = "Waiting for player 2 to join...";
                startGameButton.SetActive(false);

            }

        }
        else
        {
            waitingMessage.text = "Waiting for player 1 to start...";
            startGameButton.SetActive(false);

        }

    }
    #endregion





    #region Menu Button Function

    public void OpenSettingScreen()
    {
        CloseAllScreens();
        SettingScreen.SetActive(true);
    }

    public void OpenCreateRoomScreen() 
    {
        CloseAllScreens();
        createRoomScreen.SetActive(true);
    }

    public void OpenFindRoomScreen() 
    {
        CloseAllScreens();
        findRoomScreen.SetActive(true);
    }

    public void ConfirmNameInput() 
    {
        //if the name input field is not null or empty
        if (!string.IsNullOrEmpty(nameInput.text)) 
        {
            PhotonNetwork.NickName = nameInput.text;
            PlayerPrefs.SetString("playerName", nameInput.text);
            hasSetName = true;


            CloseAllScreens();
            mainMenuButtons.SetActive(true);
        
        }
    
    
    }


    public void PlayerLeaveRoom() 
    {
        PhotonNetwork.LeaveRoom();
        loadingText.text = "Leaving Room...";
        loadingScreen.SetActive(true);
 
    }



    public void StartGame() 
    {
       

        //start game 
        PhotonNetwork.LoadLevel("Level_1");
    
    }

    public void BackToMain() 
    {
        CloseAllScreens();
        mainMenuButtons.SetActive(true);
        
    }




    #endregion







}
