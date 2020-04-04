using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MapGenerator : MonoBehaviour {

    public int width;
    public int height;

    [Range(0, 1)] public float thresholdObstacle;
    [Range(0, 1)] public float thresholdSpawner;

    public List<GameObject> wallTilesNESW = new List<GameObject>();
    public List<GameObject> floorTiles = new List<GameObject>();
    public List<GameObject> obstacles = new List<GameObject>();
    public GameObject spawner;
    public GameObject player;

    private Dictionary<Vector2, bool> obstacleMap = new Dictionary<Vector2, bool>();

    private void Start() {
        GenerateWalls();
        GenerateFloor();
        GenerateEmptyMap();
        GenerateObstacles();
        GenerateSpawners();

        AstarPath.active.Scan();
    }

    private void GenerateWalls() {
        for (int x = -1; x <= width + 1; x++) {
            for (int y = -1; y <= height + 1; y++) {
                //Northern walls
                if (x != -1 && x != width + 1 && y == height + 1) {
                    Instantiate(wallTilesNESW[0], new Vector3(x, y), Quaternion.identity, transform);
                }
                //NorthEastern Wall
                else if (x == width + 1 && y == height + 1) {
                    Instantiate(wallTilesNESW[1], new Vector3(x, y), Quaternion.identity, transform);
                }
                //Eastern Wall
                else if (x == width + 1 && y != height + 1 && y != -1) {
                    Instantiate(wallTilesNESW[2], new Vector3(x, y), Quaternion.identity, transform);
                }
                //SouthEastern Wall
                else if (x == width + 1 && y == -1) {
                    Instantiate(wallTilesNESW[3], new Vector3(x, y), Quaternion.identity, transform);
                }
                //Southern Wall
                else if (x != -1 && x != width + 1 && y == -1) {
                    Instantiate(wallTilesNESW[4], new Vector3(x, y), Quaternion.identity, transform);
                }
                //SouthWestern Wall
                else if (x == -1 && y == -1) {
                    Instantiate(wallTilesNESW[5], new Vector3(x, y), Quaternion.identity, transform);
                }
                //Western Wall
                else if (x == -1 && y != -1 && y != height + 1) {
                    Instantiate(wallTilesNESW[6], new Vector3(x, y), Quaternion.identity, transform);
                }
                //NorthWestern Wall
                else if (x == -1 && y == height + 1) {
                    Instantiate(wallTilesNESW[7], new Vector3(x, y), Quaternion.identity, transform);
                }
            }
        }
    }

    private void GenerateFloor() {
        for (int x = 0; x <= width; x++) {
            for (int y = 0; y <= height; y++) {
                int selection = Random.Range(0, floorTiles.Count);
                Instantiate(floorTiles[selection], new Vector3(x, y), Quaternion.identity, transform);
            }
        }
    }

    private void GenerateEmptyMap() {
        for (int x = 0; x <= width; x++) {
            for (int y = 0; y <= height; y++) {
                obstacleMap.Add(new Vector2(x, y), false);
            }
        }
    }

    private void GenerateObstacles() {
        float seed = Random.Range(0f, 1f);
        for (int x = 0; x <= width; x++) {
            for (int y = 0; y <= height; y++) {
                if (Mathf.PerlinNoise(x * 0.1f + seed, y * 0.1f + seed) > thresholdObstacle) {
                    obstacleMap[new Vector2(x, y)] = true;
                    Instantiate(obstacles[0], new Vector3(x, y), Quaternion.identity, transform);
                }
            }
        }
    }

    private void GenerateSpawners() {
        List<Vector2> openTiles = new List<Vector2>();

        foreach (KeyValuePair<Vector2, bool> tile in obstacleMap) {
            if (!tile.Value) {
                if (Random.Range(0f, 1f) > thresholdSpawner) {
                    Instantiate(spawner, new Vector3(tile.Key.x, tile.Key.y), Quaternion.identity, transform);
                }
                else {
                    openTiles.Add(tile.Key);
                }
            }
        }

        int selectVector = Random.Range(0, openTiles.Count);
        Vector2 playerSpawn = openTiles[selectVector];
        player.transform.position = playerSpawn;
    }
}
