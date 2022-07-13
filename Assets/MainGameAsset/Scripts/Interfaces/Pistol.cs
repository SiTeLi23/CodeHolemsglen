using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour, IShootable
{
    [Tooltip("Bullet properties")]
    [SerializeField] float range, reloadTime;
    [SerializeField] int damage=30, ammo, maxAmmo;
    [Tooltip("Components")]
    [SerializeField] ParticleSystem pS, hitPS;
    [SerializeField] AudioSource shootFx;
    [SerializeField] AudioSource ammoOut;
    [SerializeField] Transform[] raycastPoints;
    [SerializeField] LineRenderer lR;
    [SerializeField] LayerMask shootableLayer;


    public void Shoot()
    {
        if (ammo < 1)
        {
            if (ammoOut)
            {
                ammoOut.Play();
            }
            return;
        }
        ammo--;
        if (shootFx)
        {
            shootFx.Play();
        }

        RaycastHit hit;
        foreach (Transform raypoint in raycastPoints)
        {
            if (Physics.Raycast(raypoint.position, raypoint.forward, out hit, range, shootableLayer))
            {
                if (hit.collider.CompareTag("Player")&&hit.collider.gameObject != gameObject || hit.collider.CompareTag("Enemy") && hit.collider.gameObject != gameObject)
                {
                    lR.SetPosition(0, raypoint.transform.position);
                    lR.SetPosition(1, hit.point);
                    lR.enabled = true;
                    var healthScript = hit.collider.GetComponent<Health>();
                    hitPS.transform.position = hit.point;
                    hitPS.Play();
                    if (healthScript == null)
                    {
                        Debug.Log("no script attached to target" + hit.collider.name);
                        return;
                    }
                    if (healthScript.canBeDamaged == true)
                    {
                        healthScript.TakeDamage(damage, GetComponentInParent<PlayerInput>().playerNum);
                        Debug.Log("damager is" + GetComponentInParent<PlayerInput>().playerNum);
                    }
                    pS.Play();
                    Invoke("TurnOffEffects", 0.25f);
                }
                else
                {
                    lR.SetPosition(0, raycastPoints[0].position);
                    lR.SetPosition(1, hit.point);
                    lR.enabled = true;
                    hitPS.transform.position = hit.point;
                    hitPS.Play();
                    pS.Play();
                    Invoke("TurnOffEffects", 0.25f);
                }
            }
            else
            {
                lR.SetPosition(0, raycastPoints[0].transform.position);
                lR.SetPosition(1, raycastPoints[0].transform.position + raycastPoints[0].forward * range);
                lR.enabled = true;
                pS.Play();
                Invoke("TurnOffEffects", 0.25f);
            }
        }
        if (GameObject.FindObjectOfType<PVPGameManager>())
        {
            PVPGameManager.instance.UpdateAmmoUI();
        }
        if (GameObject.FindObjectOfType<LevelGameManager>())
        {
            LevelGameManager.instance.UpdateAmmoUI();
        }
        Debug.Log("pEw Im A pIsTol");
    }
    void TurnOffEffects()
    {
        lR.enabled = false;
    }

    public int GetCurrentAmmo() 
    {
        return ammo;
    }

    public int GetMaxAmmo() 
    {
        return maxAmmo;
    }

    public void AddAmmo(int amount) 
    {
        ammo += amount;
        if (ammo > maxAmmo) 
        {
            ammo = maxAmmo;
        }
        LevelGameManager.instance.UpdateAmmoUI();

    }

    public void RefillAmmo() 
    {
        ammo = maxAmmo;
        LevelGameManager.instance.UpdateAmmoUI();
    
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (Transform raypoint in raycastPoints)
        {
            Gizmos.DrawLine(raypoint.position, raypoint.transform.position + raypoint.transform.forward * range);
        }
    }
#endif
}