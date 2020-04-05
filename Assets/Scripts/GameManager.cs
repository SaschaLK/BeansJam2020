using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public GameObject ProjectilesGroup;
    public static GameManager Instance;

    public GameObject HealthBar;

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


    internal void AddForceAtPosition(Vector2 position, int force, float radius)
    {
        var bodies = this.GetComponentsInChildren<Rigidbody2D>();

        for (var idx = 0; idx < bodies.Length; idx++)
        {
            var body = bodies[idx];
            var direction = (body.position - position);
            var intensity = Mathf.Max(0f, 1 - (direction.magnitude / radius));

            if (intensity == 0)
            {
                continue;
            }

            var baseFoce = direction.normalized * intensity * force;
            body.AddForceAtPosition(baseFoce, position, ForceMode2D.Impulse);
        }
    }
}
