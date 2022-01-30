using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StageManager : MonoBehaviour
{
    public enum STAGE_STATE {
        INIT = 0,
        WAIT,
        TEAM_MOVIE,
        GAME,
        END
    }
    
    private STAGE_STATE state = STAGE_STATE.INIT;

    private int WAIT_TIME = 3;

    private bool isGamePlaying = false;

    // Start is called before the first frame update
    void Start()
    {
        this.state = STAGE_STATE.INIT;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(PhotonNetwork.NickName);
        
        switch (this.state) {
            case STAGE_STATE.INIT:
                this.state = STAGE_STATE.WAIT;
                break;

            case STAGE_STATE.WAIT:
                this.waitForOthers();
                break;

            case STAGE_STATE.TEAM_MOVIE:
                break;
            
            case STAGE_STATE.GAME:
                break;
            
            case STAGE_STATE.END:
                this.isGamePlaying = false;
                break;
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
            if(this.WAIT_TIME - elapsedTime <= 0) {
                this.state = STAGE_STATE.TEAM_MOVIE;
                this.isGamePlaying = true;
            }
        }
    }
}
