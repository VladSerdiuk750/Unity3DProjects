using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Timeline;


[System.Serializable]
public class Audio
{
    #region Private_Variables

    private AudioSource sourceSFX;

    private AudioSource sourceMusic;

    private AudioSource sourceRandomPitchSFX;

    private float musicVolume = 1f;

    private float sfxVolume = 1f;

    [SerializeField] private AudioClip[] sounds;

    [SerializeField] private AudioClip defaultClip;

    [SerializeField] private AudioClip menuMusic;

    [SerializeField] private AudioClip gameMusic;

    #endregion

    public float MusicVolume
    {
        get { return musicVolume; }
        set
        {
            musicVolume = value;
            SourceMusic.volume = musicVolume;
            DataStore.SaveOptions();
        } 
    }

    public float SfxVolume
    {
        get => sfxVolume;
        set
        {
            sfxVolume = value;
            SourceSfx.volume = sfxVolume;
            SourceRandomPitchSfx.volume = sfxVolume;    
            DataStore.SaveOptions();
        }
    }

    public AudioSource SourceSfx
    {
        get => sourceSFX;
        set => sourceSFX = value;
    }

    public AudioSource SourceMusic
    {
        get => sourceMusic;
        set => sourceMusic = value;
    }

    public AudioSource SourceRandomPitchSfx
    {
        get => sourceRandomPitchSFX;
        set => sourceRandomPitchSFX = value;
    }

    /// <summary>
    /// Search sound in array
    /// </summary>
    /// <params name = "clipName"> Name of Sound </params>
    /// <returns>Sound. If sound isn't found, return value of variable defaultClip</returns>
    private AudioClip GetSound(string clipName)
    {
        foreach (var sound in sounds)
        {
            if (sound.name == clipName)
            {
                return sound;
            }
        }

        Debug.LogError("Can not find clip" + clipName);
        return defaultClip;
    }

    
    /// <summary>
    /// Play sounds from array
    /// </summary>
    /// <param name="clipName">Name of sound</param>
    public void PlaySound(string clipName)
    {
        sourceSFX.PlayOneShot(GetSound(clipName), SfxVolume);
    }

    /// <summary>
    /// Play sound from array with random pitch
    /// </summary>
    /// <param name="clipName">Name of sound</param>
    public void PlaySoundRandomPitch(string clipName)
    {
        SourceRandomPitchSfx.pitch = Random.Range(0.7f, 1.3f);
        SourceRandomPitchSfx.PlayOneShot(GetSound(clipName), SfxVolume);
    }

    /// <summary>
    /// Play music
    /// </summary>
    /// <param name="menu">is it for main menu?</param>
    public void PlayMusic(bool menu)
    {
        if (menu)
        {
            SourceMusic.clip = menuMusic;
        }
        else
        {
            SourceMusic.clip = gameMusic;
        }

        SourceMusic.volume = MusicVolume;
        SourceMusic.loop = true;
        SourceMusic.Play();
    }
}
