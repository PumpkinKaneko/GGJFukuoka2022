using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviourPunCallbacks
{
    public enum STAGE_STATE {
        INIT = 0,
        TITLE,
        LOGIN,
        WAIT,
        TEAM_MOVIE,
        GAME,
        END
    }

    [SerializeField]
    private string kinokoPrefabName;

    [SerializeField]
    private string takenokoPrefabname;

    [SerializeField]
    private int maxPlayer = 2;
    
    public STAGE_STATE state = STAGE_STATE.INIT;

    private int WAIT_TIME = 10;

    private bool isGamePlaying = false;

    private float movieTimer = 0;

    // Start is called before the first frame update
    void Start()
    {
        this.state = STAGE_STATE.INIT;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(PhotonNetwork.NickName);
        
        switch (this.state) {
            case STAGE_STATE.INIT:
                this.isGamePlaying = false;
                this.movieTimer = 0;
                
                SoundManager.Instance.PlayBGM(AudioState.BGM_2);
                
                GameObject tc = GameObject.Find("TitleCanvas");
                tc.GetComponent<Canvas>().enabled = true;
                GameObject wc = GameObject.Find("WaitCanvas");
                wc.GetComponent<Canvas>().enabled = false;
                GameObject mc = GameObject.Find("MovieCanvas");
                mc.GetComponent<Canvas>().enabled = false;
                
                //GameObject.Find("MovieCanvas").SetActive(true);
                //GameObject.Find("MovieCamera").SetActive(true);

                this.state = STAGE_STATE.TITLE;
                break;

            case STAGE_STATE.TITLE:
                //this.state = STAGE_STATE.LOGIN;
                break;

            case STAGE_STATE.LOGIN:
                tc = GameObject.Find("TitleCanvas");
                tc.GetComponent<Canvas>().enabled = false;
                wc = GameObject.Find("WaitCanvas");
                wc.GetComponent<Canvas>().enabled = true;

                PhotonNetwork.ConnectUsingSettings();
                this.state = STAGE_STATE.WAIT;
                break;

            case STAGE_STATE.WAIT:
                if (PhotonNetwork.PlayerList.Length >= maxPlayer)
                {
                    this.state = STAGE_STATE.TEAM_MOVIE;
                }
                // this.waitForOthers();
                break;

            case STAGE_STATE.TEAM_MOVIE:
                wc = GameObject.Find("WaitCanvas");
                wc.GetComponent<Canvas>().enabled = false;

                mc = GameObject.Find("MovieCanvas");
                mc.GetComponent<Canvas>().enabled = true;

                this.showMovie();
                this.movieTimer += Time.deltaTime;
                if (this.movieTimer >= 15) {
                    mc = GameObject.Find("MovieCanvas");
                    mc.GetComponent<Canvas>().enabled = false;
                    
                    // ゲーム開始
                    GameStageManager.Instance.GameStart();
                    this.state = STAGE_STATE.GAME;
                }
                break;
            
            case STAGE_STATE.GAME:
                if (!GameStageManager.Instance.isGamePlaying)
                {
                    PhotonNetwork.Disconnect();
                    this.state = STAGE_STATE.END;
                }
                break;
            
            case STAGE_STATE.END:
                SceneManager.LoadScene("Result");
                this.isGamePlaying = false;
                break;
        }

        // Quit
        if (Input.GetKeyDown(KeyCode.Escape) == true) {
            Application.Quit();
        }

        //Debug.Log(this.state);
    }

    void waitForOthers () {
        if (this.isGamePlaying == true) { return; }

        if (!PhotonNetwork.InRoom) { return; }
        var startTime = PhotonNetwork.CurrentRoom.CustomProperties["StartTime"];
        if (startTime != null) {
            float startTime_f = float.Parse(System.Convert.ToString(startTime));
            int elapsedTime = (int)((PhotonNetwork.ServerTimestamp - startTime_f)*0.001f); // 0.001f means millisec
            //Debug.Log(elapsedTime);
            if(this.WAIT_TIME - elapsedTime <= 0) {
                this.state = STAGE_STATE.TEAM_MOVIE;
                this.isGamePlaying = true;
            }
        }
    }

    void showMovie () {
        if (PhotonNetwork.NickName == kinokoPrefabName ) { UIManager.PlayTeamMovie(TeamState.Kinoko); }
        else { UIManager.PlayTeamMovie(TeamState.Takenoko); }
    }

    public void doLogin() {
        this.state = STAGE_STATE.LOGIN;
    }

    public STAGE_STATE getStageState() {
        return this.state;
    }

    public override void OnConnectedToMaster()
    {
        //base.OnConnectedToMaster();
        RoomOptions options = new RoomOptions();
        options.PublishUserId = true;
        PhotonNetwork.JoinOrCreateRoom("GGJFukuoka2022", options, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.PlayerList.Length > maxPlayer)
        {
            this.state = STAGE_STATE.LOGIN;
            return;
        }
        
        // ----------- Create Player Object BEGIN ---------------------
        string team = "";
        Player[] players = PhotonNetwork.PlayerList;
        if(players.Length % 2 == 0) { team = kinokoPrefabName; }
        else { team = takenokoPrefabname; }

        float rangeX = Random.Range(-10.0f, 10.0f);
        float rangeZ = Random.Range(-10.0f, 10.0f);

        GameObject player = PhotonNetwork.Instantiate(
            "Networking/" + team, 
            new Vector3(rangeX, 1f, rangeZ),
            Quaternion.identity);

        PhotonNetwork.NickName = team;
        // ----------- Create Player Object END ---------------------
    

        // for Timer 
        if (PhotonNetwork.IsMasterClient) {
            var properties = new ExitGames.Client.Photon.Hashtable();
            properties.Add ("StartTime", PhotonNetwork.ServerTimestamp);
            PhotonNetwork.CurrentRoom.SetCustomProperties (properties);
        }  
    }
}
