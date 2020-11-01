using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{
    const float initialAltitude = 2.0f;
    const float maxFallingSpeed = 0.1f; // Per one second.
    const float maxSpeed = 4.0f;
    const float maxPositionX = 100.0f;

    public bool Alive { get; set; }
    public float Speed { get; private set; }

    bool Movement { set => Speed = value ? maxSpeed : 0.0f; }
    bool InSoaringLift { get => LiftRatio != 0.0f; }
    float Altitude { get => transform.position.y; }
    float LiftRatio { get; set; }

    void Awake()
    {
        Alive = true;
        transform.Translate(-Camera.main.transform.localPosition.x,
                            initialAltitude, 0.0f);
    }

    void FixedUpdate()
    {
        var rigidbody = GetComponent<Rigidbody2D>();

        if (InSoaringLift)
        {
            transform.Translate(Vector2.up * LiftRatio * Time.deltaTime);
        }
        if (transform.position.x >= maxPositionX)
        {
            KillPlayer();
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
        if (collider.name.Contains("AtmosphericPhenomenon"))
        {
            LiftRatio = collider.gameObject.
                GetComponent<Level.AtmosphericPhenomenon>().LiftRatio;
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
            Movement = !FindObjectOfType<Menus.PauseMenuController>().Paused;
        }
    }

    void KillPlayer()
    {
        Movement = Alive = false;
        LiftRatio = 0.0f;
    }
}
