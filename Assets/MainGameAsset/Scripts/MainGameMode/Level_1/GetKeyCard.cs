using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class GetKeyCard : MonoBehaviour
{
    public GameObject interactUI;
    public GameObject targetDoor;
    public bool isInrange = false;
    public TMP_Text messageBoxtext;
    public GameObject currentUser;
    PlayerInput inputs;
    PhotonView pv;

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
    }
    void Start()
    {
        interactUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isInrange == true)
        {
            if (Input.GetKeyDown(inputs.interact) && inputs.pv.IsMine)
            {

                pv.RPC("PickUpCard", RpcTarget.All);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Player") 
        {
            if (inputs == null)
            {
                inputs = other.GetComponent<PlayerInput>();
                currentUser = other.gameObject;
            }
            isInrange = true;
            
            if(other.GetComponent<PhotonView>().OwnerActorNr == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                interactUI.SetActive(true);
            }
        }
    }



    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && currentUser!=null)
        {
            isInrange = false;
            inputs = null;
            currentUser = null;
            interactUI.SetActive(false);
        }
    }


    [PunRPC]
    public void PickUpCard() 
    {
        targetDoor.GetComponent<TheDoor>().islocked = false;
        interactUI.SetActive(false);
        isInrange = false;
        transform.GetComponent<SphereCollider>().enabled = false;
        messageBoxtext.text = "You have found a hidden key card";
        messageBoxtext.GetComponentInParent<Animator>().Play("ShowMessageBox");


    }




}
