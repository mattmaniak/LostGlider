using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{
    const float initialAltitude = 2.0f;
    const float maxFallingSpeed = 0.1f; // Per one second.
    const float maxSpeed = 2.0f;
    const float positionLimitX = 100.0f;

    public static bool Alive { get; set; }
    public static float Speed { get; private set; }

    static bool Movement
    {
        set => Speed = value ? maxSpeed : 0.0f;
    }

    bool InAirStream { get => LiftRatio != 0.0f; }
    float LiftRatio { get; set; }

    void Start()
    {
        Alive = true;
        transform.Translate(0.0f, initialAltitude, 0.0f);
    }

    void FixedUpdate()
    {
        var rigidbody = GetComponent<Rigidbody2D>();

        if (InAirStream)
        {
            transform.Translate(Vector2.up * LiftRatio * Time.deltaTime);
        }
        if (transform.position.x >= positionLimitX)
        {
            Movement = false;
        }
        if ((rigidbody.velocity.y < 0.0f)
            && (rigidbody.velocity.magnitude > maxFallingSpeed))
        {
            rigidbody.velocity = rigidbody.velocity.normalized
                                 * maxFallingSpeed;
        }
        transform.Translate(Vector2.right * Speed * Time.deltaTime);
    }

    void Update()
    {
        CheckPause();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.isTrigger)
        {
            KillPlayer();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name.Contains("AirStream"))
        {
            LiftRatio
                = collider.gameObject.GetComponent<AirStream>().LiftRatio;
        }
    }

    void OnTriggerExit2D()
    {
        LiftRatio = 0.0f;
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
