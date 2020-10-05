using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{
    const float maxSpeed = 5.0f;
    const float positionLimitX = 100.0f;

    static float speed = 0.0f;

    public static float Speed
    {
        get => speed;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Ground")
        {
            Controls.DisableControls();
        }
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

    void CheckPause()
    {
        speed = MenuController.Paused ? 0.0f : maxSpeed;
        Debug.Log(MenuController.Paused);
    }
}
