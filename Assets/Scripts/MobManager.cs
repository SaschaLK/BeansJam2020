using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobManager : MonoBehaviour {

    public static MobManager instance;

    public Transform player;

    private void Awake() {
        instance = this;
    }

}
