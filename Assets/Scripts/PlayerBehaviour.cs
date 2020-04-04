using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public enum MovementType { LookAndMove, Global };

public struct ItemSlot
{
    public GameObject Item;
    public float Count;
    public float CoolDown;
}

public class PlayerBehaviour : MonoBehaviour
{
    public float MovementSpeed = 5f;
    public float StrafeSpeed = 2f;

    public int HitPoints = 100;

    public MovementType MovementType = MovementType.Global;

    public GameObject LightObject;
    Vector2 _PositionLookAt;

    public GameObject ProjectileStone;
    public GameObject ProjectileBomb;

    public List<ItemSlot> Projectiles;

    ItemSlot CurrentProjectile;

    float _RemainCoolDown = 0f;
    Vector2 _Direction;
    Vector2 _Movement;

    Rigidbody2D _Rigidbody;

    public bool IsWalking;
    public bool IsDragging;

    float _DraggingTime;

    Animator _Animator;

    // Start is called before the first frame update
    void Start()
    {
        Projectiles = new List<ItemSlot>();
        _Rigidbody = this.GetComponent<Rigidbody2D>();

        if (ProjectileStone != null)
        {
            Projectiles.Add(new ItemSlot { Item = ProjectileStone, Count = float.PositiveInfinity, CoolDown = 0.1f });
        }
        if (ProjectileBomb != null)
        {
            Projectiles.Add(new ItemSlot { Item = ProjectileBomb, Count = 3.0f, CoolDown = 5f });
        }

        CurrentProjectile = Projectiles.FirstOrDefault();

        _Animator = this.GetComponentInChildren<Animator>();

        MovementType = (MovementType)PlayerPrefs.GetInt("MovementPreference");

    }

    // Update is called once per frame
    void Update()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        var dt = Time.deltaTime;

        //Get mouse position from screen coordinates
        var mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;

        //This describes the mouse position in world coordinates
        var mouseWorldPosition = (Vector2)Camera.main.ScreenToWorldPoint(mousePos);

        _PositionLookAt = mouseWorldPosition;

        //Calculates final rotation
        var angle = this.AngleBetweenTwoPoints(_PositionLookAt, transform.position);
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        _Movement = new Vector2(horizontal, vertical);

        IsWalking = _Movement != Vector2.zero;
        _Animator.SetBool("IsWalking", IsWalking);
        //Debug.Log(_Animator.GetBool("IsWalking"));

        //Calculates target movement vector
        switch (MovementType)
        {
            case MovementType.Global:
                {
                    //var translation = (Vector2.up * vertical + Vector2.right * horizontal) * MovementSpeed * dt;
                    //this.gameObject.transform.Translate(translation, Space.World);
                }
                break;

            case MovementType.LookAndMove:
                {
                    //var translation = (transform.up * vertical * MovementSpeed + transform.right * horizontal * StrafeSpeed) * dt;
                    //this.gameObject.transform.Translate(translation, Space.World);
                }
                break;
        }

        if (Input.GetButton("Fire1"))
        {
            if (_RemainCoolDown == 0)
            {
                IsDragging = true;
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            if (IsDragging)
            {
                IsDragging = false;
            }

            if (_RemainCoolDown == 0)
            {
                Fire();
            }
        }

        if (IsDragging)
        {
            _DraggingTime += dt;
        }

        _Animator.SetBool("IsDragging", IsDragging);

    }

    private void FixedUpdate()
    {
        var dt = Time.fixedDeltaTime;
        var movement = _Movement;

        switch (MovementType)
        {
            case MovementType.LookAndMove:
                movement = (transform.up * _Movement.y + transform.right * _Movement.x).normalized;
                break;
        }
        var finalMovement = (Vector2)transform.position + movement * MovementSpeed * dt;

        _Rigidbody.MovePosition(finalMovement);

    }

    public void Fire()
    {
        var projectile = Instantiate(CurrentProjectile.Item, GameManager.Instance.ProjectilesGroup.transform);

        projectile.transform.position += transform.position + transform.up * 1.5f;
        var projectileBehaviour = projectile.GetComponent<ProjectileBehaviour>();
        projectileBehaviour.Direction = (_PositionLookAt - (Vector2)transform.position).normalized;
        projectileBehaviour.SetVelocity(20);    //m/s

        StartCoroutine(DoCooldown(CurrentProjectile.CoolDown));
    }

    IEnumerator DoCooldown(float coolDown)
    {
        _RemainCoolDown = coolDown;

        while (_RemainCoolDown > 0)
        {
            _RemainCoolDown -= Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        //Debug.Log("Cooldown terminated");
        _RemainCoolDown = 0;
    }

    float AngleBetweenTwoPoints(Vector2 a, Vector2 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg - 90f;
    }

}
