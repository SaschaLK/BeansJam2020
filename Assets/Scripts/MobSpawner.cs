using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour {

    public List<GameObject> mobs;

    public float spawnDelay;
    private float spawnDelayStart;
    public float spawnIncrement;

    private void Start() {
        spawnDelayStart = spawnDelay;
        StartCoroutine(SpawnMob());
    }

    private IEnumerator SpawnMob() {
        while (true)
        {
            if (MobManager.instance.spawning)
            {
                Instantiate(mobs[Random.Range(0, mobs.Count)], transform.position, Quaternion.identity, transform);
                MobManager.instance.spawnableMobs--;
            }
            yield return new WaitForSecondsRealtime(spawnDelay);
            if (spawnDelay - spawnIncrement > 0)
            {
                spawnDelay -= spawnIncrement;
            }
            if (MobManager.instance.spawnableMobs <= 0)
            {
                spawnDelay = spawnDelayStart;
                MobManager.instance.spawning = false;
            }
        }
    }
}
