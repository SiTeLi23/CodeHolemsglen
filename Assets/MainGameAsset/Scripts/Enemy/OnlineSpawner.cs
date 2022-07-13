using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OnlineSpawner : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();
    public Transform[] spawnPoints;
    PhotonView pv;

    void Start()
    {
        pv = GetComponent<PhotonView>();
    }

  

    private void OnTriggerEnter(Collider other)
    {


        if (other.tag == "Player") 
        {
            if (!pv.IsMine) return;
            SpawnEnemies();
        
        }
    }

  
    public void SpawnEnemies() 
    {
        //spawn enemy only on masterclient side
        pv.RPC("OnlineSpawn", RpcTarget.MasterClient);
        //update enemy to game manager list on both sides
        pv.RPC("UpdateEnemyList", RpcTarget.All);
        //destroy trigger on both sides
        PhotonNetwork.Destroy(gameObject);

    }

    [PunRPC]
    public void OnlineSpawn() 
    {

        for (int i = 0; i < enemies.Count; i++)
        {
            int randomPos = Random.Range(0, spawnPoints.Length);
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate(enemies[i].name, spawnPoints[randomPos].position, Quaternion.identity);
                
            }
           
        }

    }

    [PunRPC]
    public void UpdateEnemyList() 
    {
        LevelGameManager.instance.activeEnemies.Clear();
        GameObject[] enemyprefab = GameObject.FindGameObjectsWithTag("Enemy");
        Transform[] enemyTransform = new Transform[enemyprefab.Length];
        for (int i = 0; i < enemyprefab.Length; i++)
        {
            enemyTransform[i] = enemyprefab[i].transform;
        }

        LevelGameManager.instance.activeEnemies.AddRange(enemyTransform);


    }


}
