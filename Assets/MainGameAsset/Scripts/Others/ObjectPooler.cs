using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]//make this showup in inspector
    public class Pool 
    {
        public string tag;
        public GameObject prefab;
        public int size;
    
    }

    #region singleton
    public static ObjectPooler instance;
    private void Awake()
    {
        if (instance != null) 
        {
            Destroy(gameObject);
        }
        else 
        {

            instance = this;
        }
    }
    #endregion


    public List<Pool> pools;

    public Dictionary<string, Queue<GameObject>> poolDictionary;
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in pools) 
        {
            // for each pool we add a queue
            Queue<GameObject> objectPool = new Queue<GameObject>();
            
            //we add all objects to the queue
            for(int i=0; i<pool.size; i++) 
            {
                GameObject obj = Instantiate(pool.prefab); //instantiate the maxnumber prefab
                obj.SetActive(false);
                objectPool.Enqueue(obj);//fill in all the object to the queue
            
            }
            //add this pool to dictionary
            poolDictionary.Add(pool.tag, objectPool);
        
        }

    }


    public GameObject SpawnFromPool(string tag,Vector3 position,Quaternion rotation) 
    {
        //make sure the dictionary contain this tag key
        if (!poolDictionary.ContainsKey(tag)) 
        {
            Debug.LogWarning("Pool with tag " + tag + "doesn't exist.");
               return null; 
                
        }

        //get the appropiate queue and pool out the first element in the queue
         GameObject objectToSpawn =  poolDictionary[tag].Dequeue();   //store this element
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        //add back to queue
        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;//we can always grab the gameobject from where we spawn it
    
    }

   



}
