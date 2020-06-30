using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float health = 100;

    private void Update()
    {
        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        print("Enemy Destroyed");
        Destroy(this.gameObject);
    }

    public void TakeDamage(float value)
    {
        health -= value;
        print(health);
    }
}
