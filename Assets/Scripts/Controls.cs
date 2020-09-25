#undef DEBUG

using System;
using UnityEngine;

public class Controls : MonoBehaviour
{
    [SerializeField]
    [Range(0.1f, 10.0f)]
    float playerSpeed = 2.0f;

    [SerializeField]
    Transform innerJoystick;
    
    [SerializeField]
    Transform player;

    const int paddingPx = 128;

    bool joystickPressed;
    SpriteRenderer sprite;
    float deltaDirection;
    Vector2 dragPoint;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        transform.position = Camera.main.ScreenToWorldPoint(
            new Vector3(Screen.width - sprite.bounds.size.x - paddingPx,
                        sprite.bounds.size.y + paddingPx,
                        -Camera.main.transform.position.z));
    }

    void FixedUpdate()
    {
        ControlByJoystick();
#if (DEBUG)
        ControlByKeyboard();
#endif

        MovePlayer();
    }

    void Update()
    {
        CalculateJoystickPosition();
#if (DEBUG)
        Debug.Log("Player position: " + player.transform.position);
#endif
    }

    void OnMouseDown()
    {
        joystickPressed = true;
    }

    void OnMouseUp()
    {
        joystickPressed = false;
    }

    void CalculateJoystickPosition()
    {
        Vector3 touchPointWorld = Camera.main.ScreenToWorldPoint(
            new Vector3(Screen.width - sprite.bounds.size.x - paddingPx,
                        Input.mousePosition.y,
                        -Camera.main.transform.position.z));

        var joystickCollider = GetComponent<Collider2D>();
        var touchPosition = new Vector2(transform.position.x,
                                        touchPointWorld.y);

        if (joystickPressed
            && (Physics2D.OverlapPoint(touchPosition) == joystickCollider))
        {
            innerJoystick.transform.position = dragPoint = touchPointWorld;
        }
        else
        {
            innerJoystick.transform.position = dragPoint
                = transform.position;

            deltaDirection = 0.0f;
        }
    }

    void ControlByJoystick()
    {
        if (joystickPressed)
        {
            deltaDirection = dragPoint.y - transform.position.y;
                // = Vector2.ClampMagnitude(dragPoint - joystickPosition2D, 1.0f);

            Debug.Log(deltaDirection);

            innerJoystick.transform.position =
                new Vector2(transform.position.x,
                            transform.position.y + deltaDirection);
        }
    }

    [Obsolete("Use only for debugging/testing purposes.")]
    void ControlByKeyboard()
    {
        if (!joystickPressed)
        {
            deltaDirection = Input.GetAxis("Vertical");
            if (Input.GetKey("escape"))
            {
                Application.Quit();
            }
        }
    }

    void MovePlayer()
    {
        player.transform.Translate(
            new Vector3(0.0f, deltaDirection * playerSpeed * Time.deltaTime,
                        0.0f));    
    }
}
