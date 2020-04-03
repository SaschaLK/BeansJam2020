using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Mob : MonoBehaviour {

    //public float speed;

    //private Transform player;

    private void Start() {
        GetComponent<AIDestinationSetter>().target = MobManager.instance.player;
        //player = MobManager.instance.player;
    }

    private void Update() {
        //transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
    }

}
