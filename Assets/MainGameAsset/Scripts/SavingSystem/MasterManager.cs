using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterManager : MonoBehaviour
{
    public static MasterManager instance;
    private void Awake()
    {
        #region singleton pattern
        if (instance != null) 
        {

            Destroy(gameObject);
        }
        else 
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
            
        
        }



        #endregion
       

    }

    //get a blank save file,this can be overrident by loading
    GameData saveData = new GameData();

    public List<string> playerNames = new List<string>();
    //public int roundTime;


}
