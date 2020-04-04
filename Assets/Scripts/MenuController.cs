using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider sfxSlider;
    [SerializeField]
    private Dropdown anchorDropdown;

    public void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVol", -5.0F);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVol", -5.0F);
        anchorDropdown.value = PlayerPrefs.GetInt("MovementPreference");
    }

    public void LoadNextLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetMovementPreference(int pref)
    {
        PlayerPrefs.SetInt("MovementPreference", pref);
    }
}
