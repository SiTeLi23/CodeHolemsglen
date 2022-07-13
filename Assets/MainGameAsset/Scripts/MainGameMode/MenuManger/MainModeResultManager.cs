using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class MainModeResultManager : MonoBehaviourPunCallbacks
{
    GameData saveData = new GameData();
    public TMP_Text woodnumtext;
    public TMP_Text metalnumtext;

  

    private void Awake()
    {
        //load save data
        saveData = SaveSystem.instance.LoadGame();
       
    }
    // Start is called before the first frame update
    void Start()
    {
        if (woodnumtext)
        {
            woodnumtext.text = saveData.woodNum.ToString();
        }
        if (metalnumtext)
        {
            metalnumtext.text = saveData.metalNum.ToString();
        }

        PhotonNetwork.Disconnect();
        //PhotonNetwork.LeaveRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        //SceneManager.LoadScene(0);
    }

    #region button function
    public void BackToStart()
    {

        //PhotonNetwork.AutomaticallySyncScene = false;
       
        SceneManager.LoadScene(0);

    }



    #endregion
}
