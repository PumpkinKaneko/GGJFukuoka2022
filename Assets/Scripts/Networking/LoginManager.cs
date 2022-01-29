using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


public class LoginManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) == true) {
            Application.Quit();
        }
    }

    void OnGUI ()
    {
        GUILayout.Label(PhotonNetwork.NetworkClientState.ToString());
    }

    public override void OnConnectedToMaster()
    {
        //base.OnConnectedToMaster();
        PhotonNetwork.JoinOrCreateRoom("GGJFukuoka2022", new RoomOptions(), TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        //base.OnJoinedRoom();

        // ----------- Create Player Object BEGIN ---------------------
        string team = "Kinoko";

        float rangeX = Random.Range(-10.0f, 10.0f);
        float rangeZ = Random.Range(-10.0f, 10.0f);

        GameObject player = PhotonNetwork.Instantiate(
            "Networking/" + team, 
            new Vector3(rangeX, 1f, rangeZ),
            Quaternion.identity);
        // ----------- Create Player Object END ---------------------
    
        // for Timer 
        if (PhotonNetwork.IsMasterClient) {
            var properties = new ExitGames.Client.Photon.Hashtable();
            properties.Add ("StartTime", PhotonNetwork.ServerTimestamp);
            PhotonNetwork.CurrentRoom.SetCustomProperties (properties);
        }
    }
}
