using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    private float _damage = 40;
    public Rigidbody2D rb;
    public GameObject impactEffect;

    private float _timeToLive = 4f;

    void Update()
    {
        _timeToLive -= Time.deltaTime;
        if (_timeToLive <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void AddForce (Vector2 forceDirection)
    {
        rb.AddForce(forceDirection, ForceMode2D.Impulse);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable target = other.GetComponent<IDamageable>();

        GameObject impactEffectVFX = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(impactEffectVFX, 2f);

        if (target != null)
        {
            target.TakeDamage(_damage);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
