using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// �^�C�}�[��UI
/// </summary>
public class TimerUI : MonoBehaviour
{
    //
    [SerializeField]private Image timerImage;       // �^�C�}�[�̉摜
    private float maxTime;                          // �ݒ莞��
    private float time;                             // ���ݎ���
    

    public bool isPlaying { get; private set; }     // �Đ��t���O


    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }


    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            time -= Time.deltaTime;
            UpdateFill();   // UI���X�V
        }
    }


    /// <summary>
    /// ����������
    /// </summary>
    private void Initialize()
    {
        time = 0;
        maxTime = 0;
        isPlaying = false;
    }


    /// <summary>
    /// UIImage�̍X�V����
    /// </summary>
    public void UpdateFill ()
    {
        timerImage.fillAmount = time / maxTime;
    }


    /// <summary>
    /// �^�C�}�[�̐ݒ�E�Đ�
    /// </summary>
    /// <param name="time">�ݒ肷�鎞��</param>
    public void PlayTimer(float time)
    {
        maxTime = time;
        isPlaying = true;
    }
}
