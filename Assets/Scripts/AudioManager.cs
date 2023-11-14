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

    public AudioClip[] sfxClip;
    public float sfxVolume;
    public int sfxChannels;
    public AudioSource[] sfxSource;

    int channelIndex;

    public enum Bgm
    {
        LobbyBGM,
        TutorialBGM,
    }

    public enum Sfx
    {
        Booster,
        BrakingSound,
        ButtonSelect,
        CountDown,
        FootSound,
        FstJump,
        GameOverVoice=7,
        Hurdle,
        KnockBack,
        MapSelect,
        OuchVoice,
        SndJump,
        StageStart=14,
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
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "main")
        {
            PlayBgm(AudioManager.Bgm.TutorialBGM);
        }
        else
        {
            PlayBgm(AudioManager.Bgm.LobbyBGM);
        }
    }

    void Init()
    {
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmSource = bgmObject.AddComponent<AudioSource>();
        bgmSource.playOnAwake = false; 
        bgmSource.loop = true; 
        bgmSource.volume = bgmVolume;

        GameObject sfxObject = new GameObject("sfxSource");
        sfxObject.transform.parent = transform;
        sfxSource = new AudioSource[sfxChannels];

        for(int index=0; index<sfxSource.Length; index++)
        {
            sfxSource[index] = sfxObject.AddComponent<AudioSource>();
            sfxSource[index].playOnAwake = false;
            sfxSource[index].volume = sfxVolume;
        }
    }

    public void PlayBgm(Bgm bgm)
    {
        bgmSource.clip = bgmClip[(int)bgm];
        bgmSource.Play();
    }

    public void PlaySfx(Sfx sfx)
    {
        for(int index = 0; index < sfxSource.Length; index++)
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

    public void BgmVolume()
    {

    }

    public void SFXVolume()
    {

    }

}
