using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if(instance != null)
            {
                return instance;
            }

            return null;
        }
    }


    [SerializeField] private AudioClip[] clips;
    private AudioSource audio;


    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);

        audio = this.GetComponent<AudioSource>();
    }


    // Start is called before the first frame update
    void Start()
    {
        //SoundManager.Instance.PlayBGM(AudioState.BGM_1);
    }


    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlayBGM(AudioState state)
    {
        audio.clip = clips[(int)state];
        audio.loop = true;
        audio.Play();
    }


    public void PlaySE(AudioState state)
    {
        audio.loop = false;
        audio.PlayOneShot(clips[(int)state]);
    }
}


public enum AudioState
{
    BGM_1 = 0,
    BGM_2,
    BGM_3,
    BGM_4,
    BGM_5,
    DEFENCE,
    MELTING
}
