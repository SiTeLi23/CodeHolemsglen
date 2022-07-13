using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;



public class PlayerController : MonoBehaviourPunCallbacks
{

    [Tooltip("Movement Values")]
    [SerializeField] float speedMultiplier, rotationSpeed, gravityForce, jumpForce, dodgeSpeed, dodgeTime, dodgeTimer, dodgeCD,pushForce;
    public bool canBeControled = true;
    public bool canDodge = true;
    [Tooltip("FX")]
    public ParticleSystem walkingDust;
    public ParticleSystem invincible;

    [Tooltip("BuffSettings")]
    [SerializeField] float startSpeed;
    [SerializeField] float invincibleTime = 5;


    //Components
    CharacterController cc;
    Health health;
    public Animator anim;
    PhotonView pv;
    
    
   

    
   

    Vector3 movementDirection; //keep track the input
    Vector3 playerVelocity;//gravity direction

    [SerializeField]bool groundedPlayer;

    public bool isAming = false;
    public Transform currentTarget;
    public List<Transform> targets = new List<Transform>();
    public float detectRange = 60f;
    public LayerMask targettableLayer;
    

    PlayerInput inputs;


    void Start()
    {
        cc = GetComponent<CharacterController>();
        //anim = GetComponentInChildren<Animator>();
        anim = GetComponent<Animator>();
        health = GetComponent<Health>();
        inputs = GetComponent<PlayerInput>();
        pv = GetComponent<PhotonView>();
        //starting property
        startSpeed = speedMultiplier;
        
        
    }

    
    void Update()
    {
        if (!pv.IsMine) return;
        

        if(health.isDead == true||canBeControled==false) { return; }
        groundedPlayer = cc.isGrounded;

        

       //SET TARGET
        //GetPotentialTarget();
      
        //Jumping Handler
        if(groundedPlayer && playerVelocity.y<0)
        {
            //if the animation is running ,set jumping state to false by default
            if (anim.GetBool("Jumping")) { anim.SetBool("Jumping", false); }
            //if we are on the ground , and velocity is not 0,reset the gravity to 0
            playerVelocity.y = 0;
        }

        //detect if character has taken out weapon
        AimingHandler();
        //Movement controller
        MoveMent();
      
           

       

    }

    #region MovementController
    public void MoveMent() 
    {

        //read input value
        var h = Input.GetAxis(inputs.horizontal);
        var v = Input.GetAxis(inputs.vertical);

        //character move based on keyboard input
        if (h != 0 || v != 0)
        {
            movementDirection.Set(h, 0, v);
            if (!isAming)
            {
                cc.Move(movementDirection * speedMultiplier * Time.deltaTime);
            }
            else 
            {
                cc.Move(movementDirection * speedMultiplier*0.5f * Time.deltaTime);
            }
            anim.SetBool("HasInput", true);
            if (walkingDust && groundedPlayer)
            {
                walkingDust.gameObject.SetActive(true);
                walkingDust.Play();
            }


        }
        else
        {

            anim.SetBool("HasInput", false);
            if (walkingDust)
            {
                walkingDust.Stop();
            }
        }

        /*//normal rotation without target system
        var desiredDirection = Quaternion.LookRotation(movementDirection); //create an angle we can use

        transform.rotation = Quaternion.Lerp(transform.rotation, desiredDirection, rotationSpeed * Time.deltaTime);*/

        //if weapon euqipped and has target , aiming rotate to target
        if (isAming)
        {
            DeterminTarget();
            if (currentTarget == null)
            {
                var desiredDirection = Quaternion.LookRotation(movementDirection); //create an angle we can use

                transform.rotation = Quaternion.Lerp(transform.rotation, desiredDirection, rotationSpeed * Time.deltaTime);



            }
            else
            {
                Vector3 direction = currentTarget.position - transform.position;
                direction.y = 0;
                Quaternion aimRotation = Quaternion.LookRotation(direction);

                //transform.rotation = Quaternion.Lerp(transform.rotation, aimRotation * Quaternion.Euler(0, -8f, 0), rotationSpeed * Time.deltaTime);
                transform.rotation = aimRotation * Quaternion.Euler(0, -8f, 0);
            }

        }
        //if don't have weapon or not has target , rotate normally
        if (!isAming || !currentTarget)
        {

            var desiredDirection = Quaternion.LookRotation(movementDirection); //create an angle we can use

            transform.rotation = Quaternion.Lerp(transform.rotation, desiredDirection, rotationSpeed * Time.deltaTime); // rotate the player to that angle gradually
        }


        var animationVector = transform.InverseTransformDirection(cc.velocity); // model relative to it's own direction,like where is the left of the character
        anim.SetFloat("ForwardMomentum", animationVector.z);
        anim.SetFloat("SideMomentum", animationVector.x);

        ProcessGravity();

        //dodge roll
        dodgeTimer -= Time.deltaTime;
        if (Input.GetKeyDown(inputs.roll) && groundedPlayer == true&&canDodge)
        {
            if (dodgeTimer < 0)
            {

                pv.RPC("OnlineDodge", RpcTarget.All);
                
            }
           

        }

        else 
        {

            anim.SetBool("Roll", false);
        }

    }
    #endregion




    //gravity hanndler
    public void ProcessGravity() 
    {
        if(Input.GetKeyDown(inputs.jump)&&groundedPlayer) 
        {
            anim.SetBool("Jumping", true);
            playerVelocity.y += Mathf.Sqrt(jumpForce * -3.0f *gravityForce);
            if (walkingDust) 
            {
                walkingDust.gameObject.SetActive(false);
            
            }
        
        }


        playerVelocity.y += gravityForce * Time.deltaTime; //gravity force slowly increasing and drag player down, so the player won't jumping forever
        cc.Move(playerVelocity * Time.deltaTime);
    
    }

    //weapon equipped aniation handler
    public void AimingHandler() 
    {
       
        if (isAming == true)
        {
            var AnimLayerIndex = anim.GetLayerIndex("UpperBody");
            anim.SetBool("Aiming", true);
            anim.SetLayerWeight(AnimLayerIndex, 1);
        }

        else 
        {
            anim.SetBool("Aiming", false);
          
        }

    }

    


    #region TargetSystem


    public void DeterminTarget() 
    {

        if (FindObjectOfType<LevelGameManager>())
        {
             float minimunDistance = Mathf.Infinity;
        currentTarget = null;

         foreach(Transform enemy in LevelGameManager.instance.activeEnemies) 
         {
            float distance = Vector3.Distance(transform.position, enemy.position);
            if (distance < minimunDistance&&distance<=detectRange) 
            {
                minimunDistance = distance;
                currentTarget = enemy;
            }
        
        
         }
        }

        if (FindObjectOfType<PVPGameManager>())
        {
        
           foreach(GameObject enemyPlayer in PVPGameManager.instance.activePlayers) 
            {
                
                if(enemyPlayer!= this.gameObject) 
                {
                    float distance = Vector3.Distance(transform.position, enemyPlayer.transform.position);

                    if (distance <= detectRange)
                    {
                        currentTarget = enemyPlayer.transform;
                    }
                    else 
                    {

                        currentTarget = null;
                    }
                }
            
            }
        
        
        }



     }

    #endregion

    #region  BuffSystem

  IEnumerator SpeedBuff() 
    {
       
        speedMultiplier = 3f;
        yield return new WaitForSeconds(5f);
        speedMultiplier = startSpeed;
        yield return null;
    
    
    }

  IEnumerator Invincible() 
    {
        if (invincible)
        {
            health.canBeDamaged = false;
            
            invincible.Play();
            health.Invoke("ResetDamage", invincibleTime);
            yield return new WaitForSeconds(invincibleTime);
            invincible.Stop();
            yield return null;
        }

    }





    #endregion

 

    IEnumerator Dodge() 
    {
        
        float startTime = Time.time;
         while(Time.time<startTime + dodgeTime) 
         {
            
            cc.Move(movementDirection * dodgeSpeed * Time.deltaTime);
            //yield return new WaitForSeconds(0.2f);
           
            yield return null;
         }
      
      
       
    }





    #region  PUN FUNCTIONS
    [PunRPC]
    public void UpdatePlayerToList()
    {
        LevelGameManager.instance.activePlayers.Clear();
        LevelGameManager.instance.playersInScene = GameObject.FindGameObjectsWithTag("Player");
        Transform[] playerTransform = new Transform[LevelGameManager.instance.playersInScene.Length];


        for (int i = 0; i < LevelGameManager.instance.playersInScene.Length; i++)
        {
            playerTransform[i] = LevelGameManager.instance.playersInScene[i].transform;
        }


        LevelGameManager.instance.activePlayers.AddRange(playerTransform);
        LevelGameManager.instance.UpdateAmmoUI();
    }

    [PunRPC]
    public void OnlineDodge() 
    {
        dodgeTimer = dodgeCD;
        //anim.Play("Roll");
        anim.SetBool("Roll", true);
        GetComponent<PlayerAnimFx>().StartCoroutine("DodgeInvincible");
        StartCoroutine("Dodge"); //we need to use coroutine,otherwise this will only play one frame withinUpdate


    }




    #endregion



    #region Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }


    #endregion


}
