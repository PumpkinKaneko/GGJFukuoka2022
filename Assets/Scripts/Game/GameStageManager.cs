using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;

public class GameStageManager : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    #region Singlton

    private static GameStageManager instance;

    public static GameStageManager Instance
    {
        get
        {
            if (!instance) instance = FindObjectOfType<GameStageManager>();
            return instance;
        }
    }

    #endregion

    private enum GameStageState
    {
        READY,START,GAME,FINISH,END
    }
    
    [SerializeField] private Camera thirdPersonCamera;

    [SerializeField] private List<GameStageController> gameStages;

    private PlayableCharacter localCharacter;

    private Dictionary<string, PlayableCharacter> networkCharacters;

    private GameStageController curGameStage;

    [SerializeField] private TextMeshProUGUI debugText;

    [SerializeField] private Dictionary<string, CharacterCustomInfo> characterInfos;

    private GameStageState curState = GameStageState.READY;

    private float switchAttackTimerProgress = 0f;
    
    public string curStateStr = GameStageState.READY.ToString();
    
    private float serverStartTime = 0f;

    [SerializeField]
    private float switchAttackInterval = 30f;
    
    // 生きているキノコのカウント
    private int aliveKinokoCount = 0;

    // 生きているタケノコのカウント
    private int aliveTakenokoCount = 0;
    
    // ステージの大きさ
    private float stageSize = 0f;

    // 経過時間
    private float elapsedTime = 0f;

    // ゲームが開始しているか
    public bool isGamePlaying = false;

    private bool isSwitchAttack = false;

    private float switchProgress = 0f;
    
    // 初期化処理
    public void Init()
    {
        switchProgress = 0f;
        switchAttackTimerProgress = 0f;
        serverStartTime = PhotonNetwork.ServerTimestamp;
        thirdPersonCamera.gameObject.SetActive(false);
        isGamePlaying = false;
        isSwitchAttack = false;
        
        ChangeState(GameStageState.READY);
        networkCharacters = new Dictionary<string, PlayableCharacter>();
        characterInfos = new Dictionary<string, CharacterCustomInfo>();
    }

    // ゲーム開始
    public void GameStart()
    {
        isGamePlaying = true;
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        if (!PhotonNetwork.InRoom) return;
        

        string t = string.Empty;
        foreach (var chara in characterInfos)
        {
            // if (String.Equals(chara.Value.userId, localCharacter.userId)) t += "IsLocal:";
            // t += chara.Value.userId + ":" + chara.Value.hp + ":" + chara.Value.isDead + "\r\n";
        }

        t += curState.ToString();
        debugText.text = t;

        // 自分がクライアントの場合
        if (PhotonNetwork.IsMasterClient)
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                ChangeState(GameStageState.START);
            }
            
            UpdateState();
            UpdateHashtable();
        }
        
        // CheckDead();
    }

    // キャラクターの死亡をチェックする
    private void CheckDead()
    {
        foreach (var key in characterInfos.Keys)
        {
            if (!networkCharacters.ContainsKey(key)) continue;

            // オンライン状では死んでいるがローカルで死んでいない場合
            if (characterInfos[key].isDead && !networkCharacters[key].isDead)
            {
                Debug.Log("Dead:" + key);
                networkCharacters[key].gameObject.SetActive(false);
            }
        }
    }

    public void DeadCharacter(PlayableCharacter character, bool isLocal)
    {
        if (isLocal)
        {
            thirdPersonCamera.gameObject.SetActive(true);
        }
    }

    public void SetCharacter(PlayableCharacter character, bool isLocal = false)
    {
        if (isLocal)
        {
            localCharacter = character;
        }

        Debug.Log(character.userId);
        networkCharacters.Add(character.userId, character);
    }

    public override void OnJoinedRoom()
    {
        // characterInfos = new Dictionary<string, CharacterCustomInfo>();
    }


    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        try
        {
            characterInfos.ContainsKey(targetPlayer.UserId);
        }
        catch
        {
            characterInfos.Add(targetPlayer.UserId,new CharacterCustomInfo());
        }

        try
        {
            characterInfos[targetPlayer.UserId] = CharacterCustomInfo.HashTableToCharacterInfo(changedProps);
            characterInfos[targetPlayer.UserId].userId = targetPlayer.UserId;
        }
        catch
        { }
   
        if (networkCharacters.ContainsKey(targetPlayer.UserId))
        {
            networkCharacters[targetPlayer.UserId].SetcustomInfo(characterInfos[targetPlayer.UserId]);
        }
        
    }
    
    private void UpdateHashtable()
    {
        ExitGames.Client.Photon.Hashtable roomHash = new Hashtable();
        // roomHash["elapsedTime"] = elapsedTime;
        roomHash["isGamePlaying"] = isGamePlaying;
        roomHash["aliveKinokoCount"] = aliveKinokoCount;
        roomHash["aliveTakenokoCount"] = aliveTakenokoCount;
        roomHash["stageSize"] = stageSize;
        roomHash["stageState"] = curState.ToString();
        roomHash["switchAttackTimerProgress"] = switchAttackTimerProgress;
        Room room = PhotonNetwork.CurrentRoom;
        room.SetCustomProperties(roomHash);

    }
    
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (PhotonNetwork.IsMasterClient || !PhotonNetwork.IsConnected) return;
        // elapsedTime = (float) propertiesThatChanged["elapsedTime"];
        aliveKinokoCount = (int) propertiesThatChanged["aliveKinokoCount"];
        aliveTakenokoCount = (int) propertiesThatChanged["aliveTakenokoCount"];
        stageSize = (float) propertiesThatChanged["stageSize"];
        isGamePlaying = (bool) propertiesThatChanged["isGamePlaying"];
        stageSize = (float) propertiesThatChanged["stageSize"];
        ChangeState((string) propertiesThatChanged["stageState"]);
    }

    private void ChangeState(GameStageState state)
    {
        if (state == curState) return;
        curState = state;

        elapsedTime = 0f;
        switch (curState)
        {
            case GameStageState.READY:
                Init();
                break;
            case GameStageState.START:
                TeamState attackTeam 
                    = Random.Range(0, 2) == 1 ? TeamState.Kinoko : TeamState.Takenoko;
                foreach (var key in networkCharacters.Keys)
                {
                    bool isAttack = networkCharacters[key].team == attackTeam;
                    networkCharacters[key].GameStart();
                    networkCharacters[key].SwitchAttack(isAttack);
                }
                break;
            case GameStageState.GAME:
                break;
            case GameStageState.FINISH:
                break;
            case GameStageState.END:
                break;
        }        
    }

    private void ChangeState(string str)
    {
        Debug.Log("Change:"+str);
        switch (str)
        {
            case "READY":
                ChangeState(GameStageState.READY);
                break;
            case "START":
                ChangeState(GameStageState.START);
                break;
            case "GAME":
                ChangeState(GameStageState.GAME);
                break;
            case "FINISH":
                ChangeState(GameStageState.FINISH);
                break;
            case "END":
                ChangeState(GameStageState.END);
                break;
        }
    }
    
    private void UpdateState()
    {
        elapsedTime += Time.deltaTime;
        switch (curState)
        {
            case GameStageState.READY:
                break;
            case GameStageState.START:
                ChangeState(GameStageState.GAME);
                break;
            case GameStageState.GAME:
                switchProgress += Time.deltaTime;
                switchAttackTimerProgress = Mathf.InverseLerp(0f,switchAttackInterval,switchProgress);
                if (switchProgress >= switchAttackInterval)
                {
                    switchProgress -= switchAttackInterval;
                    foreach (var key in networkCharacters.Keys)
                    {
                        bool isAttack = networkCharacters[key].isAttack;
                        networkCharacters[key].SwitchAttack(!isAttack);
                    }
                }
                
                // タイマー更新
                foreach (var key in networkCharacters.Keys)
                {
                    networkCharacters[key].UpdateTimer(switchAttackTimerProgress);
                }
                
                /*
                if ((int) elapsedTime % (int) switchAttackInterval == 0 && !isSwitchAttack)
                {
                    foreach (var key in networkCharacters.Keys)
                    {
                        bool isAttack = networkCharacters[key].isAttack;
                        networkCharacters[key].SwitchAttack(!isAttack);
                    }
                    // switchAttackTimerProgress = 1f;
                    isSwitchAttack = true;
                }
                else
                {
                    if ((int) elapsedTime % (int) switchAttackInterval != 0)
                    {
                        // switchAttackTimerProgress = 0f;
                        isSwitchAttack = false;
                    }
                }
                /**/

                // 生きているキノコの人数を算出
                try
                {
                    aliveKinokoCount = 
                        networkCharacters.Values.ToList()
                            .FindAll((a) => { return a.team == TeamState.Kinoko && !a.isDead; }).Count;
                }
                catch
                {
                    aliveKinokoCount = networkCharacters.Values.Count;
                }
                
                // 生きているタケノコの人数を算出
                try
                {
                    aliveTakenokoCount = 
                        networkCharacters.Values.ToList()
                            .FindAll((a) => { return a.team == TeamState.Takenoko && !a.isDead; }).Count;
                }
                catch
                {
                    aliveTakenokoCount = networkCharacters.Values.Count;
                }

                if (aliveKinokoCount <= 0)
                {
                    // タケノコの勝利
                    // Debug.Log("TAKENOKO WIN !!");
                    // ChangeState(GameStageState.FINISH);
                }else if (aliveTakenokoCount <= 0)
                {
                    // キノコの勝利
                    // Debug.Log("KINOKO WIN !!");
                    // ChangeState(GameStageState.FINISH);
                }
                
                break;
            case GameStageState.FINISH:
                break;
            case GameStageState.END:
                break;
        }
    }


    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (info.Sender.IsLocal)
        {
            
        }
        else
        {
            
        }
    }

    
}
