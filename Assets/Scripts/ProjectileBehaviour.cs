using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    //public float MovementSpeed;
    public Vector2 Direction;

    public bool IsFlying;

    Rigidbody2D _RigidBody;

    public int BaseDamage = 10;

    public float LifeTime = 10000;

    bool _IsDying;

    // Start is called before the first frame update
    void Start()
    {
        _RigidBody = this.GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        if (_IsDying == false)
        {
            StartCoroutine(DoDying());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            //TakeDamage(collision.gameObject);
            var mob = collision.gameObject.GetComponent<Mob>();
            var damage = (int)(Mathf.Max(0f, (BaseDamage + Mathf.Abs(_RigidBody.velocity.magnitude) * Random.Range(-1f, 5f))));
            mob.TakeDamage(damage);
            
        }
        LifeTime = 10;
    }

    //Destroys projectile after LifeTime
    IEnumerator DoDying()
    {
        while (LifeTime > 0)
        {
            LifeTime -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Destroy(this.gameObject);
    }

    public void SetVelocity(float velocity)
    {
        if (_RigidBody == null)
        {
            _RigidBody = this.GetComponent<Rigidbody2D>();
        }
        _RigidBody.velocity = Direction * velocity;
    }
}
