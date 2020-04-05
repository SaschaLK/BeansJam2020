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
        while (true) {
            MobManager.instance.respawning = false;
            if (MobManager.instance.spawning && MobManager.instance.isOverworld) {
                Instantiate(mobs[0], transform.position, Quaternion.identity, transform);
                MobManager.instance.spawnableMobs--;
            }
            else if(MobManager.instance.spawning && !MobManager.instance.isOverworld) {
                Instantiate(mobs[1], transform.position, Quaternion.identity, transform);
                MobManager.instance.spawnableMobs--;
            }
            yield return new WaitForSecondsRealtime(spawnDelay);
            if (spawnDelay - spawnIncrement > 0) {
                spawnDelay -= spawnIncrement;
            }
            if (MobManager.instance.spawnableMobs <= 0) {
                spawnDelay = spawnDelayStart;
                MobManager.instance.spawning = false;
            }
        }
    }
}
