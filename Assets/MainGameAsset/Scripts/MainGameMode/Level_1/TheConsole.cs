using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class TheConsole : MonoBehaviour
{
    public GameObject interactUI;
    public GameObject door;
    public AudioSource denySound;
    public bool isInrange =false;
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
            if (Input.GetKeyDown(inputs.interact) && door.GetComponent<TheDoor>().islocked == false && inputs.pv.IsMine)
            {
                pv.RPC("OpenDoor", RpcTarget.All);

            }
            else if (Input.GetKeyDown(inputs.interact) && door.GetComponent<TheDoor>().islocked == true && inputs.pv.IsMine)
            {
                Debug.Log("You need keyCard to get through it");
                denySound.Play();

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
            if (other.GetComponent<PhotonView>().OwnerActorNr == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                interactUI.SetActive(true);
            }
        }
    }

  

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player"&& currentUser != null)
        {
            isInrange = false;
            inputs = null;
            currentUser = null;
            interactUI.SetActive(false);
        }
    }


    [PunRPC]
    public void OpenDoor() 
    {
        isInrange = false;
        interactUI.SetActive(false);
        door.GetComponent<Animator>().enabled = true;
        door.GetComponent<BoxCollider>().enabled = false;
        door.GetComponent<AudioSource>().Play();
        transform.GetComponent<SphereCollider>().enabled = false;

    }

}
