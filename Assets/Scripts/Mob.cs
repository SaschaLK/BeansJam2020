using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Mob : MonoBehaviour {

    public int damage;
    AIPath _AIPath;
    Animator _Animator;

    private void Start() {
        if (MobManager.instance != null && MobManager.instance.player != null) {
            GetComponent<AIDestinationSetter>().target = MobManager.instance.player;
            MobManager.instance.mobs.Add(gameObject);
            _AIPath = GetComponentInChildren<AIPath>();
        }
        _Animator = GetComponentInChildren<Animator>();
    }

    public void Killed() {
        MobManager.instance.kills++;
    }

    public void Update()
    {
        if (_AIPath == null)
        {
            _AIPath = GetComponentInChildren<AIPath>();
        }

        var isWalking = _AIPath.canMove;

        if (_Animator != null)
        {
            _Animator.SetBool("IsWalking", isWalking);
        }
    }

}
