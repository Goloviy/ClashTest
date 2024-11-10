using DigitalRuby.SoundManagerNamespace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : Singleton<SoundController>
{
    const string SOUND_VOLUME = "SOUND_VOLUME";
    const string MUSIC_VOLUME = "MUSIC_VOLUME";
    [Header("Sound References")]
    public AudioSource[] asSounds;
    public SOUND_TYPE[] soundTypes;
    public AudioClip[] soundClips;
    [Header("Music References")]
    public AudioSource[] asMusics;
    public MUSIC_BG[] musicTypes;
    public AudioClip[] musicClips;
    Dictionary<SOUND_TYPE, AudioClip> dictSound;
    Dictionary<MUSIC_BG, AudioClip> dictMusic;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        var volumeS = PlayerPrefs.GetFloat(SOUND_VOLUME, 1f);
        var volumeM = PlayerPrefs.GetFloat(MUSIC_VOLUME, 1f);
        SoundVolumeChanged(volumeS);
        MusicVolumeChanged(volumeM);

        dictSound = new Dictionary<SOUND_TYPE, AudioClip>();
        dictMusic = new Dictionary<MUSIC_BG, AudioClip>();
        for (int i = 0; i < soundTypes.Length; i++)
        {
            if (soundTypes.Length > i && soundClips.Length > i)
            {
                dictSound.Add(soundTypes[i], soundClips[i]);
            }
        }
        for (int i = 0; i < musicTypes.Length; i++)
        {
            if (musicTypes.Length > i && musicClips.Length > i)
            {
                dictMusic.Add(musicTypes[i], musicClips[i]);
            }
        }
    }
    private void Start()
    {


    }
    public void PlaySound(SOUND_TYPE type)
    {
        PlaySound(dictSound[type]);
    }
    public void PlaySound(AudioClip audioClip)
    {

        var source = GetAudioSource();
        source.PlayOneShot(audioClip);
    }
    private AudioSource GetAudioSource()
    {
        foreach (var source in asSounds)
        {
            if (!source.isPlaying)
                return source;
        }
        return asSounds[0];
    }
    private AudioSource GetAudioSourceBG()
    {
        //foreach (var source in asMusics)
        //{
        //    if (!source.isPlaying)
        //        return source;
        //}
        asMusics[0].Stop();
        asMusics[0].clip = null;
        return asMusics[0];
    }
    public void PlayMusic(AudioClip audioClip)
    {
        var source = GetAudioSourceBG();
        source.clip = audioClip;
        source.loop = true;
        source.Play();
    }
    public void PlayMusic(MUSIC_BG type)
    {
        PlayMusic(dictMusic[type]);
    }
    public void SoundVolumeChanged(float value)
    {
        foreach (var audioSource in asSounds)
        {
            audioSource.volume = value;
        }
        PlayerPrefs.SetFloat(SOUND_VOLUME, value);
    }

    public void MusicVolumeChanged(float value)
    {
        foreach (var audioSource in asMusics)
        {
            audioSource.volume = value;
        }
        PlayerPrefs.SetFloat(MUSIC_VOLUME, value);
    }
}
public enum SOUND_TYPE
{
    UI_BUTTON_CLICK,
    UI_BUTTON_CLICK2,
    UI_OPEN_LOADING,
    UI_OPEN_POPUP,
    UI_UPGRADE_SUCCESS,
    UI_FAIL,

    ALERT = 100,
    SOUND_PICKUP = 101,
    SOUND_LEVELUP = 102,
    SKILL_GRANDE_SHOOT = 103,
    SKILL_PISTOL_SHOOT = 104,
    SKILL_AWP = 105,
    SKILL_AWP_S = 106,
    SKILL_CLAW = 107,
    SKILL_CLAW_S = 108,
    SKILL_CROSSBOW = 109,
    SKILL_CROSSBOW_S = 110,
    SKILL_DRONE_LASER = 111,
    SKILL_KATANA = 112,
    SKILL_KATANA_S = 113,
    SKILL_QUAKE = 114,
    SKILL_SATELLITE = 115,
    SKILL_SATELLITE_S = 116,
    SKILL_SWORD = 117,
    SKILL_SWORD_S = 118,
}
public enum MUSIC_BG
{
    MUSIC_BG_1,
    MUSIC_BG_2
}