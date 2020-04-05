using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

public class Mob : MonoBehaviour {

    public int HitPoints = 50;
    public int Damage;

    public bool IsDying;

    AIPath _AIPath;
    Animator _Animator;

    public float TimeToDeath;

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

    internal void TakeDamage(int damage)
    {
        HitPoints -= damage;

        if (HitPoints < 0)
        {
            IsDying = true;
            _AIPath.canMove = false;
            StartCoroutine(DoDying(1.2f));
            _Animator.SetBool("IsDying", IsDying);
            if (MobManager.instance != null)
            {
                MobManager.instance.KillMob(this.gameObject);
            }
        }
    }

    IEnumerator DoDying(float timeToDeath)
    {
        print(GetComponentInChildren<Animator>().name);
        if (GetComponentInChildren<Animator>().name == "Mob_Sprite01")
        GetComponent<Rigidbody2D>().simulated = false;
        TimeToDeath = timeToDeath;

        while (TimeToDeath > 0)
        {
            TimeToDeath -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Killed();
        Destroy(this.gameObject);
    }
}
