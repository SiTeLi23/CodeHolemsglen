using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public bool canBePickedUp;

    private void Start()
    {
        var rb = GetComponent<Rigidbody>();
        if (transform.parent) //if this has a parent, then it is most likely in the players hand.
        {
            
           
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.velocity = Vector3.zero;
            canBePickedUp = false;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.None;
            rb.useGravity = true;
            canBePickedUp = true;
        }
    }

}
