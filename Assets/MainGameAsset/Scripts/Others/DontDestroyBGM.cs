using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyBGM : MonoBehaviour
{
    public static DontDestroyBGM instance;

  /*  private void OnEnable()
    {
        SceneManager.sceneLoaded += OnscenLoaded;
    }*/

    public void OnscenLoaded(Scene scene,LoadSceneMode mode) 
    {
        if(scene.name == "PVPMode"|| scene.name == "Level1") 
        {
            
                Destroy(gameObject);
            
        }
    }

    private void Awake()
    {
        #region singleton
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

        SceneManager.sceneLoaded += OnscenLoaded;

    }


    

}
