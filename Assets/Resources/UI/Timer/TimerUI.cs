using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// タイマーのUI
/// </summary>
public class TimerUI : MonoBehaviour
{
    //
    [SerializeField]private Image timerImage;       // タイマーの画像
    private float maxTime;                          // 設定時間
    private float time;                             // 現在時間
    

    public bool isPlaying { get; private set; }     // 再生フラグ


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
            UpdateFill();   // UIを更新
        }
    }


    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Initialize()
    {
        time = 0;
        maxTime = 0;
        isPlaying = false;
    }


    /// <summary>
    /// UIImageの更新処理
    /// </summary>
    public void UpdateFill ()
    {
        timerImage.fillAmount = time / maxTime;
    }


    /// <summary>
    /// タイマーの設定・再生
    /// </summary>
    /// <param name="time">設定する時間</param>
    public void PlayTimer(float time)
    {
        maxTime = time;
        isPlaying = true;
    }
}
