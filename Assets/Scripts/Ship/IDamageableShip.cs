using UnityEngine;

public interface IDamageable
{
    public bool Invincible { get; set; }
    public void TakeDamage(float damage);
    public void TakeDamageWithForce(float damage, Vector2 force);
}