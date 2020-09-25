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

    bool joystickPressed;
    Vector2 deltaDirection;
    Vector2 dragPoint;

    void Start()
    {

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
        bool touching = Input.GetMouseButton(0);

        // TODO: TEMPONARY X-AXIS SOLUTION.
        Vector3 touchPointWorld = Camera.main.ScreenToWorldPoint(
            new Vector3(Screen.width / 2, Input.mousePosition.y,
                        -Camera.main.transform.position.z));

        var joystickCollider = GetComponent<Collider2D>();
        var touchPosition = new Vector2(touchPointWorld.x,
                                        touchPointWorld.y);

        if (touching && joystickPressed
            && (Physics2D.OverlapPoint(touchPosition) == joystickCollider))
        {
            innerJoystick.transform.position = dragPoint = touchPointWorld;
        }
        else
        {
            innerJoystick.transform.position = deltaDirection = dragPoint
                = Vector2.zero;
        }
    }

    void ControlByJoystick()
    {
        var joystickPosition2D = new Vector2(transform.position.x,
                                             transform.position.y);

        if (joystickPressed)
        {
            deltaDirection
                = Vector2.ClampMagnitude(dragPoint - joystickPosition2D, 1.0f);

            innerJoystick.transform.position =
                new Vector2(transform.position.x + deltaDirection.x,
                            transform.position.y + deltaDirection.y);
        }
    }

    [Obsolete("Use only for debugging/testing purposes.")]
    void ControlByKeyboard()
    {
        if (!joystickPressed)
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
