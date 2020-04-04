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
    public List<GameObject> obstacles1 = new List<GameObject>();
    public List<GameObject> obstacles2 = new List<GameObject>();
    public List<GameObject> obstacles3 = new List<GameObject>();
    public List<GameObject> obstacles4 = new List<GameObject>();
    public List<GameObject> obstacles5 = new List<GameObject>();
    public List<GameObject> obstacles6 = new List<GameObject>();
    public List<GameObject> obstacles7 = new List<GameObject>();
    public List<GameObject> obstacles8 = new List<GameObject>();
    public List<GameObject> obstacles9 = new List<GameObject>();
    public List<GameObject> obstacles10 = new List<GameObject>();
    public List<GameObject> obstacles11 = new List<GameObject>();
    public List<GameObject> obstacles12 = new List<GameObject>();
    public GameObject spawner;
    public GameObject torch;
    [Range(0,1)] public float lightThreshold;
    public GameObject player;

    private Dictionary<Vector2, bool> obstacleMap = new Dictionary<Vector2, bool>();

    private void Start() {
        GenerateWalls();
        GenerateFloor();
        GenerateDictionary();
        GenerateObstacleMap();
        PlaceObstacles();
        GenerateSpawners();
        GenerateLights();

        AstarPath.active.Scan();
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
                Instantiate(floorTiles[selection], new Vector3(x, y), Quaternion.identity, transform);
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
                    Instantiate(obstacles1[0], new Vector3(x, y), Quaternion.identity, transform);
                }
            }
        }
    }

    private void PlaceObstacles() {
        //foreach(KeyValuePair<Vector2,bool> obstacle in obstacleMap) {




        //    List<Vector2> neighbours = new List<Vector2>();
        //    //int caseNumber = 0;

        //    Vector2 tempNorth = obstacle.Key + Vector2.up;
        //    Vector2 tempEast = obstacle.Key + Vector2.right;
        //    Vector2 tempSouth = obstacle.Key + Vector2.down;
        //    Vector2 tempWest = obstacle.Key + Vector2.left;

        //    if (obstacleMap.ContainsKey(tempNorth) || !obstacleMap[tempNorth] ) {
        //        neighbours.Add(tempNorth);
        //    }
        //    if (obstacleMap.ContainsKey(tempEast)) {
        //        neighbours.Add(tempEast);
        //    }
        //    if (obstacleMap.ContainsKey(tempSouth)) {
        //        neighbours.Add(tempSouth);
        //    }
        //    if (obstacleMap.ContainsKey(tempWest)) {
        //        neighbours.Add(tempWest);
        //    }
        //}
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
                Debug.Log("int light");
                Instantiate(torch, possibleLight, Quaternion.identity, transform);
            }
        }
    }
}
