using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum TrackThemes {Menu, Alive, Dead};
public class AudioController : MonoBehaviour
{

    public static AudioController Instance;

    [SerializeField]
    private AudioMixer _AudioMixer_MainMix;

    [SerializeField]
    private AudioMixerSnapshot _AudioMixer_Menu;

    [SerializeField]
    private AudioMixerSnapshot _AudioMixer_Dead;

    [SerializeField]
    private AudioMixerSnapshot _AudioMixer_Alive;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(this);    
    }

    public void setMusicVol(float music_vol)
    {
        PlayerPrefs.SetFloat("MusicVol",music_vol);
        if (music_vol <= -20.0F)
            music_vol = -80.0F;
        _AudioMixer_MainMix.SetFloat("music_vol",music_vol);
    }

    public void setSFXVol(float sfx_vol)
    {
        PlayerPrefs.SetFloat("SFXVol", sfx_vol);
        if (sfx_vol <= -20.0F)
            sfx_vol = -80.0F;
        _AudioMixer_MainMix.SetFloat("sfx_vol", sfx_vol);
    }

    public void ChangeTheme(TrackThemes theme)
    {
        switch (theme)
        {
            case TrackThemes.Menu:
                _AudioMixer_Menu.TransitionTo(1.5F); 
                break;
            case TrackThemes.Dead:
                _AudioMixer_Dead.TransitionTo(1.5F);
                break;
            default:
                _AudioMixer_Alive.TransitionTo(1.5F);
                break;

        }
    }
}
