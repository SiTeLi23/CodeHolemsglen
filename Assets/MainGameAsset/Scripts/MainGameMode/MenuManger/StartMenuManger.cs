using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class StartMenuManger : MonoBehaviour
{
    public int Score;
    GameData saveData = new GameData();
    public TMP_Text scoreText;
    public TMP_InputField p1NameInput;
    public TMP_InputField p2NameInput;

    private void Awake()
    {
        
        Time.timeScale = 1;
       

    }


    private void Update()
    {
        
        
       
    }


  public void PlayerNameSetting() 
    {
        if (p1NameInput && p2NameInput)
        {
            MasterManager.instance.playerNames.Clear();
            MasterManager.instance.playerNames.Add(p1NameInput.text);
            MasterManager.instance.playerNames.Add(p2NameInput.text);
            saveData.SetPlayer1Name(p1NameInput.text);
            saveData.SetPlayer2Name(p2NameInput.text);
            SaveSystem.instance.SaveGame(saveData);
        }

    }


    

    #region ButtonInteraction
    //Button Interaction function
    public void StartPVP() 
    {
        SaveSystem.instance.SaveGame(saveData);
        saveData.ResetData();
        SaveSystem.instance.SaveGame(saveData);
        PlayerNameSetting();
        SceneManager.LoadScene("PVPMode");
        
    
    }

    public void PVPLevelSelect()
    {
        SaveSystem.instance.SaveGame(saveData);
        saveData.ResetData();
        SaveSystem.instance.SaveGame(saveData);
        SceneManager.LoadScene("PVPLevel");


    }

    public void MainLevel() 
    {
      
        SaveSystem.instance.SaveGame(saveData);
        SceneManager.LoadScene("MainLevel");

    }

  
    public void LoadGame() 
    {
        
        SceneManager.LoadScene("MainModeResultScene");
        // SceneManager.LoadScene("PVPLevel");
        // saveData = SaveSystem.instance.LoadGame();
        Debug.Log("Loading Successed");

    }


    public void Level1()
    {
     
        PlayerNameSetting();
        SceneManager.LoadScene("Level_1");

    }


    public void ExitGame() 
    {
        Application.Quit();


    }

    #endregion

}
