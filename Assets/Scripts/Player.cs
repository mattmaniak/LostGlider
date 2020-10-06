using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{
    const float initialAltitude = 1.75f;
    const float maxSpeed = 1.0f;
    const float positionLimitX = 100.0f;

    static float speed = 0.0f;

    static bool alive = true;

    public static bool Alive
    {
        get => alive;
    }

    public static float Speed
    {
        get => speed;
    }

    static bool Movement
    {
        set => speed = value ? maxSpeed : 0.0f;
    }

    void Start()
    {
        transform.Translate(0.0f, initialAltitude, 0.0f);
    }

    void Update()
    {
        CheckPause();
        transform.Translate(Vector3.right * speed * Time.deltaTime);

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
        if (alive)
        {
            Movement = !PauseMenuController.Paused;
        }
    }

    void KillPlayer()
    {
        Movement = alive = false;
    }
}
