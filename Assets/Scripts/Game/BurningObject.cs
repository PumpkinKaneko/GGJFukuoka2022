using UnityEngine;

public class BurningObject : MonoBehaviour
{
    public bool isBurning = false;

    [SerializeField]
    public string targetTag = "BurningLight";

    public void Init()
    {
        isBurning = false;
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
