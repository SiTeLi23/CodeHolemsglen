using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawn : MonoBehaviour
{
    public GameObject testCollect;
    public Vector3 center, size;
    public GameObject[] collectables;
    public float randomSpawnTime;
    public float collectableExistTime;
    public float timer;



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= randomSpawnTime)
        {
            SpawnCollectable();
            timer = 0;
        }

    }

    public void SpawnCollectable() 
    {

        Vector3 pos = center + new Vector3(Random.Range(-size.x/2,size.x/2),Random.Range(-size.y / 2, size.y / 2),Random.Range(-size.z / 2, size.z / 2));
        int randomObject = Random.Range(0, collectables.Length);
        GameObject spawnedObj= Instantiate(collectables[randomObject], pos, Quaternion.identity);
        Destroy(spawnedObj, collectableExistTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, size);
    }
}
