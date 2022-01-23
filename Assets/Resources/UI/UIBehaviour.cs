using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBehaviour : MonoBehaviour
{
    private static UIBehaviour instance;

    public static UIBehaviour Instance
    {
        get
        {
            if (instance) return instance;

            Debug.LogError("UIGroupが見つかりません。");
            return null;
        }
    }



    protected virtual void Awake()
    {
        Debug.Log("シングルトン > " + this.transform.name);
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
