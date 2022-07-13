using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class WaitForAllPlayer : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        if (LevelGameManager.instance.activePlayers.Count != 2)
        {
            return;


        }
        else if (LevelGameManager.instance.activePlayers.Count == 2)
        {
            gameObject.SetActive(false);

        }
    }


 
}
