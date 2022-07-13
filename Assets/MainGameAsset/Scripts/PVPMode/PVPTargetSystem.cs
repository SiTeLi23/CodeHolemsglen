using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PVPTargetSystem : MonoBehaviour
{
    public PlayerController pc;
    public Transform target;
    void Start()
    {
        pc = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        //CheckTargetState();
    }

    #region target system1
    /* public void CheckTargetState()
     {
         if (target)
         {
             if (target.GetComponent<Health>().isDead == true)
             {
                 target = null;

             }

         }

     }


     private void OnTriggerEnter(Collider other)
     {
         if (other.tag == "Player" && target == null && other.gameObject!=transform.parent)
         {

             target = other.transform;

         }
     }

     private void OnTriggerStay(Collider other)
     {

         if (!pc.weaponEquiped)
         {

             target = null;

         }
         if (other.tag == "Player" && target == null && other.GetComponent<Health>().canBeDamaged && other.gameObject != transform.parent)
         {

             target = other.transform;

         }


     }

     private void OnTriggerExit(Collider other)
     {
         if (other.tag == "Player")
         {

             target = null;
             pc.target = null;
         }
     }*/

    #endregion

   /* void OnTriggerEnter(Collider other)
    {
        pc.OnChildTriggerEntered(other, transform.position);
    }*/

}
