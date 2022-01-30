using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CharacterController : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private Camera playerCamera;
    
    private IInputProvider inputProvider;

    public bool isStop = false;
    
    public void InstallInputProvider(IInputProvider provider)
    {
        inputProvider = provider;
    }
    
    // 移動速度
    [SerializeField]
    private float moveSpeed = 1f;

    // 減速値
    [SerializeField,Range(0f,0.999f)]
    private float moveDecelerateFactor = 0.7f;
    
    // 横方向の感度
    [SerializeField]
    private float xRotateSens = 5f;

    // 縦方向の感度
    [SerializeField]
    private float yRotateSens = 5f;

    private Rigidbody rBody;
    
    void Start()
    {
        if (photonView.IsMine) isStop = false;
        else isStop = true;
        
        rBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (isStop) return;
        
        UpdateCursor();

        UpdateInput();
        
        UpdateRotate();
    }
    
    // 入力を監視
    private void UpdateInput()
    {
        Vector3 direction = Vector3.zero;
        if (inputProvider.GetInputFront())
        {
            direction += transform.forward;
        }
        
        if (inputProvider.GetInputBack())
        {
            direction -= transform.forward;
        }

        if (inputProvider.GetInputRight())
        {
            direction += transform.right;
        }

        if (inputProvider.GetInputLeft())
        {
            direction -= transform.right;
        }

        rBody.AddForce(direction * moveSpeed);
        
        if (inputProvider.GetInputActionA())
        {
            OnActionA();
        }
        
        if (inputProvider.GetInputActionB())
        {
            OnActionB();
        }

        if (inputProvider.GetInputActionC())
        {
            OnActionC();
        }

        rBody.velocity *= moveDecelerateFactor;
    }
    
    public void OnActionA()
    {
        
    }

    public void OnActionB()
    {
        
    }

    public void OnActionC()
    {
        
    }
    
    // 回転の更新
    private void UpdateRotate()
    {
        float xAxis = inputProvider.GetAxisX();
        float yAxis = inputProvider.GetAxisY();

        Vector3 newRot = new Vector3(-yAxis*yRotateSens,0,0);
        playerCamera.transform.Rotate(newRot);

        // 横移動
        float xAngle = playerCamera.transform.localEulerAngles.x;
        if (xAngle > 180)
        {
            xAngle = 360f - xAngle;
            if (xAngle >= 30f)
            {
                playerCamera.transform.localEulerAngles = new Vector3(-30f,0f,0f);
            }
        }
        else
        {
            if (xAngle >= 30f)
            {
                playerCamera.transform.localEulerAngles = new Vector3(30f,0f,0f);
            }
        }
        newRot = new Vector3(0f,xAxis*xRotateSens,0f);
        transform.Rotate(newRot,Space.World);

    }

    // TODO : 全体を管理するクラスで実装
    private void UpdateCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
