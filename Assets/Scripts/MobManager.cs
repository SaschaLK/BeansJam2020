using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobManager : MonoBehaviour {

    public static MobManager instance;

    public Transform player;
    public bool spawning;
    public int spawnableMobs;

    private void Awake() {
        instance = this;
    }

}
