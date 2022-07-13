using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndRegion : MonoBehaviour
{
    public List<GameObject> playersArrived = new List<GameObject>();
    public bool endLevel = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        if(playersArrived.Count == 2 &&endLevel==false) 
        {
            endLevel =true;
            foreach(GameObject player in playersArrived) 
            {
                player.GetComponent<PlayerController>().enabled = false;
            
            }


            LevelGameManager.instance.StartCoroutine("ToResultScene");
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") 
        {

            playersArrived.Add(other.gameObject);
        
        }
    }


    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Player")
        {

            playersArrived.Remove(other.gameObject);

        }
    }



    
 


}
