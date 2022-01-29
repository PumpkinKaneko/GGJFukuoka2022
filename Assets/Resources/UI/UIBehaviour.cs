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

            Debug.LogError("UIGroupが見つかりません。");
            return null;
        }
    }



    protected virtual void Awake()
    {
        Debug.Log("シングルトン > " + this.transform.name);
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
    /// 動画再生メソッド
    /// </summary>
    /// <param name="team">チームの状態</param>
    public virtual void PlayMovie(TeamState team) { }
}
