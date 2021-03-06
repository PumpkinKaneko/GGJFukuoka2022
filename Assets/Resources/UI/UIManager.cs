using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager
{
    public static void PlayGameTimer(float time)
    {
        IngameUIGroup instance = IngameUIGroup.Instance;
        if(instance)
        {
            instance.PlayTimer(time);
        }
        else
        {
            Debug.LogWarning("インスタンス[" + typeof(IngameUIGroup) + "]が見つかりません。\nシーン[ " + SceneManager.GetActiveScene().name + " ]に対象のインスタンスが存在するか確認してください。");
        }
    }


    public static void TimerUpdate(float time, float limit)
    {
        IngameUIGroup instance = IngameUIGroup.Instance;
        if (instance)
        {
            instance.TimerUpdate(time, limit);
        }
        else
        {
            Debug.LogWarning("インスタンス[" + typeof(IngameUIGroup) + "]が見つかりません。\nシーン[ " + SceneManager.GetActiveScene().name + " ]に対象のインスタンスが存在するか確認してください。");
        }
    }


    public static void PlayTeamMovie(TeamState team)
    {
        MovieUIGroup instance = MovieUIGroup.Instance;
        if(instance)
        {
            instance.PlayMovie(team);
        }
        else
        {
            Debug.LogWarning("インスタンス[" + typeof(MovieUIGroup) + "]が見つかりません。\nシーン[ " + SceneManager.GetActiveScene().name + " ]に対象のインスタンスが存在するか確認してください。");
        }
    }


    public static void SetWinner(TeamState team)
    {
        ResultUIGroup instance = ResultUIGroup.Instance;
        if (instance)
        {
            instance.SetWinner(team);
        }
        else
        {
            Debug.LogWarning("インスタンス[" + typeof(ResultUIGroup) + "]が見つかりません。\nシーン[ " + SceneManager.GetActiveScene().name + " ]に対象のインスタンスが存在するか確認してください。");
        }
    }
}
