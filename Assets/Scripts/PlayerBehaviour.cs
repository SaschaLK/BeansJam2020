using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public enum MovementType { Global, LookAndMove };

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
    public HealthBar _UIHealthBar;

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
    Vector2 _DraggingPointStart;
    Vector2 _DraggingPointEnd;

    Rigidbody2D _Rigidbody;

    public bool IsWalking;
    public bool IsDragging;

    public GameObject DraggingArrowObject;

    public GameObject CurrentDraggingArrowObject;

    //float _DraggingTime;

    Animator _Animator;
    private Vector2 _DraggingVector;

    public AudioClip[] AudioClipFootSteps;
    public AudioSource AudioSourceFootSteps;
    public AudioSource AudioSourceShootSlingShot;

    GameObject _SpriteHealthBar;
    

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
            Projectiles.Add(new ItemSlot { Item = ProjectileBomb, Count = 3.0f, CoolDown = 0.1f });
        }

        CurrentProjectile = Projectiles.FirstOrDefault();

        _Animator = this.GetComponentInChildren<Animator>();

        MovementType = (MovementType)PlayerPrefs.GetInt("MovementPreference");
        
        if (AudioController.Instance != null)
        {
            AudioController.Instance.ChangeTheme(TrackThemes.Alive);
        }

        
    }


    // Update is called once per frame
    void Update()
    {
        //if (_UIHealthBar == null)
        //{
        //    if (GameManager.Instance != null)
        //    {
        //        _UIHealthBar = GameManager.Instance.HealthBar.GetComponent<HealthBar>();
        //        _UIHealthBar.SetMaxHealth(HitPoints);
        //    }
        //}
        if (_SpriteHealthBar == null)
        {
            _SpriteHealthBar = GameManager.Instance.HealthBar;
            //Debug.Log(_SpriteHealthBar);
        }
        

        //==== Movement
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        var dt = Time.deltaTime;

        //Get mouse position from screen coordinates
        var mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;

        //This describes the mouse position in world coordinates
        var mouseWorldPosition = (Vector2)Camera.main.ScreenToWorldPoint(mousePos);

        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            CurrentProjectile = Projectiles[0];
        }

        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            CurrentProjectile = Projectiles[1];
        }
        

        if (Input.GetButton("Fire1"))
        {
            if (_RemainCoolDown == 0)
            {
                if (IsDragging == false)
                {
                    _DraggingPointStart = mouseWorldPosition;
                    CurrentDraggingArrowObject = Instantiate(DraggingArrowObject, GameManager.Instance.ProjectilesGroup.transform);
                }
                IsDragging = true;
                
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            if (IsDragging)
            {
                IsDragging = false;
                Destroy(CurrentDraggingArrowObject);
            }

            if (_RemainCoolDown == 0)
            {
                Fire();
            }
        }

        _PositionLookAt = mouseWorldPosition;

        if (IsDragging)
        {
            //_DraggingTime += dt;
            _DraggingPointEnd = mouseWorldPosition;
            _DraggingVector = _DraggingPointStart - _DraggingPointEnd;

            var intensity = _DraggingVector.magnitude;
            var shootingVector = (_PositionLookAt- (Vector2)transform.position).normalized;
            var from = (_PositionLookAt + shootingVector * intensity);
            var to = _PositionLookAt - shootingVector * intensity;


            Debug.DrawLine(from, to, Color.yellow);

            UpdateGameObjectBetweenTwoPoints(CurrentDraggingArrowObject, from, to);
        }
        
        //Calculates final rotation
        var angle = this.AngleBetweenTwoPoints(_PositionLookAt, transform.position);
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        _Movement = new Vector2(horizontal, vertical);

        IsWalking = _Movement != Vector2.zero;

        _Animator.SetBool("IsWalking", IsWalking);
        _Animator.SetBool("IsDragging", IsDragging);

        //==== HealthBar
        if (_SpriteHealthBar != null)
        {
            //_SpriteHealthBar.transform.position = (Vector2)transform.position + Vector2.up * 2.0f;
            //_SpriteHealthBar.transform.rotation = Quaternion.identity;
            
        }

        //==== Audio
        if (IsWalking)
        {
            if (AudioSourceFootSteps.isPlaying == false)
            {
                AudioSourceFootSteps.clip = AudioClipFootSteps[Random.Range(0, AudioClipFootSteps.Length - 1)];
                
                AudioSourceFootSteps.PlayDelayed(0f);
            }
        }
        else
        {
            AudioSourceFootSteps.Stop();
        }
    }

    public void UpdateGameObjectBetweenTwoPoints(GameObject prefab, Vector2 from, Vector2 to)
    {
        var center = (to + from) / 2.0f;
        prefab.transform.position = center;

        var direction = (to - from);
        var directionNormalized = direction.normalized;

        prefab.transform.up = -directionNormalized;

        var distance = Mathf.Min(Mathf.Max(1f, direction.magnitude), 4f);
        prefab.transform.localScale = Vector2.one * distance;
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

        if (Input.GetKeyUp(KeyCode.B))
        {
            GameManager.Instance.AddForceAtPosition(_PositionLookAt, 10000, 10f);
        }
    }

    public void Fire()
    {
        var projectile = Instantiate(CurrentProjectile.Item, GameManager.Instance.ProjectilesGroup.transform);
        var intensity = Mathf.Abs(_DraggingVector.magnitude);

        projectile.transform.position += transform.position + transform.up * 1.5f;
        var projectileBehaviour = projectile.GetComponent<ProjectileBehaviour>();
        projectileBehaviour.Direction = (_PositionLookAt - (Vector2)transform.position).normalized;
        var velocity = 10 + Mathf.Min(20f, intensity * 20);
        projectileBehaviour.SetVelocity(velocity);    //m/s

        //Audio
        AudioSourceShootSlingShot.PlayDelayed(0);

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

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.layer == 9){
            TakeDamage(collision.gameObject);
        }
    }

    private void TakeDamage(GameObject collisionObject) {

        var damage = 0;

        var mob = collisionObject.GetComponent<Mob>();
        
        if (mob != null)
        {
            damage = mob.Damage + (int)Random.Range(0f, 24f);
        }

        HitPoints -= damage;

        if (_UIHealthBar != null)
        {
            var healthBar = _UIHealthBar.GetComponent<HealthBar>();
            healthBar.SetHealth(HitPoints);
        }

        if(HitPoints <= 0) {
            //TO_DO Change to GameManager.instance.ChangeRealm()
            if (MobManager.instance != null)
            {
                HitPoints = 100;
                MobManager.instance.ChangeRealm();
            }
        }
    }
}
