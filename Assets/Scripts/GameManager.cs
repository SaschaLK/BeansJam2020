using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct FixedExplosion
{
    public int Force;
    public Vector2 Position;
    public float Radius;
}

public class GameManager : MonoBehaviour
{
    public GameObject ProjectilesGroup;
    public static GameManager Instance;

    public GameObject HealthBar;

    Queue<FixedExplosion> _FixedExplosions = new Queue<FixedExplosion>();

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

    private void FixedUpdate()
    {
        var isOverworld = true;
        if (MobManager.instance != null)
        {
            isOverworld = MobManager.instance.isOverworld;
        }

        var bodies = isOverworld ? GameObject.FindGameObjectsWithTag("Overworld") : GameObject.FindGameObjectsWithTag("Underworld");

        while (_FixedExplosions.Count > 0)
        {
            var fixedExplosion = _FixedExplosions.Dequeue();

            var position = fixedExplosion.Position;
            var radius = fixedExplosion.Radius;
            var force = fixedExplosion.Force;

            for (var idx = 0; idx < bodies.Length; idx++)
            {
                var body = bodies[idx];
                var direction = ((Vector2)(body.transform.position) - position);
                var intensity = Mathf.Max(0f, 1 - (direction.magnitude / radius));

                if (intensity == 0)
                {
                    continue;
                }

                var mob = body.GetComponent<Mob>();
                if (mob != null)
                {
                    mob.IsDying = true;
                }

                var baseForce = direction.normalized * intensity * force;

                var rigidBody = body.GetComponent<Rigidbody2D>();
                //rigidBody.AddForceAtPosition(baseForce, position, ForceMode2D.Force);
                rigidBody.AddForce(baseForce);
            }
        }
    }

    internal void AddForceAtPosition(Vector2 position, int force, float radius)
    {
        _FixedExplosions.Enqueue(new FixedExplosion { Position = position, Force = force, Radius = radius });

        //var bodies = this.GetComponentsInChildren<Rigidbody2D>();
        
    }
}
