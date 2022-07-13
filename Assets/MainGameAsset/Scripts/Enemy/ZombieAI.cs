using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class ZombieAI : MonoBehaviour
{

    [Header("Behaviour Settings")]
    [SerializeField] float sightRange = 10;
    [SerializeField] float attackRange = 1f;
  

    public enum States { IDLE, CHASE, ATTACK, PATROLLING, DEAD, RANGEATTACK }
    public States currentState;
    public States defaultState;
    NavMeshAgent agent;
    EnemyHealth health;

    [Header("Battle Settings")]
    public GameObject damageBox;


    [Header("MovementSettings")]
    public bool willPatrol;
    [SerializeField] float patrolSpeed = 3.5f;
    [SerializeField] float chaseSpeed = 5f;
    public List<Vector3> waypoints = new List<Vector3>();


    int currentWaypoint = 0;

    [Header("TargetSettings")]
    public Transform currentTarget;
    


    [Header("OtherSettings")]
    //other settings
    public Animator anim;
    public GameObject skinMeshRender;
    public float launchForce;
    public float timer;
    Ray sightRay;
    RaycastHit hit;

    [Header("Sound FX")]
    //SoundFX
    public AudioSource audioSource;
    public AudioClip walkingClip;
    public AudioClip deathClip;
    public AudioClip beingDamagedClip;


    [Header("Range Attack")]
    public bool rangeAttack;
    public GameObject projectile;
    public Transform spawnPoint;
    



    void Start()
    {
        currentState = defaultState;
        agent = GetComponent<NavMeshAgent>();
        health = GetComponent<EnemyHealth>();
        anim = GetComponent<Animator>();



        //audioSource = GetComponent<AudioSource>();


        if (!GetComponent<PhotonView>().IsMine) return;

        //if(willPatrol)collectWaypoints();
        collectWaypoints();

        StartCoroutine(SM());
    }

    private void Update()
    {
       
    }



    IEnumerator SM()
    {
        while (true)
        {
            yield return StartCoroutine(currentState.ToString());
        }
    }
    IEnumerator IDLE()
    {
        //enter state
        yield return new WaitForSeconds(0.1f);
        //GetTargetsList()
        if (currentTarget == null)
        {

            currentTarget = LevelGameManager.instance.activePlayers[0];
        }
        //loop while state is active
        while (currentState == States.IDLE)
        {
          
           
            EnableAngent();
            CheckForPlayer();
            yield return new WaitForSeconds(2);
            currentState = States.PATROLLING;
            //audioSource.Stop();
            yield return null;
        }

        //exit the state
        yield return null;
    }
    IEnumerator CHASE()
    {
      
        //enter state
        Debug.Log("Chase");
        EnableAngent();
        agent.speed = chaseSpeed;
       // agent.SetDestination(target.position);
        //loop while state is active
        while (currentState == States.CHASE)
        {
           
            DeterminTarget();
            CheckForPlayer();
            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {
                if (currentTarget)
                {
                    agent.SetDestination(currentTarget.position);
                }
            }
           

            anim.SetBool("Moving", true);





            yield return new WaitForEndOfFrame();
        }

        //exit the state
        yield return null;
    }
    IEnumerator ATTACK()
    { 
        //yield return new WaitForSeconds(0.1f);
        //enter state
        Debug.Log("attack");
        EnableAngent();
        agent.speed = 0;
        //agent.SetDestination(transform.position);
        //loop while state is active
        anim.SetBool("Moving", false);
       
        timer = 0;
        while (currentState == States.ATTACK)
        {

            DeterminTarget();
            anim.SetBool("Attack", true);
            if (currentTarget)
            {
                transform.LookAt(currentTarget);
            }
               

                CheckForPlayer();
        
            //insert your attack behaviour here
            yield return new WaitForEndOfFrame();
        }

        anim.SetBool("Attack", false);
        
        //exit the state
        yield return null;
    }

    IEnumerator RANGEATTACK()
    {
        
        //enter state
        Debug.Log("Range attack");
        EnableAngent();
        agent.speed = 0;
        
        //loop while state is active
        anim.SetBool("Moving", false);

        timer = 0;
        while (currentState == States.RANGEATTACK)
        {

            DeterminTarget();
            //anim.SetBool("Attack", true);
            if (currentTarget)
            {
                transform.LookAt(currentTarget);
                //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(currentTarget.position - transform.position), 5000f * Time.deltaTime);
                Debug.Log("Spawn Projectile");
                yield return new WaitForSeconds(1.3f);
                if (!health.isDead)
                {
                    PhotonNetwork.Instantiate(projectile.name, spawnPoint.position, spawnPoint.rotation);
                }



            }


            CheckForPlayer();

            //insert your attack behaviour here
            yield return new WaitForEndOfFrame();
        }

        //anim.SetBool("Attack", false);

        //exit the state
        yield return null;
    }



    IEnumerator PATROLLING()
    {
        /*//enter state
        currentTarget = null;*/
        Debug.Log("PATROLLING");
        EnableAngent();
       // yield return new WaitForSeconds(0.1f);
        agent.speed = patrolSpeed;
        agent.SetDestination(waypoints[currentWaypoint]);
        //loop while state is active
        while (currentState == States.PATROLLING)
        {



            DeterminTarget();
            CheckForPlayer();
            anim.SetBool("Moving", true);


            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
            {

                currentWaypoint = (currentWaypoint + 1) % waypoints.Count;
                agent.SetDestination(waypoints[currentWaypoint]);
            }




            yield return new WaitForEndOfFrame();
        }

        //exit the state
        yield return null;
    }
    IEnumerator DEAD()
    {
        currentState = States.DEAD;
        //loop while state is active
        while (currentState == States.DEAD)
        {
            anim.SetBool("Moving", false);
            //add any death animations here
            yield return new WaitForSeconds(0.5f);
            anim.SetBool("Death",true);
            DisableAngent();
            yield return new WaitForSeconds(3);
            Destroy(gameObject);
            yield return null;
        }

        //exit the state
        yield return null;
    }



    private void CheckForPlayer()
    {
      /*  if (currentTarget == null)
        {
            DeterminTarget();
        }*/
        if(currentTarget == null) 
        {
            return;
        }
        var gap = Vector3.Distance(transform.position, currentTarget.position);
        sightRay.direction = currentTarget.position - transform.position;
        sightRay.origin = transform.position;

       if(health.isDead == true) 
        {
            currentState = States.DEAD;
            return;
        }

        if (gap <= attackRange && currentTarget!=null)
        {
            if (rangeAttack == false)
            {
                currentState = States.ATTACK;
            }
            else if(rangeAttack == true&&projectile!=null) 
            {
                currentState = States.RANGEATTACK;
            }
        }

        else if (gap <= sightRange && currentTarget != null)
        {
           
            currentState = States.CHASE;
        }
        else
        {
            currentState = States.PATROLLING;
        }
    }
    private void collectWaypoints()
    {
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Waypoint"))
            {
                waypoints.Add(child.position);
                Destroy(child.gameObject);
            }
        }



    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
   





        if (waypoints.Count > 0)
        {
            foreach (Vector3 child in waypoints)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(child, 0.2f);
            }
        }
        else
        {
            foreach (Transform child in transform)
            {
                if (child.CompareTag("Waypoint"))
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawSphere(child.position, 0.2f);
                }
            }
        }

    }


   

    public void DeterminTarget() 
    {

        float minimumDistance = Mathf.Infinity;
        Transform nearestTarget = null;

         foreach(Transform playerPosition in LevelGameManager.instance.activePlayers) 
        {
            float distance = Vector3.Distance(transform.position, playerPosition.position);
            if (distance < minimumDistance) 
            {
                minimumDistance = distance;
                nearestTarget = playerPosition;
            }

        
        }

        currentTarget = nearestTarget;
    }



    public void DisableAngent()
    {
        agent.updatePosition = false;
        agent.updateRotation = false;

    }

    public void EnableAngent()
    {
        agent.updatePosition = true;
        agent.updateRotation = true;

    }


    #region AnimationEvenet
    IEnumerator TurnOnDamageBox() 
    {
        
        damageBox.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        damageBox.SetActive(false);
        
    
    }

  

    #endregion



}