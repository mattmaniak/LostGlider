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

    public static float Speed
    {
        get => speed;
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
            speed = 0.0f;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ground")
        {
            Controls.DisableControls();
        }
    }

    void CheckPause()
    {
        speed = PauseMenuController.Paused ? 0.0f : maxSpeed;
    }
}
