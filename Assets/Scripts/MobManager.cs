using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MobManager : MonoBehaviour {

    public static MobManager instance;

    public Transform player;
    public bool isOverworld = true;
    public bool spawning;
    public int spawnableMobs;
    private int spawnableMobsMemory;
    public List<GameObject> mobs;
    public int kills;

    public bool respawning;

    public HealthBar healthbar;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        spawnableMobsMemory = spawnableMobs;
    }

    private void Update() {
        if (kills > 0 && mobs.Count == 0 && !respawning) {
            respawning = true;
            Debug.Log("Respawn");
            spawnableMobsMemory = spawnableMobsMemory * 2;
            spawnableMobs = spawnableMobsMemory;
            spawning = true;
        }
    }

    public void ChangeRealm() {
        isOverworld = !isOverworld;
        foreach(GameObject mob in mobs) {
            var path = mob.GetComponent<AIPath>();
            if (path != null)
            {
                if (!isOverworld && mob.tag == "Overworld")
                {
                    mob.GetComponent<AIPath>().canMove = false;
                }
                else if (isOverworld && mob.tag == "Overworld")
                {
                    mob.GetComponent<AIPath>().canMove = true;
                }
            }
        }

        MapGenerator.instance.ChangeRealm();
        healthbar.SetHealth(100);
        spawnableMobs = kills;
        spawning = true;

    }

    public void KillMob(GameObject mob)
    {
        mobs.Remove(mob);
    }
}
