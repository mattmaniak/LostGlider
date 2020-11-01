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

    static bool ControlsEnabled { get; set; }
    bool JoystickPressed { get; set; }
    float DeltaDirection { get; set; }
    float InnerJoysticSliderSize { get; set; }
    Vector2 DragPoint { get; set; }

    void Start()
    {
        ControlsEnabled = true;
        InnerJoysticSliderSize = GetComponent<SpriteRenderer>().bounds.size.y
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
        if (!FindObjectOfType<Player>().Alive)
        {
            ControlsEnabled = false;
        }
        bool leftMouseButtonHeld = Input.GetMouseButton(0);
        
        if (ControlsEnabled && leftMouseButtonHeld)
        {
            RaycastHit2D hit;

            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(
                Input.mousePosition);

            Vector2 mousePosition2D = new Vector2(mousePosition.x,
                mousePosition.y);

            hit = Physics2D.Raycast(mousePosition2D, Vector2.zero);

            if ((hit.collider != null)
                && (hit.collider.gameObject.name == "OuterJoystick"))
            {
                JoystickPressed = true;
            }
        }
        else
        {
            JoystickPressed = false;
        }
        CalculateJoystickPosition();
    }


    public static void DisableControls()
    {
        ControlsEnabled = false;
    }

    void CalculateJoystickPosition()
    {
        var touchPosition = Camera.main.ScreenToWorldPoint(
            new Vector3(0.0f, Input.mousePosition.y, 0.0f));

        var joystickPosition = new Vector2(transform.position.x,
                                           touchPosition.y);

        if (ControlsEnabled && JoystickPressed)
        {
            // TODO MATHF.CLAMP DOESN"T WORK.
            if (joystickPosition.y
                > (transform.position.y + (InnerJoysticSliderSize / 2.0f)))
            {
                joystickPosition.y = transform.position.y
                    + (InnerJoysticSliderSize / 2.0f);
            }
            if (joystickPosition.y
                < (transform.position.y - (InnerJoysticSliderSize / 2.0f)))
            {
                joystickPosition.y = transform.position.y
                    - (InnerJoysticSliderSize / 2.0f);
            }
            innerJoystick.transform.position = DragPoint = joystickPosition;
        }
        else
        {
            innerJoystick.transform.position = DragPoint = transform.position;
            DeltaDirection = 0.0f;
        }
    }

    void ControlByJoystick()
    {
        if (JoystickPressed)
        {
            DeltaDirection = (transform.position.y - DragPoint.y)
                / (InnerJoysticSliderSize / 2.0f);
        }
    }

    [Obsolete("Use only for debugging/testing purposes.")]
    void ControlByKeyboard()
    {
        if (ControlsEnabled && !JoystickPressed)
        {
            DeltaDirection = -Input.GetAxis("Vertical");
            if (Input.GetKey("escape"))
            {
                Utils.UnityQuit.Quit();
            }
        }
    }

    void MovePlayerVertically()
    {
        float deltaY = FindObjectOfType<Player>().Speed * DeltaDirection
            * Time.deltaTime;
        player.transform.Translate(new Vector3(0.0f, deltaY, 0.0f));
    }

    void SwitchPlayerGravity()
    {
        if (FindObjectOfType<Menus.PauseMenuController>().Paused)
        {
            playerRigidbody.Sleep();
            ControlsEnabled = false;
        }
        else if (FindObjectOfType<Player>().Alive)
        {
            playerRigidbody.WakeUp();
            ControlsEnabled = true;
        }
    }
}
