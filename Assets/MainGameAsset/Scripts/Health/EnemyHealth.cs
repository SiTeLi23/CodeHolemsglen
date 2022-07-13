using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class EnemyHealth : Health
{
    PhotonView pv;
   
    //public Slider healthUI;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }
    private void Update()
    {
        UpdateHealthUI();
    }

    [PunRPC]
    public override void TakeDamage(int amount, int damager)
    {
        base.TakeDamage(amount,damager);
        canBeDamaged = false;
        Invoke("ResetDamage", 0.2f);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            //call the death function if life point become 0
            //TriggerDeath(damager);
            pv.RPC("TriggerDeath", RpcTarget.All, damager);
        }
    }

    [PunRPC]
    public override void TriggerDeath(int damager) 
    {
        base.TriggerDeath(damager);
        Animator anim = GetComponentInChildren<Animator>();
        anim.SetBool("Death",true);
        healthUI.gameObject.SetActive(false);
        LevelGameManager.instance.activeEnemies.Remove(transform);
        Invoke("EnemyDie", 1.5f);


    }


    public void EnemyDie() 
    {
        pv.RPC("Die", RpcTarget.All);

    }


    [PunRPC]
    public override void Die()
    {
        
        Destroy(gameObject);
       //PhotonNetwork.Destroy(gameObject);
    }



    public override void UpdateHealthUI()
    {
        if (!healthUI) return;
        float amount = (float)currentHealth / (float)maxHealth;
        healthUI.value = amount;
    }

    public void ResetDamage() 
    {

        canBeDamaged = true;
    }


}
