using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    [SerializeField] int ammoAmount=10;
    [SerializeField] AudioSource ammoPickUpSound;
    [SerializeField] GameObject boxModel;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") 
        {
           // boxModel.SetActive(false);
            
            other.GetComponent<PlayerInteraction>().currentWeapon.GetComponent<Pistol>().AddAmmo(ammoAmount);
            if (ammoPickUpSound) 
            {
                AudioSource collectFx = Instantiate(ammoPickUpSound,transform.position, Quaternion.identity);
                collectFx.Play();
                Destroy(collectFx.gameObject, 2f);
            }
            //if this is pvp mode,do this
            if (GameObject.FindObjectOfType<PVPGameManager>())
            {
                PVPGameManager.instance.UpdateAmmoUI();
            }
            if (GameObject.FindObjectOfType<LevelGameManager>())
            {
                LevelGameManager.instance.UpdateAmmoUI();
            }

            Destroy(gameObject);
        }

    }
}
