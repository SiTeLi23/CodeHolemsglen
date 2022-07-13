using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerHealth : Health
{

    PlayerController pc;
    public PhotonView pv;
    [SerializeField] AudioSource hurtSound;
    [SerializeField] float respawnTime = 3f;
    //public Slider healthUI;


    private void Awake()
    {
       
    }

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        pc = GetComponent<PlayerController>();
        Animator anim = GetComponent<Animator>();
        anim.SetBool("Death", false);

        healthUI = LevelGameManager.instance.healthBars[0];
        UpdateHealthUI();

    }
    private void Update()
    {
        //UpdateHealthUI();
    }

   


    [PunRPC]
    public override void TakeDamage(int amount, int damager)
    {
        if (!pv.IsMine) return;
        //if (!myPhotonView.IsMine) return;
        // base.TakeDamage(amount,damager);
        if (!canBeDamaged) return;
        if (hurtSound)
        {
            hurtSound.Play();
        }
        currentHealth -= amount;
        canBeDamaged = false;
         UpdateHealthUI();
       Invoke("ResetDamage", 0.3f);
       
       
      
        if (currentHealth <= 0)
        {
            canBeDamaged = false;
            currentHealth = 0;
            UpdateHealthUI();

            //call the death function if life point become 0
            //TriggerDeath(damager);
            pv.RPC("TriggerDeath", RpcTarget.All, damager);
           
        }


    }


    [PunRPC]
    public override void TriggerDeath(int damager) 
    {
        //if (!pv.IsMine) return;

        //base.TriggerDeath(damager);
        transform.GetComponent<CharacterController>().enabled = false;
        UpdateHealthUI();

        if (FindObjectOfType<LevelGameManager>())
        {
            
            
            LevelGameManager.instance.LostLife();
            
            
        }

        Animator anim = GetComponentInChildren<Animator>();
        anim.SetBool("Death",true);
        isDead = true;
        canBeDamaged = false;
     
      
        pv.RPC("Die", RpcTarget.All);



    }

 



    [PunRPC]
    public override void Die()
    {
        //if (!pv.IsMine) return;
        if (FindObjectOfType<LevelGameManager>())
        {

            UpdateHealthUI();
            if (LevelGameManager.instance.lifeNum > 0)
            {
                //if (!pv.IsMine) return;
                //LevelGameManager.instance.SpawnPlayer(GetComponent<PlayerInput>().playerNum);
              transform.GetComponent<PlayerHealth>().enabled = false;
              transform.GetComponent<PlayerInteraction>().currentWeapon.GetComponent<Pistol>().enabled = false;

                pv.RPC("OnlineRespawn", RpcTarget.All);
             //Invoke("ReSpawn", respawnTime);

            }
            else 
            {

                LevelGameManager.instance.StartCoroutine("ToGameOverScene");
            }
           
        }

       

  
        

    }

    [PunRPC]
    public void OnlineRespawn() 
    {

        Invoke("ReSpawn", respawnTime);
    }






     public void ReSpawn()
    {

        gameObject.SetActive(false);
        if (PhotonNetwork.IsMasterClient)
        {
            gameObject.transform.position = LevelGameManager.instance.spawnPositions[0].position;
        }

        else
        {
            gameObject.transform.position = LevelGameManager.instance.spawnPositions[1].position;
        }



        isDead = false;
        currentHealth = maxHealth;
        transform.GetComponent<PlayerHealth>().enabled = true;
        transform.GetComponent<PlayerInteraction>().currentWeapon.GetComponent<Pistol>().enabled = true;
        transform.GetComponent<CharacterController>().enabled = true;
        transform.GetComponent<PlayerController>().canDodge = true;
        transform.GetComponent<PlayerController>().canBeControled = true;
        transform.GetComponent<PlayerHealth>().canBeDamaged = true;
        transform.GetComponent<PlayerInteraction>().currentWeapon.GetComponent<Pistol>().AddAmmo(transform.GetComponent<PlayerInteraction>().currentWeapon.GetComponent<Pistol>().GetMaxAmmo());
        UpdateHealthUI();

        Invoke("SetBackPlayer", 0.3f);
      
        //gameObject.SetActive(true);
     

    }


    public override void UpdateHealthUI()
    {
        if (!healthUI) return;
        if (!pv.IsMine) return;
        float amount = (float)currentHealth / (float)maxHealth;
        healthUI.value = amount;
    }

    public void ResetDamage() 
    {

        canBeDamaged = true;
        
        
    }

   public void SetBackPlayer() 
    {
        gameObject.SetActive(true);
    
    }
   




}
