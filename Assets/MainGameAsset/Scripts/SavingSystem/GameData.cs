using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    //save the information
    public string player1Name, player2Name;
    public int score = 0;
    public Dictionary<string, int> playerScore = new Dictionary<string, int>();
    public int woodNum = 0,metalNum =0;


    public void AddScore(int points) 
    {

        score += points;
    }
    public void ResetData() 
    {

        score = 0;
        woodNum = 0;
        metalNum = 0;
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



    #region SettingPlayerName
    public void SetPlayer1Name(string name) 
    {
        player1Name = name;
      
    }
    public void SetPlayer2Name(string name)
    {
        player2Name = name;

    }



    #endregion

}
