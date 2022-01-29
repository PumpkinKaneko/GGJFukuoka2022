using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BowlUIGage : MonoBehaviour
{
    [SerializeField] private Image gageImage;   // ゲージの画像
    private float maxValue;                     // 最大値
    private float val;                          // 現在値

    public bool isWinner;     // 勝利フラグ


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
    /// UIImageの更新処理
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
    /// 勝利フラグの設定
    /// </summary>
    /// <param name="time">設定する時間</param>
    public void SetWinner()
    {
        isWinner = true;
        
    }
}
