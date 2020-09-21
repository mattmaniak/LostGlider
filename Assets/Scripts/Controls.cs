#define DEBUG
#undef DEBUG

using System;
using UnityEngine;

public class Controls : MonoBehaviour
{
    [SerializeField]
    [Range(0.1f, 10.0f)]
    float playerSpeed = 2.0f;

    [SerializeField]
    Transform player;

    [SerializeField]
    Transform innerJoystick;
    
    [SerializeField]
    Transform outerJoystick;

    bool screenPressed;
    Vector2 deltaDirection;
    Vector2 dragPoint;
    Vector2 pressPoint;

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

    void CalculateJoystickPosition()
    {
        Vector3 worldPressPoint = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                        Camera.main.transform.position.z));

        // Button is clicked.
        if (Input.GetMouseButtonDown(0))
        {
            innerJoystick.transform.position =
                outerJoystick.transform.position = pressPoint = worldPressPoint;

            ToggleJoystickVisibility(true);
        }

        // Button is held.
        if (Input.GetMouseButton(0))
        {
            dragPoint = worldPressPoint;
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
            deltaDirection = Vector2.ClampMagnitude(dragPoint - pressPoint,
                                                    1.0f);
            innerJoystick.transform.position =
                new Vector2(pressPoint.x + deltaDirection.x,
                            pressPoint.y + deltaDirection.y);
        }
        else
        {
            deltaDirection = Vector2.zero;
            ToggleJoystickVisibility(false);
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

    void ToggleJoystickVisibility(bool visible)
    {
        innerJoystick.GetComponent<SpriteRenderer>().enabled =
            outerJoystick.GetComponent<SpriteRenderer>().enabled = visible;
    }
}
