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
            Debug.LogWarning("�C���X�^���X[" + typeof(IngameUIGroup) + "]��������܂���B\n�V�[��[ " + SceneManager.GetActiveScene().name + " ]�ɑΏۂ̃C���X�^���X�����݂��邩�m�F���Ă��������B");
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
            Debug.LogWarning("�C���X�^���X[" + typeof(IngameUIGroup) + "]��������܂���B\n�V�[��[ " + SceneManager.GetActiveScene().name + " ]�ɑΏۂ̃C���X�^���X�����݂��邩�m�F���Ă��������B");
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
            Debug.LogWarning("�C���X�^���X[" + typeof(MovieUIGroup) + "]��������܂���B\n�V�[��[ " + SceneManager.GetActiveScene().name + " ]�ɑΏۂ̃C���X�^���X�����݂��邩�m�F���Ă��������B");
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
            Debug.LogWarning("�C���X�^���X[" + typeof(ResultUIGroup) + "]��������܂���B\n�V�[��[ " + SceneManager.GetActiveScene().name + " ]�ɑΏۂ̃C���X�^���X�����݂��邩�m�F���Ă��������B");
        }
    }
}
