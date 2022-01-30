using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        //PhotonNetwork.IsMessageQueueRunning = false;
        //SceneManager.LoadScene("");

        
        //if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers) {
        //    PhotonNetwork.CurrentRoom.IsOpen = false;
        //}


        // ----------- Create Player Object BEGIN ---------------------
        string team = "";
        Player[] players = PhotonNetwork.PlayerList;
        if(players.Length % 2 == 0) { team = "Kinoko"; }
        else { team = "Takenoko"; }

        float rangeX = Random.Range(-10.0f, 10.0f);
        float rangeZ = Random.Range(-10.0f, 10.0f);

        /*
        GameObject player = PhotonNetwork.Instantiate(
            "Networking/" + team, 
            new Vector3(rangeX, 1f, rangeZ),
            Quaternion.identity);
        */
        
        PhotonNetwork.NickName = team;
        // ----------- Create Player Object END ---------------------
    

        // for Timer 
        if (PhotonNetwork.IsMasterClient) {
            var properties = new ExitGames.Client.Photon.Hashtable();
            properties.Add ("StartTime", PhotonNetwork.ServerTimestamp);
            PhotonNetwork.CurrentRoom.SetCustomProperties (properties);
        }  
    }

    //OnCreateRoomFailed
}
