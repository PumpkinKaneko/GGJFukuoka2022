using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class KeyboardInputProvider : MonoBehaviour,IInputProvider
{
    [SerializeField]
    public KeyCode moveFrontKey = KeyCode.W;

    [SerializeField]
    public KeyCode moveBackKey = KeyCode.S;

    [SerializeField]
    public KeyCode moveRightKey = KeyCode.D;

    [SerializeField]
    public KeyCode moveLeftKey = KeyCode.A;

    [SerializeField]
    public KeyCode actionAKey = KeyCode.Space;

    [SerializeField]
    public KeyCode actionBKey = KeyCode.LeftShift;

    [SerializeField]
    public KeyCode actionCKey = KeyCode.L;
    
    // Test
    [SerializeField]
    private CharacterController testPlayer;
    
    void Start()
    {
        testPlayer.InstallInputProvider(this);
    }

    void Update()
    {
        
    }

    public bool GetInputFront()
    {
        return Input.GetKey(moveFrontKey);
    }

    public bool GetInputBack()
    {
        return Input.GetKey(moveBackKey);
    }

    public bool GetInputRight()
    {
        return Input.GetKey(moveRightKey);
    }

    public bool GetInputLeft()
    {
        return Input.GetKey(moveLeftKey);
    }

    public bool GetInputActionA()
    {
        return Input.GetKey(actionAKey);
    }
    
    public bool GetInputActionB()
    {
        return Input.GetKey(actionBKey);
    }
    
    public bool GetInputActionC()
    {
        return Input.GetKey(actionCKey);
    }

    public float GetAxisX()
    {
        return Input.GetAxis("Mouse X");
    }

    public float GetAxisY()
    {
        return Input.GetAxis("Mouse Y");
    }
}
