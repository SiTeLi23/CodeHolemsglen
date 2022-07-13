using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerInteraction : MonoBehaviour
{
    PlayerController pc;
    Health health;
    public GameObject currentWeapon;
    PlayerInput inputs;
    PhotonView pv;

    [Tooltip("Settings")]
    [SerializeField] float shootTimer,PistolShotCD = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponent<PlayerController>();
        pv = GetComponent<PhotonView>();
        health = GetComponent<Health>();
        inputs = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pv.IsMine) return;

       if(health.isDead == true) { return; }
       shootTimer += Time.deltaTime;
        if (Input.GetKeyDown(inputs.fire)&&pc.isAming ==true) 
        {
           
            if (shootTimer > PistolShotCD)
            {
                //Shoot();
                pv.RPC("Shoot", RpcTarget.All);
                shootTimer=0;
            }
            
        }

        pc.anim.SetBool("Dancing", Input.GetKey(inputs.taunt));

        if (Input.GetKeyDown(inputs.aim)) 
        {

            StartAiming();
        }
    }

    [PunRPC]
    public void Shoot() 
    {

        pc.anim.SetTrigger("Shoot");
        currentWeapon.GetComponent<IShootable>().Shoot();
    }

    public void StartAiming() 
    {
        pc.isAming = !pc.isAming;
        if(pc.isAming == false) 
        {

            Invoke("WaitAnimEnd", 0.3f);
        }

    }


    public void WaitAnimEnd() 
    {
        var AnimLayerIndex = pc.anim.GetLayerIndex("UpperBody");
        pc.anim.SetLayerWeight(AnimLayerIndex, 0);

    }





}



