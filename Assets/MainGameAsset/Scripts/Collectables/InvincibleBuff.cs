using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleBuff : MonoBehaviour
{
    [SerializeField] AudioSource pickUpSound;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") 
        {
            other.GetComponent<PlayerController>().StopCoroutine("Invincible");
            if (pickUpSound) 
            {
                AudioSource pickFx = Instantiate(pickUpSound, transform.position, transform.rotation);
                Destroy(pickFx.gameObject, 2f);

            }


            other.GetComponent<PlayerController>().StartCoroutine("Invincible");
            Destroy(gameObject);
            

        }
    }
}
