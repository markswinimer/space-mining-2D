using UnityEngine;

public class Cannon : Weapon
{
    public GameObject projectilePrefab;
    public Transform firePoint;

    public void Fire()
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * force, ForceMode2D.Impulse);
    }
    
}