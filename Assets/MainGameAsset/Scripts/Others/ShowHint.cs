using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowHint : MonoBehaviour
{
    public GameObject hintBox;
    public TMP_Text hintText;
    public string hint;
    Animator anim;
    void Start()
    {
        anim = hintBox.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.tag == "Player") 
        {
            hintText.text = hint;
            anim.Play("ShowHint");
           GetComponent<BoxCollider>().enabled = false;
           gameObject.SetActive(false);
        }

     

    }

}
