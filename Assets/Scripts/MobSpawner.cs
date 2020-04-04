﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour {

    public List<GameObject> mobs;

    public float spawnDelay;

    private void Start() {
        StartCoroutine(SpawnMob());
    }

    private IEnumerator SpawnMob() {
        Instantiate(mobs[Random.Range(0, mobs.Count)], transform.position, Quaternion.identity, transform);
        yield return new WaitForSecondsRealtime(spawnDelay);
    }
}