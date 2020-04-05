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
    public List<GameObject> mobs;
    public int kills;

    private void Awake() {
        instance = this;
    }

    private void Update() {
        if (Input.GetButtonDown("Jump")) {
            ChangeRealm();
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
    }

    public void KillMob(GameObject mob)
    {
        mobs.Remove(mob);
    }
}
