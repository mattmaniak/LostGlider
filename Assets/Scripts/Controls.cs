using System;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public float playerSpeed = 2.0f;
    public Transform player;

    bool _screenPressed;
    Vector2 _pressPoint;
    Vector2 _dragPoint;

    public Transform innerJoystick;
    public Transform outerJoystick;

    void FixedUpdate()
    {
        _controlByJoystick();
        _controlByKeyboard();
    }

    void Update()
    {
        _calculateJoystickPosition();
    }

    void _calculateJoystickPosition()
    {
        Vector3 worldPressPoint = Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                        Camera.main.transform.position.z));

        // Button is clicked.
        if (Input.GetMouseButtonDown(0))
        {
            innerJoystick.transform.position =
                outerJoystick.transform.position = _pressPoint = worldPressPoint;

            _toggleJoystickVisibility(true);
        }

        // Button is held.
        if (Input.GetMouseButton(0))
        {
            _dragPoint = worldPressPoint;
           _screenPressed = true;
        }
        else
        {
           _screenPressed = false;
        }
    }

    void _controlByJoystick()
    {
        Vector2 direction;

        if (_screenPressed)
        {
            direction = Vector2.ClampMagnitude(_dragPoint - _pressPoint, 1.0f);
            player.transform.Translate(direction * playerSpeed
                                       * Time.deltaTime);        

            innerJoystick.transform.position =
                new Vector2(_pressPoint.x + direction.x,
                            _pressPoint.y + direction.y);
        }
        else
        {
            _toggleJoystickVisibility(false);
        }
    }

    [Obsolete("Use only for debugging/testing purposes.")]
    void _controlByKeyboard()
    {
        if (!_screenPressed)
        {
            _movePlayer(new Vector2(Input.GetAxis("Horizontal"),
                                    Input.GetAxis("Vertical")));
        }
    }

    void _movePlayer(Vector2 direction)
    {
        player.transform.Translate(direction * playerSpeed * Time.deltaTime);    
    }

    void _toggleJoystickVisibility(bool visible)
    {
        innerJoystick.GetComponent<SpriteRenderer>().enabled =
            outerJoystick.GetComponent<SpriteRenderer>().enabled = visible;
    }
}
