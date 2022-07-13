using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class HydrantIntact : MonoBehaviour
{
    [SerializeField] ParticleSystem waterEjectParticle;
    [SerializeField] GameObject interactUI;
  
    [SerializeField] GameObject waterSound;
    [SerializeField] GameObject smokePlace;
    [SerializeField] GameObject fire;
    
    public bool isInrange;
    PhotonView pv;


    //fire control
    [SerializeField] float reactiveTime;
    public bool canResPawn = false;
    public bool isUsing = false;
    public bool waterSpawning = false;
    //playerInput
    PlayerInput inputs;
    
    void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (!pv.IsMine) return;
        if (!fire)
        {
            interactUI.SetActive(false);
            waterSpawning = false;

            waterEjectParticle.Stop();
            interactUI.SetActive(false);
            waterSound.SetActive(false);
          

            GetComponent<SphereCollider>().enabled = false;
            StopCoroutine("PutOutFire");
            this.enabled = false;

            return;
        } 
        if (canResPawn == true&&fire)
        {
            StartCoroutine("ReActive");
        }

        
        if (isInrange == true&& !isUsing&&!waterSpawning&&fire!=null)
        {
            if (Input.GetKeyDown(inputs.interact)&&inputs.pv.IsMine)
            {
                pv.RPC("StartUsing", RpcTarget.All);
               
            }
          
        }
       
    }


  


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player"&& !isUsing) 
        {
            if (inputs == null)
            {
                inputs = other.GetComponent<PlayerInput>();
                
            }
            //other.GetComponent<PlayerController>().canDodge = false;
            isInrange = true;
            //isUsing = true;
            if (other.GetComponent<PhotonView>().OwnerActorNr == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                interactUI.SetActive(true);
            }
           
        }
    }

   


    private void OnTriggerExit(Collider other)
    {
        
        if (other.tag == "Player")
        {
            if (inputs != null)
            {
                if (inputs.pv.OwnerActorNr == other.GetComponent<PlayerInput>().pv.OwnerActorNr)
                {
                    if (!fire) return;
                    pv.RPC("StopUsing", RpcTarget.All);
                   /* inputs = null;
                    Invoke("ResetDodge", 3f);
                    isUsing = false;
                    isInrange = false;
                    waterSpawning = false;
                   
                    waterEjectParticle.Stop();
                    interactUI.SetActive(false);
                    waterSound.SetActive(false);
                    smokePlace.SetActive(false); 
                  
                    GetComponent<SphereCollider>().enabled = false;
                    StopCoroutine("PutOutFire");
                    if (fire.activeSelf == false)
                    {
                        if (!fire) return;
                        canResPawn = true;
                    }
                    Invoke("ResetCollider", 1f);*/
                }
            }
        }
    }

    //fire will temporayly put out
    IEnumerator PutOutFire() 
    {
        while (fire.activeSelf == true)
        {
            waterEjectParticle.Play();
            waterSound.SetActive(true);
            waterSpawning = true;
            smokePlace.SetActive(true);
            yield return new WaitForSeconds(2f);
            fire.SetActive(false);
            smokePlace.SetActive(false);
           
            yield return null;
        }
        
      
       
    }


    //fire will respawn
    IEnumerator ReActive()
    {
        
            yield return new WaitForSeconds(reactiveTime);
        if (fire)
        {
            fire.SetActive(true);
        }
            yield return null;
        

    }



    #region PunMethods

    [PunRPC]
    public void StartUsing() 
    {
        isUsing = true;
        canResPawn = false;
       
        interactUI.SetActive(false);
     
      
        StartCoroutine("PutOutFire");

    }

    [PunRPC]

    public void StopUsing() 
    {
        inputs = null;
        //Invoke("ResetDodge", 3f);
        isUsing = false;
        isInrange = false;
        waterSpawning = false;

        waterEjectParticle.Stop();
        interactUI.SetActive(false);
        waterSound.SetActive(false);
        smokePlace.SetActive(false);

        GetComponent<SphereCollider>().enabled = false;
        StopCoroutine("PutOutFire");
        if (fire.activeSelf == false)
        {
            if (!fire) return;
            canResPawn = true;
        }
        
        Invoke("ResetCollider", 1f);


    }


   //[PunRPC]
    public void ResetDodge() 
    {
        if (inputs != null)
        {
            inputs.GetComponent<PlayerController>().canDodge = true;
        }
    }


    public void ResetCollider() 
    {

        GetComponent<SphereCollider>().enabled = true;
    }





    #endregion



}
