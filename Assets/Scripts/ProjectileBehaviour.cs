using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    //public float MovementSpeed;
    public Vector2 Direction;

    public bool IsFlying;

    Rigidbody2D _RigidBody;

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
