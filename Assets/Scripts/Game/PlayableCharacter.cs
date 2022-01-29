using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterController))]
public class PlayableCharacter : MonoBehaviour
{
    
    [SerializeField]
    private BurningObject burningObject;

    public bool IsBurning
    {
        get => burningObject.isBurning;
    }

    // 焼かれてしまった
    public bool isBurned = false;
    
    // 焼かれた時間
    public float burnedTime = 0f;

    private IInputProvider inputProvider;
    
    private CharacterController characterController;
    
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
        UpdateBurning();
    }

    // 焼かれてる時の処理
    void UpdateBurning()
    {
        if (IsBurning && !isBurned)
        {
            if (burnedTime >= 5f)
            {
                isBurned = true;
                OnBurned();
            }
            burnedTime += Time.deltaTime;
        }
    }

    // 焼かれてしまったとき
    void OnBurned()
    {
        Debug.Log("On Burned !!");
    }
    
}
