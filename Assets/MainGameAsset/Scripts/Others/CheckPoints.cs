using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class CheckPoints : MonoBehaviour
{
    public Transform p1SpawnPoint;
    public Transform p2SpawnPoint;
    public TMP_Text messageBoxtext;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") 
        {

            LevelGameManager.instance.spawnPositions[0] = p1SpawnPoint;
            LevelGameManager.instance.spawnPositions[1] = p2SpawnPoint;
            messageBoxtext.text = "You have reached check point";
            messageBoxtext.GetComponentInParent<Animator>().Play("ShowMessageBox");
            GetComponent<BoxCollider>().enabled = false;
            this.enabled = false;
        }
    }
}
