using System;
using UnityEngine;

public class PlayerSystem : MonoBehaviour
{
    private float health = 200f;

    private void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "EnemyEffect")
        {
            health -= 50;
            Debug.Log("Ouch! Hit, current health is : " + health);
        }
    }
}
