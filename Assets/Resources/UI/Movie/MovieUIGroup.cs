using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;


public class MovieUIGroup : UIBehaviour<MovieUIGroup>
{
    [SerializeField]private VideoClip[] movies;          // �Đ����铮��t�@�C��
    [SerializeField]private VideoPlayer targetPlayer;    // �Ώۂ�VideoPlayer�R���|�[�l���g


    // Start is called before the first frame update
    void Start()
    {
        //UIManager.PlayTeamMovie(TeamState.Takenoko);     // �w�肵��������Đ�����
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// ����Đ����\�b�h
    /// </summary>
    /// <param name="team">�`�[���̏��</param>
    public override void PlayMovie(TeamState team)
    {
        switch(team)
        {
            case TeamState.Kinoko:
                targetPlayer.clip = movies[0];  // �L�m�R�p������Z�b�g
                targetPlayer.Play();            // �Đ�

                //Debug.Log("�L�m�R���Đ����܂��B");
                break;

            case TeamState.Takenoko:
                targetPlayer.clip = movies[1];  // �^�P�m�R�p������Z�b�g
                targetPlayer.Play();            // �Đ�

                //Debug.Log("�^�P�m�R���Đ����܂��B");
                break;
        }

        base.PlayMovie(team);
    }
}


/// <summary>
/// �`�[���̏�ԑJ�ڔԍ�
/// </summary>
public enum TeamState
{
    Kinoko = 0,
    Takenoko
}
