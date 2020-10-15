using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{
    const float initialAltitude = 2.0f;
    const float maxSpeed = 4.0f;
    const float positionLimitX = 100.0f;

    public static bool Alive { get; set; }

    public static float Speed { get; private set; }

    static bool Movement
    {
        set => Speed = value ? maxSpeed : 0.0f;
    }

    void Start()
    {
        Alive = true;
        transform.Translate(0.0f, initialAltitude, 0.0f);
    }

    void Update()
    {
        CheckPause();
        transform.Translate(Vector3.right * Speed * Time.deltaTime);

        if (transform.position.x >= positionLimitX)
        {
            Movement = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.isTrigger)
        {
            KillPlayer();
        }
    }

    void CheckPause()
    {
        if (Alive)
        {
            Movement = !PauseMenuController.Paused;
        }
    }

    void KillPlayer()
    {
        Movement = Alive = false;
    }
}
