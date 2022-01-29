using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;


public class MovieUIGroup : UIBehaviour<MovieUIGroup>
{
    [SerializeField]private VideoClip[] movies;          // 再生する動画ファイル
    [SerializeField]private VideoPlayer targetPlayer;    // 対象のVideoPlayerコンポーネント


    // Start is called before the first frame update
    void Start()
    {
        UIManager.PlayTeamMovie(TeamState.Kinoko);     // 指定した動画を再生する
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// 動画再生メソッド
    /// </summary>
    /// <param name="team">チームの状態</param>
    public override void PlayMovie(TeamState team)
    {
        switch(team)
        {
            case TeamState.Kinoko:
                targetPlayer.clip = movies[0];  // キノコ用動画をセット
                targetPlayer.Play();            // 再生

                //Debug.Log("キノコを再生します。");
                break;

            case TeamState.Takenoko:
                targetPlayer.clip = movies[1];  // タケノコ用動画をセット
                targetPlayer.Play();            // 再生

                //Debug.Log("タケノコを再生します。");
                break;
        }

        base.PlayMovie(team);
    }
}


/// <summary>
/// チームの状態遷移番号
/// </summary>
public enum TeamState
{
    Kinoko = 0,
    Takenoko
}
