using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BurningObject))]
public class ObjectActivatorForBurning : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private float burningTime = 2f;

    private float elapsedTime = 0f;

    private bool isActive = false;
    
    private BurningObject burningObject;
    
    // Start is called before the first frame update
    void Start()
    {
        burningObject = GetComponent<BurningObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (burningObject.isBurning)
        {
            if (elapsedTime >= burningTime)
            {
                isActive = true;
            }
            elapsedTime += Time.deltaTime;
        }
        else
        {
            isActive = false;
            elapsedTime = 0f;
        }
        target.SetActive(isActive);
    }
}
