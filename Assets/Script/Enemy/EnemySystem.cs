using UnityEngine;

public class EnemySystem : MonoBehaviour
{
    private float health = 150f;

    void Update()
    {
        if (health <= 0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Effect")
        {
            health -= 15f;
        }
    }
}
