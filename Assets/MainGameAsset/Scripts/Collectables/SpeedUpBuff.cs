using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpBuff : MonoBehaviour
{

   [SerializeField] AudioSource pickUpSound;
 

   


    private void OnTriggerEnter(Collider other)
    {
        
        if(other.tag == "Player") 
        {
            
            var player = other.transform.GetComponent<PlayerController>();
            player.StopCoroutine("SpeedBuff");
            if (pickUpSound)
            {
                AudioSource pickFx = Instantiate(pickUpSound, transform.position, transform.rotation);
                Destroy(pickFx.gameObject, 2f);
            }

            player.StartCoroutine("SpeedBuff");
            Destroy(gameObject);
        
        }

    }



}
