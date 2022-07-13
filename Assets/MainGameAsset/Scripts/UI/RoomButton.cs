using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;

public class RoomButton : MonoBehaviour
{
    public TMP_Text roomNameText;

    public RoomInfo roomInfo;

   
    public void SetUpRoomInfo(RoomInfo inputInfo) 
    {
        roomInfo = inputInfo;
        roomNameText.text = roomInfo.Name;

    
    }


    public void OpenRoomButton() 
    {

        StartLauncher.instance.JoinRoom(roomInfo);
    }


}
