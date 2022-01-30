using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DebugTimer : MonoBehaviour
{
    private int PLAY_TIME = 300;
    private int remainingTime = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.InRoom) { return; }

        var startTime = PhotonNetwork.CurrentRoom.CustomProperties["StartTime"];
        if (startTime != null) {
            float startTime_f = float.Parse(System.Convert.ToString(startTime));
            this.remainingTime = this.PLAY_TIME
                - (int)((PhotonNetwork.ServerTimestamp - startTime_f) * 0.001f); // 0.001f means millisec
        }

        //Debug.Log(this.GetCurrentTimeRate());

        if (this.remainingTime < 0) {
            this.remainingTime = 0;
        }
    }

    public float GetCurrentTimeRate(){
        return (float)this.remainingTime / (float)this.PLAY_TIME;
    }

    void OnGUI ()
    {
        GUI.Label(new Rect(0, 100, 100, 200), this.remainingTime.ToString());
    }
}
