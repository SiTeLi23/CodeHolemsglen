using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerAnimFx : MonoBehaviour
{
    [SerializeField] AudioSource footStepFx;
   
    Health health;
    Animator anim;
    PlayerController pc;
    TrailRenderer tr;

    [SerializeField] float InvincibleTime=1f;


    private void Start()
    {
        health = GetComponent<Health>();
        anim = GetComponent<Animator>();
        pc = GetComponent<PlayerController>();
        tr = GetComponent<TrailRenderer>();
    }
    public void Step() 
    {
        
        if (!footStepFx) return;
        if (health.isDead == true) return;
     
        if (anim.GetBool("HasInput") == false || anim.GetBool("Jumping")==true || health.isDead == true)
        {
            anim.enabled = false;
            anim.enabled = true;
            footStepFx.Stop();
            return; 
        }
        if (transform.GetComponent<PhotonView>().IsMine)
        {
            footStepFx.Play();
        }
      
    }

 

   IEnumerator DodgeInvincible() 
    {
        health.canBeDamaged = false;
        yield return new WaitForSeconds(InvincibleTime);
        health.canBeDamaged = true;
        yield return null;
    
    }



    IEnumerator ShowTrail() 
    {
        tr.enabled = true;
        yield return new WaitForSeconds(0.4f);
        tr.enabled = false;

    
    }


}
