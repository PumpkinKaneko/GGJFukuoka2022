using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStageManager : MonoBehaviour
{
    
    [SerializeField]
    private List<GameStageController> gameStages;

    private GameStageController curGameStage;

    // 経過時間
    private float elapsedTime = 0f;

    // ステージのロードが終了しているか
    public bool isLoadedStage = false;
    
    // ゲームステージの読み込み処理
    public void LoadGameStage(int index,Action onLoaded)
    {
        onLoaded?.Invoke();
    }

    // ゲーム開始
    public void GameStart()
    {
        
    }
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
