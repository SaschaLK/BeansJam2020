using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public GameObject ProjectilesGroup;
    public static GameManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance == null)
        {
            GameManager.Instance = this;
        }

        if (AudioController.Instance != null)
        {
            AudioController.Instance.ChangeTheme(TrackThemes.Alive);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
