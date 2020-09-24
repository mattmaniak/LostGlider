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
    Transform outerJoystick;    

    [SerializeField]
    Transform player;

    bool screenPressed;
    Vector2 deltaDirection;
    Vector2 dragPoint;
    Vector2 touchPoint;

    void Start()
    {
        touchPoint = new Vector2(outerJoystick.transform.position.x,
                                 outerJoystick.transform.position.y);
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

    void CalculateJoystickPosition()
    {
        Vector3 touchPointWorld = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                        Camera.main.transform.position.z));

        // Button is clicked.
        if (Input.GetMouseButtonDown(0))
        {
            var joystickCollider = outerJoystick.GetComponent<Collider2D>();
            var touchPosition = new Vector2(touchPointWorld.x,
                                            touchPointWorld.y);

            if (Physics2D.OverlapPoint(touchPosition) == joystickCollider)
            {
                innerJoystick.transform.position = touchPointWorld;
            }
        }

        // Button is held.
        if (Input.GetMouseButton(0))
        {
            dragPoint = touchPointWorld;
            screenPressed = true;
        }
        else
        {
            screenPressed = false;
        }
    }

    void ControlByJoystick()
    {
        if (screenPressed)
        {
            deltaDirection = Vector2.ClampMagnitude(dragPoint - touchPoint,
                                                    1.0f);
            innerJoystick.transform.position =
                new Vector2(touchPoint.x + deltaDirection.x,
                            touchPoint.y + deltaDirection.y);
        }
        else
        {
            innerJoystick.transform.position = deltaDirection = dragPoint
                = Vector2.zero;
        }
    }

    [Obsolete("Use only for debugging/testing purposes.")]
    void ControlByKeyboard()
    {
        if (!screenPressed)
        {
            deltaDirection = new Vector2(Input.GetAxis("Horizontal"),
                                         Input.GetAxis("Vertical"));
            if (Input.GetKey("escape"))
            {
                Application.Quit();
            }
        }
    }

    void MovePlayer()
    {
        player.transform.Translate(deltaDirection * playerSpeed
                                   * Time.deltaTime);    
    }
}
