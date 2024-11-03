using UnityEngine;

public interface IDamageAbleObject
{
    public void TakeDamage(float damage);
    public void TakeDamageWithForce(float damage, Vector2 force);
}