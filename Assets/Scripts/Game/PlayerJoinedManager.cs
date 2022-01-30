using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerJoinedManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private string prefabName;
    
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnConnectedToMaster()
    {
        RoomOptions option = new RoomOptions();
        option.PublishUserId = true;
        //base.OnConnectedToMaster();
        PhotonNetwork.JoinOrCreateRoom("GGJFukuoka2022", option, TypedLobby.Default);
    }
    
    // ゲームにログインしたとき
    public override void OnJoinedRoom()
    {
        GameObject player = 
            PhotonNetwork.Instantiate("Networking/"+prefabName,new Vector3(0,0,0),Quaternion.identity);
        
        // for Timer 
        if (PhotonNetwork.IsMasterClient) {
            var properties = new ExitGames.Client.Photon.Hashtable();
            properties.Add ("StartTime", PhotonNetwork.ServerTimestamp);
            PhotonNetwork.CurrentRoom.SetCustomProperties (properties);
        }
    }
}
