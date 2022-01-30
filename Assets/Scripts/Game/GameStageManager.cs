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
    public bool isGamePlaying = true;

    private bool isFinished = false;
    
    private string winTeam = "kinoko";
    
    private bool isSwitchAttack = false;

    private float switchProgress = 0f;
    
    // 初期化処理
    public void Init()
    {
        switchProgress = 0f;
        switchAttackTimerProgress = 0f;
        serverStartTime = PhotonNetwork.ServerTimestamp;
        thirdPersonCamera.gameObject.SetActive(false);
        isGamePlaying = true;
        isSwitchAttack = false;
        
        ChangeState(GameStageState.READY);
        networkCharacters = new Dictionary<string, PlayableCharacter>();
        characterInfos = new Dictionary<string, CharacterCustomInfo>();
    }

    // ゲーム開始
    public void GameStart()
    {
        isGamePlaying = true;
        ChangeState(GameStageState.START);
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
            // t += chara.Value.userId + ":" + chara.Value.hp + ":" + chara.Value.isBurning + "\r\n";
        }

        // t += curState.ToString();
        debugText.text = t;

        // 自分がクライアントの場合
        if (PhotonNetwork.IsMasterClient)
        {
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

        // Debug.Log(character.userId);
        if (character.userId != null) networkCharacters.Add(character.userId, character);
    }

    public void RemoveCharacter(PlayableCharacter character)
    {
        try
        {
            networkCharacters.Remove(character.userId);
        }catch {}
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
            if (targetPlayer.UserId != null)
                characterInfos.Add(targetPlayer.UserId,new CharacterCustomInfo());
        }
        
        try
        {
            characterInfos[targetPlayer.UserId] = CharacterCustomInfo.HashTableToCharacterInfo(changedProps);
            characterInfos[targetPlayer.UserId].userId = targetPlayer.UserId;
        }
        catch
        { }
   
        if (targetPlayer.UserId != null && networkCharacters.ContainsKey(targetPlayer.UserId))
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
        roomHash["isFinished"] = isFinished;
        roomHash["winTeam"] = winTeam;
        Room room = PhotonNetwork.CurrentRoom;
        room.SetCustomProperties(roomHash);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        try
        {
            characterInfos.Remove(otherPlayer.UserId);
        }
        catch
        {}
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
        isFinished = (bool) propertiesThatChanged["isFinished"];
        winTeam = (string) propertiesThatChanged["winTeam"];
        CommonValues.Instance.winnerTeam = String.Equals(winTeam, "kinoko") ? TeamState.Kinoko : TeamState.Takenoko;
        // Debug.Log(winTeam);
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
                
                // 初期化処理
                foreach (var key in networkCharacters.Keys)
                {
                    if (!networkCharacters[key]) networkCharacters[key].Init();
                }
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
                /*
                switch (winTeam)
                {
                    case "kinoko":
                        CommonValues.Instance.winnerTeam = TeamState.Kinoko;
                        break;
                    case "takenoko":
                        CommonValues.Instance.winnerTeam = TeamState.Takenoko;
                        break;
                }
                /**/
                ChangeState(GameStageState.END);
                break;
            case GameStageState.END:
                isGamePlaying = false;
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
                    Debug.Log("TAKENOKO WIN !!");
                    winTeam = "takenoko";
                    UpdateHashtable();
                    ChangeState(GameStageState.FINISH);
                    SetWinnerTeam(TeamState.Takenoko);
                    localCharacter.PlayResultBGM(TeamState.Takenoko);
                    // CommonValues.Instance.winnerTeam = TeamState.Takenoko;
                }else if (aliveTakenokoCount <= 0)
                {
                    // キノコの勝利
                    Debug.Log("KINOKO WIN !!");
                    winTeam = "kinoko";
                    UpdateHashtable();
                    ChangeState(GameStageState.FINISH);
                    SetWinnerTeam(TeamState.Kinoko);
                    // CommonValues.Instance.winnerTeam = TeamState.Kinoko;                    
                    localCharacter.PlayResultBGM(TeamState.Kinoko);
                }
                break;
            case GameStageState.FINISH:
                break;
            case GameStageState.END:
                break;
        }
    }

    public void SetWinnerTeam(TeamState team)
    {
        CommonValues.Instance.winnerTeam = team;
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
