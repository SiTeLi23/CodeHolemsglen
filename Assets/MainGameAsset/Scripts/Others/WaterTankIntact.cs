using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WaterTankIntact : MonoBehaviour
{
    [SerializeField] ParticleSystem waterEjectParticle;
    [SerializeField] GameObject interactUI;
    [SerializeField] GameObject waterSound;
    [SerializeField] GameObject smokePlace;
    [SerializeField] GameObject fire;
    public bool isInrange=false;
    public bool isUsed = false;

    PhotonView pv;


    //playerInput
    PlayerInput inputs;
    void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!fire)
        {
            interactUI.SetActive(false);
        }

        if (isInrange == true) 
        {
            if (Input.GetKeyDown(inputs.interact)&&!isUsed&& inputs.pv.IsMine)
            {
                pv.RPC("OnlinePutOutFire", RpcTarget.All);

            }

        }

    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player") 
        {
            isInrange = true;
            inputs = other.GetComponent<PlayerInput>();
            if (other.GetComponent<PhotonView>().OwnerActorNr == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                interactUI.SetActive(true);
            }
           
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!fire) return;
      /*  if (other.tag == "Player") 
        {
            isInrange = true;
           
        
        
        }*/

    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isInrange = false;
            interactUI.SetActive(false);


        }
        
      
    }


    IEnumerator PutOutFire() 
    {
        
            smokePlace.SetActive(true);
            yield return new WaitForSeconds(5f);
            waterEjectParticle.Stop();
            waterSound.SetActive(false);
            Destroy(fire);
          
            yield return null;
   
       
    }

    [PunRPC]
  public void OnlinePutOutFire() 
    {

        isUsed = true;
        waterEjectParticle.Play();
        interactUI.SetActive(false);
        waterSound.SetActive(true);
        StartCoroutine("PutOutFire");



    }







}
