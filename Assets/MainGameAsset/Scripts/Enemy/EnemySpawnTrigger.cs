using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class EnemySpawnTrigger : MonoBehaviour
{
    
    public List<Transform> enemies = new List<Transform>();
    PhotonView pv;

    void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") 
        {

            pv.RPC("ActiveEnemy", RpcTarget.All);
        
        }
    }


    [PunRPC]
    public void ActiveEnemy() 
    {
        foreach (Transform enemy in enemies)
        {
            enemy.gameObject.SetActive(true);
            LevelGameManager.instance.activeEnemies.Add(enemy);

        }

        transform.GetComponent<BoxCollider>().enabled = false;
        Destroy(gameObject);

    }


}
