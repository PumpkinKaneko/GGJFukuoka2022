using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BowlUIGage : MonoBehaviour
{
    [SerializeField] private Image gageImage;   // �Q�[�W�̉摜
    private float maxValue;                     // �ő�l
    private float val;                          // ���ݒl

    public bool isWinner;     // �����t���O


    void Awake()
    {
        Initialize();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateFill();
    }


    public void Initialize()
    {
        maxValue = 3;
        isWinner = false;
    }


    /// <summary>
    /// UIImage�̍X�V����
    /// </summary>
    public void UpdateFill()
    {
        if (isWinner)
        {
            gageImage.fillAmount = Mathf.Lerp(gageImage.fillAmount, (maxValue / 2) / maxValue, 1f * Time.deltaTime);
        }
        else
        {
            gageImage.fillAmount = Mathf.Lerp(gageImage.fillAmount, maxValue / maxValue, 1f * Time.deltaTime);
        }
    }


    /// <summary>
    /// �����t���O�̐ݒ�
    /// </summary>
    /// <param name="time">�ݒ肷�鎞��</param>
    public void SetWinner()
    {
        isWinner = true;
        
    }
}
