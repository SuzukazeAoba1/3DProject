using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    public AudioClip[] bgmClip;
    public float bgmVolume;
    public AudioSource bgmSource;
    AudioHighPassFilter bgmEffect;

    public AudioClip[] sfxClip;
    public float sfxVolume;
    public int sfxChannels;
    public AudioSource[] sfxSource;

    private Dictionary<Sfx, float> lastPlayTimeDictionary = new Dictionary<Sfx, float>();
    public float defaultCooldown = 0.5f;

    int channelIndex;

    public enum Sfx
    {
        Booster,
        BrakingSound,
        ButtonSelect,
        CountDown,
        FootSound,
        FstJump,
        FstJump2,
        GameOverVoice,
        Hurdle,
        KnockBack,
        MapSelect,
        OuchVoice,
        SndJump,
        SndJump2,
        StageStart,
    }

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Init();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        
    }

    private void Start()
    {
        PlayBgm(true, 1);
    }

    void initVolume()
    {
        bgmSource.volume = bgmVolume;
        for (int index = 0; index < sfxSource.Length; index++)
        {
            sfxSource[index].volume = sfxVolume;
        }
    }

    void Init()
    {
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmSource = bgmObject.AddComponent<AudioSource>();
        bgmEffect = Camera.main.GetComponent<AudioHighPassFilter>();
        bgmSource.playOnAwake = false; 
        bgmSource.loop = true; 
        

        GameObject sfxObject = new GameObject("sfxSource");
        sfxObject.transform.parent = transform;
        sfxSource = new AudioSource[sfxChannels];

        for(int index=0; index<sfxSource.Length; index++)
        {
            sfxSource[index] = sfxObject.AddComponent<AudioSource>();
            sfxSource[index].playOnAwake = false;
            sfxSource[index].bypassListenerEffects = true;
        }

        initVolume();
    }

    public void PlayBgm(bool isPlay, int bgmIndex = 1)
    {
        if (isPlay)
        {
            if (bgmSource != null)  // null 체크 추가
            {
                if (bgmIndex == 1)
                    bgmSource.clip = bgmClip[0];
                else if (bgmIndex == 2)
                    bgmSource.clip = bgmClip[1];

                bgmSource.Play();
            }
        }
        else
        {
            if (bgmSource != null)  
            {
                bgmSource.Stop();
            }
        }
    }

    public void PlaySfx(Sfx sfx)
    {
        for (int index = 0; index < sfxSource.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxSource.Length;

            if (sfxSource[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;
            sfxSource[loopIndex].clip = sfxClip[(int)sfx];
            sfxSource[loopIndex].Play();

            break;
        }
    }

    public void SFXVolume(float volume)
    {
        sfxVolume = volume;
        for (int index = 0; index < sfxSource.Length; index++)
        {
            sfxSource[index].volume = sfxVolume;
        }
    }

    public void BGMVolume(float volume)
    {
        bgmVolume = volume;
        bgmSource.volume = bgmVolume;
    }

}
