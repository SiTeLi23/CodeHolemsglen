using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : MonoBehaviour, IShootable
{
    [Tooltip("Bullet properties")]
    [SerializeField] float range, reloadTime;
    [SerializeField] int damage, ammo, maxAmmo;
    [Tooltip("Components")]
    [SerializeField] ParticleSystem pS;
    [SerializeField] Transform[] raycastPoints;
    [SerializeField] LayerMask shootableLayer;
 

    public void Shoot()
    {
        if (ammo < 1) return;
        ammo--;

        RaycastHit hit;
        foreach(Transform raypoint in raycastPoints)
        {
            if (Physics.Raycast(raypoint.position, raypoint.forward, out hit, range, shootableLayer))
            {
                if(hit.collider.CompareTag("Player") && hit.collider.gameObject != gameObject)
                {
                    var healthScript = hit.collider.GetComponent<Health>();
                    if(healthScript == null)
                    {
                        Debug.Log("no script attached to target" + hit.collider.name);
                        return;
                    }
                    healthScript.TakeDamage(damage,GetComponentInParent<PlayerInput>().playerNum);
                }
            }
        }
        pS.Play();
        Debug.Log("BANG! I'M a ShOtGuN");
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * range);
    }
#endif
}
