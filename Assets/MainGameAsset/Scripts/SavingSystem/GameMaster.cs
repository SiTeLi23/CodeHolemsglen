using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    GameData saveData = new GameData();
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) 
        {
            saveData.AddScore(1);
            PrintScore();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            saveData.AddScore(-1);
            PrintScore();
        }


        if (Input.GetKeyDown(KeyCode.F5)) 
        {
            SaveSystem.instance.SaveGame(saveData);
            Debug.Log("Saving data");
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            saveData = SaveSystem.instance.LoadGame();
            Debug.Log("loading data");
            PrintScore();
        }
        if (Input.GetKeyDown(KeyCode.F12))
        {
            saveData.ResetData();
            Debug.Log("Reset Data");
            PrintScore();
        }

    }


    void PrintScore() 
    {
        Debug.Log("The current score is " + saveData.score);
    }
}
