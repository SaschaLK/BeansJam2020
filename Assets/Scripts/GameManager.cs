using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
