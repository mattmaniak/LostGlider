#undef DEBUG

using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]

public class Controls : MonoBehaviour
{
    [SerializeField]
    [Range(0.1f, 10.0f)]
    float playerSpeed = 10.0f;

    [SerializeField]
    Rigidbody2D playerRigidbody;

    [SerializeField]
    Transform innerJoystick;
    
    [SerializeField]
    Transform player;

    const int paddingPx = 128;
    const float playerMaxFallingSpeed = 0.25f; // Per one second.

    static bool controlsEnabled = true;
    bool joystickPressed;
    SpriteRenderer sprite;
    float deltaDirection;
    float innerJoysticSliderSize;
    Vector2 dragPoint;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();

        transform.position = Camera.main.ScreenToWorldPoint(
            new Vector3(Screen.width - sprite.bounds.size.x - paddingPx,
                        sprite.bounds.size.y + paddingPx,
                        -Camera.main.transform.position.z));

        innerJoysticSliderSize
            = sprite.bounds.size.y
            - innerJoystick.GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void FixedUpdate()
    {
        ControlByJoystick();
#if DEBUG
        ControlByKeyboard();
#endif
        MovePlayer();
    }

    void Update()
    {
        CalculateJoystickPosition();
#if DEBUG
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

    public static void DisableControls()
    {
        controlsEnabled = false;
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

        if (controlsEnabled && joystickPressed)
        {
            // TODO MATHF.CLAMP DOESN"T WORK.
            if (touchPosition.y
                > (transform.position.y + (innerJoysticSliderSize / 2.0f)))
            {
                touchPointWorld.y
                    = transform.position.y + (innerJoysticSliderSize / 2.0f);
            }
            if (touchPosition.y
                < (transform.position.y - (innerJoysticSliderSize / 2.0f)))
            {
                touchPointWorld.y
                    = transform.position.y - (innerJoysticSliderSize / 2.0f);
            }
            innerJoystick.transform.position = dragPoint = touchPointWorld;
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
            deltaDirection = Input.GetAxis("Vertical");
            if (Input.GetKey("escape"))
            {
                Application.Quit();
            }
        }
    }

    void MovePlayer()
    {
        float deltaY = deltaDirection * playerSpeed * Time.deltaTime;
        player.transform.Translate(new Vector3(0.0f, deltaY, 0.0f));

        if ((playerRigidbody.velocity.y < 0.0f)
            && (playerRigidbody.velocity.magnitude > playerMaxFallingSpeed))
        {
            playerRigidbody.velocity = playerRigidbody.velocity.normalized
                                       * playerMaxFallingSpeed;
        }
    }
}
