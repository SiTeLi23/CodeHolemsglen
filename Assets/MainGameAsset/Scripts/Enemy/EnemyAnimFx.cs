using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimFx : MonoBehaviour
{
    [SerializeField] TrailRenderer handTrail;
    void Start()
    {
        handTrail = GetComponentInChildren<TrailRenderer>();
    }

    
    IEnumerator AttackTrail() 
    {
        if (handTrail)
        {
            handTrail.enabled = true;
            yield return new WaitForSeconds(handTrail.time);
            handTrail.enabled = false;
        }
    
    }

}
