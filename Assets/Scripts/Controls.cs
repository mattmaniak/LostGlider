using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Controls : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D playerRigidbody;

    [SerializeField]
    Transform innerJoystick;
    
    [SerializeField]
    Transform player;

    const int paddingPx = 128;

    static bool controlsEnabled = true;
    bool joystickPressed;
    float deltaDirection;
    float innerJoysticSliderSize;
    Vector2 dragPoint;

    void Start()
    {
        innerJoysticSliderSize = GetComponent<SpriteRenderer>().bounds.size.y
                                 - innerJoystick.GetComponent<SpriteRenderer>().
                                   bounds.size.y;
    }

    void FixedUpdate()
    {
        SwitchPlayerGravity();
        ControlByJoystick();
        if (DebugUtils.GlobalEnabler.activated)
        {
            ControlByKeyboard();
        }
        MovePlayerVertically();
    }

    void Update()
    {
        if (!Player.Alive)
        {
            controlsEnabled = false;
        }
        bool leftMouseButtonHeld = Input.GetMouseButton(0);
        
        if (controlsEnabled && leftMouseButtonHeld)
        {
            Vector3 mousePosition = Camera.main.
                                    ScreenToWorldPoint(Input.mousePosition);

            Vector2 mousePosition2D = new Vector2(mousePosition.x,
                                                  mousePosition.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePosition2D, Vector2.zero);
            if ((hit.collider != null)
                && (hit.collider.gameObject.name == "OuterJoystick"))
            {
                joystickPressed = true;
            }
        }
        else
        {
            joystickPressed = false;
        }
        CalculateJoystickPosition();
    }


    public static void DisableControls()
    {
        controlsEnabled = false;
    }

    void CalculateJoystickPosition()
    {
        var touchPosition = Camera.main.ScreenToWorldPoint(
            new Vector3(0.0f, Input.mousePosition.y, 0.0f));

        var joystickPosition = new Vector2(transform.position.x,
                                           touchPosition.y);

        if (controlsEnabled && joystickPressed)
        {
            // TODO MATHF.CLAMP DOESN"T WORK.
            if (joystickPosition.y
                > (transform.position.y + (innerJoysticSliderSize / 2.0f)))
            {
                joystickPosition.y
                    = transform.position.y + (innerJoysticSliderSize / 2.0f);
            }
            if (joystickPosition.y
                < (transform.position.y - (innerJoysticSliderSize / 2.0f)))
            {
                joystickPosition.y
                    = transform.position.y - (innerJoysticSliderSize / 2.0f);
            }
            innerJoystick.transform.position = dragPoint = joystickPosition;
        }
        else
        {
            innerJoystick.transform.position = dragPoint = transform.position;
            deltaDirection = 0.0f;
        }
    }

    void ControlByJoystick()
    {
        if (joystickPressed)
        {
            deltaDirection = (transform.position.y - dragPoint.y)
                             / (innerJoysticSliderSize / 2.0f);
        }
    }

    [Obsolete("Use only for debugging/testing purposes.")]
    void ControlByKeyboard()
    {
        if (controlsEnabled && !joystickPressed)
        {
            deltaDirection = -Input.GetAxis("Vertical");
            if (Input.GetKey("escape"))
            {
                UnityQuit.Quit();
            }
        }
    }

    void MovePlayerVertically()
    {
        float deltaY = deltaDirection * Player.Speed * Time.deltaTime;
        player.transform.Translate(new Vector3(0.0f, deltaY, 0.0f));
    }

    void SwitchPlayerGravity()
    {
        if (PauseMenuController.Paused)
        {
            playerRigidbody.Sleep();
            controlsEnabled = false;
        }
        else if (Player.Alive)
        {
            playerRigidbody.WakeUp();
            controlsEnabled = true;
        }
    }
}
