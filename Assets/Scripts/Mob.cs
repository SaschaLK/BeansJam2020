using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Mob : MonoBehaviour {
    private void Start() {
        if (MobManager.instance != null && MobManager.instance.player != null) {
            GetComponent<AIDestinationSetter>().target = MobManager.instance.player;
            MobManager.instance.mobs.Add(gameObject);
        }
    }

    public void Killed() {
        MobManager.instance.kills++;
    }

}
