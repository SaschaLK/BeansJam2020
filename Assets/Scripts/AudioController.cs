using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum TrackThemes {menu, alive, dead};
public class AudioController : MonoBehaviour
{

    public static AudioController Instance;

    [SerializeField]
    private AudioMixer mainMix;

    [SerializeField]
    private AudioMixerSnapshot menu;
    [SerializeField]
    private AudioMixerSnapshot dead;
    [SerializeField]
    private AudioMixerSnapshot alive;

    public void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);    
    }

    public void setMusicVol(float music_vol)
    {
        PlayerPrefs.SetFloat("MusicVol",music_vol);
        if (music_vol <= -20.0F)
            music_vol = -80.0F;
        mainMix.SetFloat("music_vol",music_vol);
    }

    public void setSFXVol(float sfx_vol)
    {
        PlayerPrefs.SetFloat("SFXVol", sfx_vol);
        if (sfx_vol <= -20.0F)
            sfx_vol = -80.0F;
        mainMix.SetFloat("sfx_vol", sfx_vol);
    }

    public void ChangeTheme(TrackThemes theme)
    {
        switch (theme)
        {
            case TrackThemes.menu:
                menu.TransitionTo(1.5F); 
                break;
            case TrackThemes.dead:
                dead.TransitionTo(1.5F);
                break;
            case TrackThemes.alive:
            default:
                alive.TransitionTo(1.5F);
                break;

        }
    }
}
