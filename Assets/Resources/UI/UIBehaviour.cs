using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBehaviour<T> : MonoBehaviour where T : UIBehaviour<T>
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance) return instance;

            Debug.LogError("UIGroup��������܂���B");
            return null;
        }
    }



    protected virtual void Awake()
    {
        Debug.Log("�V���O���g�� > " + this.transform.name);
        instance = (T)this;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// <summary>
    /// ����Đ����\�b�h
    /// </summary>
    /// <param name="team">�`�[���̏��</param>
    public virtual void PlayMovie(TeamState team) { }
}
