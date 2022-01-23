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
        time = UIManager.Timer;         // 時間の更新
        maxTime = UIManager.MaxTime;    // 最大時間の設定

        UpdateFill();   // UIを更新
    }


    /// <summary>
    /// UIImageの更新処理
    /// </summary>
    public void UpdateFill ()
    {
        timerImage.fillAmount = time / maxTime;
    }
}
