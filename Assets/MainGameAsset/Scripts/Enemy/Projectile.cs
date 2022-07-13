using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Projectile : MonoBehaviour
{
    Rigidbody rb;
    PhotonView pv;
    public float moveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * moveSpeed;
        Destroy(gameObject, 5f);
    }


    

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            pv.RPC("HitPlayer", RpcTarget.All);
            //Destroy(gameObject);
        }
    }


    [PunRPC]
    public void HitPlayer() 
    {

        Destroy(gameObject);
    }



}
