using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PVPResultMenuManager : MonoBehaviour
{
    public TMP_Text winner;
    void Start()
    {
        ShowWinner();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowWinner() 
    {
        winner.text = "The winner is "+"Player " + PVPGameManager.instance.winner.ToString();
    
    
    }

    public void BackToStart() 
    {

        SceneManager.LoadScene("StartMenu");
    }


}
