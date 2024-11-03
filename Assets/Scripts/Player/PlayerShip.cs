using UnityEngine;

public class PlayerShip : MonoBehaviour, IDamageable
{
    public bool Invincible { get; set; }
    public float health = 100f;
    public Rigidbody2D rb;

    public void TakeDamage(float damage)
    {
        if (!Invincible)
        {
            health -= damage;
            if (health <= 0)
            {
                Die();
            }
        }
    }

    public void TakeDamageWithForce(float damage, Vector2 forceDirection)
    {
        if (!Invincible)
        {
            health -= damage;
            rb.AddForce(forceDirection, ForceMode2D.Impulse);
            if (health <= 0)
            {
                Die();
            }
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
