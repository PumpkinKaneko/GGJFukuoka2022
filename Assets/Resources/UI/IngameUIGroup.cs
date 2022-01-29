using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameUIGroup : UIBehaviour<IngameUIGroup>
{
    [SerializeField]private TimerUI timer;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlayTimer(float time)
    {
        timer.PlayTimer(time);
    }
}
