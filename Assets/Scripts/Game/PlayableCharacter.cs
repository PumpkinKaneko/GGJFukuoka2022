using System;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayableCharacter : MonoBehaviourPunCallbacks
{
    
    [SerializeField]
    private BurningObject burningObject;

    [SerializeField]
    private Camera playerCamera;

    [SerializeField]
    private GameObject lightObject;
    
    public bool isAttack = false;
    
    public int hp = 10;
    
    public bool isLocal = false;

    public bool isDead = false;

    public string userId;
    
    public bool IsBurning
    {
        get => burningObject.isBurning;
    }

    // 焼かれてしまった
    public bool isBurned = false;

    // 焼かれて死んでしまうまでの時間
    [SerializeField]
    public float burnedDeadTime = 10f;
    
    // 焼かれてる経過時間
    public float burnedTime = 0f;
    
    public TeamState team = TeamState.Kinoko;
    
    private IInputProvider inputProvider;
    
    private CharacterController characterController;

    private bool isInit = false;
    
    // 初期化処理
    public void Init()
    {
        burningObject.Init();
        burnedTime = 0f;
    }
    
    public void SetInputProvider(IInputProvider iProvider)
    {
        inputProvider = iProvider;
        characterController.InstallInputProvider(iProvider);
    }
    
    // ワールドポジションを設定
    public void SetWorldPosition(Vector3 pos)
    {
        transform.position = pos;
    }
    
    void Start()
    {

    }

    void Update()
    {
        if (PhotonNetwork.IsConnected)
        {
            if (!isInit)
            {
                isInit = true;
                userId = photonView.Controller.UserId;
                // このオブジェクトがローカルのものか
                if (photonView.IsMine)
                {
                    isLocal = true;
                    playerCamera.enabled = true;            
                    GameStageManager.Instance.SetCharacter(this,true);
                    // characterController.isStop = false;
                }
                else
                {
                    isLocal = false;
                    playerCamera.enabled = false;
                    GameStageManager.Instance.SetCharacter(this,false);
                    // コントローラーを無効化
                    // characterController.isStop = true;
                }

                // UpdateHashTable();
            }
        }
        
        if (isLocal && Input.GetKeyDown(KeyCode.Space))
        {
            hp -= 1;
            if (hp <= 0)
            {
                isDead = true;
                DeadCharacter();
            }
        }
        
        UpdateBurning();
        UpdateHashTable();
    }

    // 焼かれてる時の処理
    void UpdateBurning()
    {
        if (IsBurning && !isBurned)
        {
            if (burnedTime >= burnedDeadTime)
            {
                isBurned = true;
                OnBurned();
            }
            burnedTime += Time.deltaTime;
        }
    }

    void UpdateHashTable()
    {
        if (!isLocal) return;
        var hashtable = new ExitGames.Client.Photon.Hashtable();
        hashtable["name"] = gameObject.name;
        hashtable["hp"] = hp;
        hashtable["isDead"] = isDead;
        hashtable["isBurning"] = IsBurning;
        hashtable["isBurned"] = isBurned;
        hashtable["isAttack"] = isAttack;
        PhotonNetwork.LocalPlayer.SetCustomProperties(hashtable);
    }
    
    // 焼かれてしまったとき
    void OnBurned()
    {
        Debug.Log("On Burned !!");
        DeadCharacter();
    }

    // 死んだときの処理
    public void DeadCharacter()
    {
        Debug.Log(userId);
        isDead = true;
        UpdateHashTable();
        // 死んだときの処理
        if (isLocal)
        {
            gameObject.SetActive(false);
            playerCamera.gameObject.SetActive(false);
            GameStageManager.Instance.DeadCharacter(this,true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void SetcustomInfo(CharacterCustomInfo info)
    {
        if (isLocal) return;
        if (!isDead && info.isDead)
        {
            DeadCharacter();
        }

        isDead = info.isDead;
        hp = info.hp;
        isAttack = info.isAttack;
    }
    
    public void OnConnectedToServer()
    {

    }

    public override void OnJoinedRoom()
    {

    }

    public void SwitchAttack(bool isAttack)
    {
        photonView.RPC(nameof(SwitchAttackRPC),RpcTarget.All,isAttack);
    }
    
    [PunRPC]
    public void SwitchAttackRPC(bool isAttack)
    {
        this.isAttack = isAttack;
        lightObject.SetActive(this.isAttack);
    }

    
}
