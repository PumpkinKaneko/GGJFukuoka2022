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
}
