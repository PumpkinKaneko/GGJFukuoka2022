using System;
using UnityEngine;

public class BurningObject : MonoBehaviour
{
    public bool isBurning = false;

    [SerializeField]
    public string targetTag = "BurningLight";

    private float elapsedTime = 0f;
    
    private bool isTriggerStay = false;
    
    public void Init()
    {
        isBurning = false;
    }

    void Update()
    {
        if (isTriggerStay)
        {
            if (elapsedTime >= 0.3f)
            {
                isTriggerStay = false;
                isBurning = false;
                elapsedTime = 0f;
            }
            elapsedTime += Time.deltaTime;
        }
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == targetTag)
        {
            isBurning = true;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == targetTag)
        {
            isBurning = true;
            isTriggerStay = true;
            elapsedTime = 0f;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == targetTag)
        {
            isBurning = false;
        }
    }
}
