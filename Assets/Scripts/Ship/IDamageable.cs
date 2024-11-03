using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(float damage);
    public void TakeDamageWithForce(float damage, Vector2 force);
}