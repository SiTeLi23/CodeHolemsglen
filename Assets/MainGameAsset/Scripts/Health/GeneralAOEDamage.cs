using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralAOEDamage : MonoBehaviour
{

    public int damageAmount;
    public bool isHazard = false;

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            Debug.Log("Hit");
            var healthScript = other.GetComponent<Health>();
            var playerHealth = other.GetComponent<PlayerHealth>();
            if (!healthScript)
            {
                Debug.Log("no health Script attached");
                return;

            }

            //playerHealth.TakeDamage(damageAmount, other.GetComponentInParent<PlayerInput>().playerNum);
           // playerHealth.pv.RPC("TakeDamage", Photon.Pun.RpcTarget.All, damageAmount, other.GetComponentInParent<PlayerInput>().playerNum);
            healthScript.TakeDamage(damageAmount, other.GetComponentInParent<PlayerInput>().playerNum);
        }



    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (isHazard == true)
            {
                var healthScript = other.GetComponent<Health>();
                var playerHealth = other.GetComponent<PlayerHealth>();
                if (!healthScript)
                {
                    Debug.Log("no health Script attached");
                    return;

                }

                //playerHealth.TakeDamage(damageAmount, other.GetComponentInParent<PlayerInput>().playerNum);
                //playerHealth.pv.RPC("TakeDamage", playerHealth.pv.RpcTarget.All, damageAmount, other.GetComponentInParent<PlayerInput>().playerNum);
                healthScript.TakeDamage(damageAmount, other.GetComponentInParent<PlayerInput>().playerNum);
            }
        }
    }

}
