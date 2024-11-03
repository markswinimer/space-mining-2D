using UnityEngine;

public class Asteroid : MonoBehaviour, IDamageable
{
    public float health = 30f;
    public float damage = 10f;
    public float force = .2f;

    public Rigidbody2D rb;
    public GameObject impactEffect;
    public GameObject destroyEffect;

    [SerializeField] private Ore _ore;
    private int _avgOreDrop = 3;

    public void AddForce(Vector2 forceDirection)
    {
        rb.AddForce(forceDirection, ForceMode2D.Impulse);
    }

    // when a ship collides with this
    void OnCollisionEnter2D(Collision2D other) 
    {
        IDamageable target = other.gameObject.GetComponent<IDamageable>();

        GameObject impactEffectVFX = Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(impactEffectVFX, 2f);

        if (target != null)
        {
            target.TakeDamageWithForce(damage, rb.linearVelocity.normalized * force);
        }
    }
    
    public void TakeDamage(float damage)
    {
        Debug.Log("Asteroid taking damage");
        health -= damage;
        if (health <= 0)
        {
            HandleDestroy();
        }
    }

    public void TakeDamageWithForce(float damage, Vector2 forceDirection)
    {
        Debug.Log("Asteroid taking damage with force");
        health -= damage;
        rb.AddForce(forceDirection, ForceMode2D.Impulse);

        if (health <= 0)
        {
            HandleDestroy();
        }
    }

    void HandleDestroy()
    {
        // drop ore
        for (int i = 0; i < _avgOreDrop; i++)
        {
            Ore ore = Instantiate(_ore, transform.position, transform.rotation);
            // apply a veryyy small force
            ore.ApplyForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * force);
        }

        GameObject destroyEffectVFX = Instantiate(destroyEffect, transform.position, transform.rotation);
        Destroy(destroyEffectVFX, 4f);
        
        Destroy(gameObject);
    }
}