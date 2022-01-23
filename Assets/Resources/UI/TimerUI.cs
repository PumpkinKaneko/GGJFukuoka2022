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
    [SerializeField]private Image timerImage;
    private float maxTime;
    private float time;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        time = UIManager.Timer;         // ���Ԃ̍X�V
        maxTime = UIManager.MaxTime;    // �ő厞�Ԃ̐ݒ�

        UpdateFill();   // UI���X�V
    }


    /// <summary>
    /// UIImage�̍X�V����
    /// </summary>
    public void UpdateFill ()
    {
        timerImage.fillAmount = time / maxTime;
    }
}
