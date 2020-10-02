using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Player : MonoBehaviour
{
    static float maxSpeed = 5.0f;

    public static float MaxSpeed
    {
        get { return maxSpeed; }
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
        transform.Translate(Vector3.right * maxSpeed * Time.deltaTime);
    }
}
