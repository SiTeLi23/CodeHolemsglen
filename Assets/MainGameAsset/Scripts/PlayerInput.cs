using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerInput : MonoBehaviour
{
   
    public int playerNum = 1;
    [HideInInspector]
    public KeyCode jump, fire, taunt, interact ,aim,roll;
    [HideInInspector]
    public string horizontal, vertical;

    public TMP_Text playerName;
    public PhotonView pv;
    

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        DeterminInput();

        //pv.RPC("UpdateUI", RpcTarget.All);
        UpdateUI();
    }
    public void DeterminInput() 
    {
        switch (playerNum) 
        {

            case 1:
                vertical = "P1Vertical";
                horizontal = "P1Horizontal";
                jump = KeyCode.Space;
                interact = KeyCode.E;
                fire = KeyCode.Mouse0;
                taunt = KeyCode.R;
                aim = KeyCode.Q;
                roll = KeyCode.LeftShift;
                break;

            case 2:
                vertical = "P2Vertical";
                horizontal = "P2Horizontal";
                jump = KeyCode.RightAlt;
                interact = KeyCode.Comma;
                fire = KeyCode.Slash;
                taunt = KeyCode.L;
                aim = KeyCode.RightShift;
                roll = KeyCode.Period;
                break;
        
        }

           
    
    }

    //[PunRPC]
    public void UpdateUI() 
    {
       
            
            playerName.text = pv.Owner.NickName;
            
        
    }



}
