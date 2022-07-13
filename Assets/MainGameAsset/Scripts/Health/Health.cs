using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Tooltip("Health Amounts")]
    public int maxHealth, startingHealth, currentHealth;
    public bool canBeDamaged;
    public bool isDead = false;

    //components
    public Slider healthUI;
    public Gradient gradient;


    void Start()
    {
        startingHealth = maxHealth;
        currentHealth = startingHealth;
        UpdateHealthUI();
    }

   public virtual void TakeDamage(int amount, int damager = 0)//if the one who cause damage is not player , should be default 0 
    {
        if(!canBeDamaged) 
        {
            Debug.Log("Attempted to damage" + gameObject.name + " but object can not be damaged right now");
            return; 
        }
        currentHealth -= amount;
        if(currentHealth <= 0) 
        {
            currentHealth = 0;
            //call the death function if life point become 0
            TriggerDeath(damager);
        }

        //update Health UI 
        UpdateHealthUI();

    }

    public virtual void TriggerDeath(int damager) 
    {


        //death FX
        Debug.Log(gameObject.name + "has been killed");
        isDead = true;
        canBeDamaged = false;
        Invoke("Die", 2f);
        
    }

    public virtual void Die() 
    {

        Debug.Log(gameObject.name + "removed from scene");
        Destroy(gameObject);
    }

    public virtual void UpdateHealthUI() 
    {
        if (!healthUI) { return; }
        healthUI.maxValue = maxHealth;
        healthUI.value = currentHealth;
    
    }

  

}
