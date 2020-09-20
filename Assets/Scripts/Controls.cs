using System;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public float playerSpeed = 2.0f;
    public Transform player;

    bool screenPressed;
    Vector2 pressPoint;
    Vector2 dragPoint;

    public Transform innerJoystick;
    public Transform outerJoystick;

    void FixedUpdate()
    {
        ControlByJoystick();
        ControlByKeyboard();
    }

    void Update()
    {
        CalculateJoystickPosition();
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
        Vector2 direction;

        if (screenPressed)
        {
            direction = Vector2.ClampMagnitude(dragPoint - pressPoint, 1.0f);
            player.transform.Translate(direction * playerSpeed
                                       * Time.deltaTime);        

            innerJoystick.transform.position =
                new Vector2(pressPoint.x + direction.x,
                            pressPoint.y + direction.y);
        }
        else
        {
            ToggleJoystickVisibility(false);
        }
    }

    [Obsolete("Use only for debugging/testing purposes.")]
    void ControlByKeyboard()
    {
        if (!screenPressed)
        {
            MovePlayer(new Vector2(Input.GetAxis("Horizontal"),
                                   Input.GetAxis("Vertical")));
        }
    }

    void MovePlayer(Vector2 direction)
    {
        player.transform.Translate(direction * playerSpeed * Time.deltaTime);    
    }

    void ToggleJoystickVisibility(bool visible)
    {
        innerJoystick.GetComponent<SpriteRenderer>().enabled =
            outerJoystick.GetComponent<SpriteRenderer>().enabled = visible;
    }
}
