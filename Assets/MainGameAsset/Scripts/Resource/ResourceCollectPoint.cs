using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class ResourceCollectPoint : MonoBehaviour
{
    public enum ResourceType { Wood,Metal}
    public ResourceType resourceType;
    public int numberOfResource;
    public TMP_Text resourceNumtext;
    public GameObject interactUI;
    public GameObject resourceUI;
    public bool isInrange = false;
    PlayerInput inputs;
    void Start()
    {
        interactUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isInrange == true)
        {
            if (Input.GetKeyDown(inputs.interact))
            {
                transform.GetComponent<CapsuleCollider>().enabled = false;
                switch (resourceType) 
                {
                    case ResourceType.Wood:
                        CollectWood(numberOfResource);
                        break;

                    case ResourceType.Metal:
                        CollectMetal(numberOfResource);
                        break;


                }



                interactUI.SetActive(false);
               
                Destroy(gameObject, 0.1f);



            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player" && other.GetComponent<PhotonView>().OwnerActorNr == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            if (inputs == null)
            {
                inputs = other.GetComponent<PlayerInput>();
            }
            isInrange = true;

            
                interactUI.SetActive(true);
            
        }
    }

  

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && other.GetComponent<PhotonView>().OwnerActorNr == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            isInrange = false;
            inputs = null;
            interactUI.SetActive(false);
        }
    }

    public void CollectWood(int num) 
    {
        LevelGameManager.instance.AddWood(num);
        resourceNumtext.text = numberOfResource.ToString();
        resourceUI.GetComponent<Animator>().Play("ShowResourceUI");
        isInrange = false;
    
    }

    public void CollectMetal(int num) 
    {
        LevelGameManager.instance.AddMetal(num);
        resourceNumtext.text = numberOfResource.ToString();
        resourceUI.GetComponent<Animator>().Play("ShowResourceUI");
        isInrange = false;

    }


}
