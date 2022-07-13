using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public GameObject currentWeapon;
    public GameObject weaponHand;
    public List<GameObject> reachableObjects;
    public float reach;
    public LayerMask pickableLayer;

    public void PickUpWeapon(Transform newWeapon)
    {
        DropWeapon();
        var rb = newWeapon.GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.velocity = Vector3.zero;
        

        newWeapon.parent = weaponHand.transform;
        newWeapon.localRotation = Quaternion.Euler(Vector3.zero);
        newWeapon.localPosition = Vector3.zero;

        newWeapon.GetComponent<PickUp>().canBePickedUp = false;
        currentWeapon = newWeapon.gameObject;
    }

    public void DropWeapon()
    {
        if (currentWeapon == null) return;
        var weaponScript = currentWeapon.GetComponent<MonoBehaviour>();
        weaponScript.enabled = false;
        currentWeapon.transform.parent = null;

        var rb = currentWeapon.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
        rb.useGravity = true;
        rb.AddForce(currentWeapon.transform.forward * 2 + currentWeapon.transform.up * 3, ForceMode.Impulse);
        currentWeapon.GetComponent<PickUp>().canBePickedUp = true;
        currentWeapon = null;

    }

    public void GetReachableWeapons()
    {
        reachableObjects.Clear();
        //use a sphere collision to maintain a list of weapons that are grabable and in range.
        var possibleObjects = Physics.OverlapSphere(transform.position, reach, pickableLayer);

        foreach(Collider item in possibleObjects)
        {
            if (!item.GetComponent<PickUp>().canBePickedUp) continue;
            reachableObjects.Add(item.gameObject);
        }
        //this list should reset each time the function is called.
        //the list cannot include the current weapon
    }

    public void PickClosestWeapon()
    {
        //compare each item in the weapons list. If the this item is closer than the last, set it as the newWeapon
        //once all are compared, call PickUpWeapon
        float distOfObject;
        float distOfLastObject;
        var closestWeapon = reachableObjects[0];
        distOfLastObject = Vector3.Distance(transform.position, closestWeapon.transform.position);
        for (int i = 1; i < reachableObjects.Count; i++)
        {
            distOfObject = Vector3.Distance(transform.position, closestWeapon.transform.position);
            if(distOfObject < distOfLastObject)
            {
                closestWeapon = reachableObjects[i];
            }
            distOfLastObject = distOfObject;
        }
        PickUpWeapon(closestWeapon.transform);
    }

    private void Update()
    {
        GetReachableWeapons();
    }
}
