using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public float MovementSpeed = 5f;
    public float StrafeSpeed = 2f;

    public int HitPoints = 100;

    // Start is called before the first frame update
    void Start()
    {
        
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
        var angle = this.AngleBetweenTwoPoints(mouseWorldPosition, transform.position);

        //Calculates final rotation
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        //Calculates target movement vector
        var translation = (transform.up * vertical * MovementSpeed + transform.right * horizontal * StrafeSpeed) * dt;
        this.gameObject.transform.Translate(translation, Space.World);
    }

    float AngleBetweenTwoPoints(Vector2 a, Vector2 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg - 90f;
    }

}
