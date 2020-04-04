using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    //public float MovementSpeed;
    public Vector2 Direction;

    public bool IsFlying;

    Rigidbody2D _RigidBody;

    // Start is called before the first frame update
    void Start()
    {
        _RigidBody = this.GetComponent<Rigidbody2D>();
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
