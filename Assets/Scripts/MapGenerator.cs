using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MapGenerator : MonoBehaviour {

    public static MapGenerator instance;

    private void Awake() {
        instance = this;
    }

    public int worldBuffer;
    public int width;
    public int height;

    [Range(0, 1)] public float thresholdObstacle;
    [Range(0, 1)] public float thresholdSpawner;

    public GameObject waterTile;
    public List<GameObject> wallTilesNESW = new List<GameObject>();
    public List<GameObject> floorTiles = new List<GameObject>();
    private List<GameObject> instantiatedFloorTiles = new List<GameObject>();
    public List<Sprite> floorTiles2 = new List<Sprite>();
    public List<GameObject> obstacles = new List<GameObject>();

    public GameObject spawner;
    public GameObject torch;
    [Range(0,1)] public float lightThreshold;
    public GameObject player;

    private Dictionary<Vector2, bool> obstacleMap = new Dictionary<Vector2, bool>();
    private Dictionary<Vector2, int> obstacleIDS = new Dictionary<Vector2, int>();

    private void Start() {
        GenerateWater();
        GenerateWalls();
        GenerateFloor();
        GenerateDictionary();
        GenerateObstacleMap();
        PlaceObstacles();
        GenerateSpawners();
        GenerateLights();

        AstarPath.active.Scan();
    }

    private void GenerateWater() {
        waterTile.GetComponent<SpriteRenderer>().size = new Vector2(worldBuffer+width, worldBuffer+height);
        waterTile.transform.position = new Vector2(width / 2, height / 2);
    }

    private void GenerateWalls() {
        for (int x = -1; x <= width + 1; x++) {
            for (int y = -1; y <= height + 1; y++) {
                int temp = Random.Range(0, 2);
                //Northern walls
                if (x != -1 && x != width + 1 && y == height + 1) {
                    Instantiate(wallTilesNESW[0+temp], new Vector3(x, y), Quaternion.identity, transform);
                }
                //NorthEastern Wall
                else if (x == width + 1 && y == height + 1) {
                    Instantiate(wallTilesNESW[2], new Vector3(x, y), Quaternion.identity, transform);
                }
                //Eastern Wall
                else if (x == width + 1 && y != height + 1 && y != -1) {
                    Instantiate(wallTilesNESW[3+temp], new Vector3(x, y), Quaternion.identity, transform);
                }
                //SouthEastern Wall
                else if (x == width + 1 && y == -1) {
                    Instantiate(wallTilesNESW[5], new Vector3(x, y), Quaternion.identity, transform);
                }
                //Southern Wall
                else if (x != -1 && x != width + 1 && y == -1) {
                    Instantiate(wallTilesNESW[6+temp], new Vector3(x, y), Quaternion.identity, transform);
                }
                //SouthWestern Wall
                else if (x == -1 && y == -1) {
                    Instantiate(wallTilesNESW[8], new Vector3(x, y), Quaternion.identity, transform);
                }
                //Western Wall
                else if (x == -1 && y != -1 && y != height + 1) {
                    Instantiate(wallTilesNESW[9+temp], new Vector3(x, y), Quaternion.identity, transform);
                }
                //NorthWestern Wall
                else if (x == -1 && y == height + 1) {
                    Instantiate(wallTilesNESW[11], new Vector3(x, y), Quaternion.identity, transform);
                }
            }
        }
    }

    private void GenerateFloor() {
        for (int x = 0; x <= width; x++) {
            for (int y = 0; y <= height; y++) {
                int selection = Random.Range(0, floorTiles.Count);
                instantiatedFloorTiles.Add(Instantiate(floorTiles[selection], new Vector3(x, y), Quaternion.identity, transform));
            }
        }
    }

    private void GenerateDictionary() {
        for (int x = 0; x <= width; x++) {
            for (int y = 0; y <= height; y++) {
                obstacleMap.Add(new Vector2(x, y), false);
            }
        }
    }

    private void GenerateObstacleMap() {
        float seed = Random.Range(0f, 1f);
        for (int x = 0; x <= width; x++) {
            for (int y = 0; y <= height; y++) {
                if (Mathf.PerlinNoise(x * 0.1f + seed, y * 0.1f + seed) > thresholdObstacle) {
                    obstacleMap[new Vector2(x, y)] = true;
                    obstacleIDS.Add(new Vector2(x, y), 0);
                }
            }
        }
    }

    private void PlaceObstacles() {
        foreach (KeyValuePair<Vector2, bool> obstacle in obstacleMap) {
            Vector2 tempNorth = obstacle.Key + Vector2.up;
            Vector2 tempEast = obstacle.Key + Vector2.right;
            Vector2 tempSouth = obstacle.Key + Vector2.down;
            Vector2 tempWest = obstacle.Key + Vector2.left;

            if(obstacleMap.ContainsKey(tempNorth) && obstacleMap[tempNorth]) {
                if (obstacleIDS.ContainsKey(obstacle.Key)) {
                    obstacleIDS[obstacle.Key] += 1;
                }
            }
            if(obstacleMap.ContainsKey(tempEast) && obstacleMap[tempEast]) {
                if (obstacleIDS.ContainsKey(obstacle.Key)) {
                    obstacleIDS[obstacle.Key] += 2;
                }
            }
            if(obstacleMap.ContainsKey(tempSouth) && obstacleMap[tempSouth]) {
                if (obstacleIDS.ContainsKey(obstacle.Key)) {
                    obstacleIDS[obstacle.Key] += 4;
                }
            }
            if(obstacleMap.ContainsKey(tempWest) && obstacleMap[tempWest]) {
                if (obstacleIDS.ContainsKey(obstacle.Key)) {
                    obstacleIDS[obstacle.Key] += 8;
                    }
            }

            if (obstacleIDS.ContainsKey(obstacle.Key)) {
                switch (obstacleIDS[obstacle.Key]) {
                    case 0:
                        Instantiate(obstacles[0], obstacle.Key, Quaternion.identity, transform);
                        break;
                    case 1:
                        Instantiate(obstacles[1], obstacle.Key, Quaternion.identity, transform);
                        break;
                    case 2:
                        Instantiate(obstacles[2], obstacle.Key, Quaternion.identity, transform);
                        break;
                    case 3:
                        Instantiate(obstacles[3], obstacle.Key, Quaternion.identity, transform);
                        break;
                    case 4:
                        Instantiate(obstacles[4], obstacle.Key, Quaternion.identity, transform);
                        break;
                    case 5:
                        Instantiate(obstacles[5], obstacle.Key, Quaternion.identity, transform);
                        break;
                    case 6:
                        Instantiate(obstacles[6], obstacle.Key, Quaternion.identity, transform);
                        break;
                    case 7:
                        Instantiate(obstacles[7], obstacle.Key, Quaternion.identity, transform);
                        break;
                    case 8:
                        Instantiate(obstacles[8], obstacle.Key, Quaternion.identity, transform);
                        break;
                    case 9:
                        Instantiate(obstacles[9], obstacle.Key, Quaternion.identity, transform);
                        break;
                    case 10:
                        Instantiate(obstacles[10], obstacle.Key, Quaternion.identity, transform);
                        break;
                    case 11:
                        Instantiate(obstacles[11], obstacle.Key, Quaternion.identity, transform);
                        break;
                    case 12:
                        Instantiate(obstacles[12], obstacle.Key, Quaternion.identity, transform);
                        break;
                    case 13:
                        Instantiate(obstacles[13], obstacle.Key, Quaternion.identity, transform);
                        break;
                    case 14:
                        Instantiate(obstacles[14], obstacle.Key, Quaternion.identity, transform);
                        break;
                    case 15:
                        Instantiate(obstacles[15], obstacle.Key, Quaternion.identity, transform);
                        break;
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

    private void GenerateLights() {
        List<Vector2> possibleLights = new List<Vector2>();

        foreach(KeyValuePair<Vector2, bool> tile in obstacleMap) {
            if (!tile.Value) {
                Vector2 tempNorth = tile.Key + Vector2.up;
                Vector2 tempEast = tile.Key + Vector2.right;
                Vector2 tempSouth = tile.Key + Vector2.down;
                Vector2 tempWest = tile.Key + Vector2.left;
                if (obstacleMap.ContainsKey(tempNorth) && obstacleMap[tempNorth]) {
                    possibleLights.Add(tile.Key);
                }
                else if(obstacleMap.ContainsKey(tempEast) && obstacleMap[tempEast]) {
                    possibleLights.Add(tile.Key);
                }
                else if(obstacleMap.ContainsKey(tempSouth) && obstacleMap[tempSouth]) {
                    possibleLights.Add(tile.Key);
                }
                else if(obstacleMap.ContainsKey(tempWest) && obstacleMap[tempWest]) {
                    possibleLights.Add(tile.Key);
                }
            }
        }

        foreach(Vector2 possibleLight in possibleLights) {
            float tempChance = Random.Range(0f, 1f);
            if (tempChance > lightThreshold) {
                //Debug.Log("int light");
                Instantiate(torch, possibleLight, Quaternion.identity, transform);
            }
        }
    }

    public void ChangeRealm() {
        foreach (GameObject floorTile in instantiatedFloorTiles) {
            floorTile.GetComponent<SpriteRenderer>().sprite = floorTiles2[Random.Range(0, floorTiles2.Count)];
        }
    }
}
