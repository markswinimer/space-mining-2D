using UnityEngine;

public class Weapon : MonoBehaviour {
    public float fireRate = 0.5f;
    public float damage = 10f;
    public float force = 100f;
    public float range = 100f;

    [SerializeField] private Transform firePointTransform;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject fireEffect;
    [SerializeField] private AudioSource shootSound;

    public void PrimaryAction()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePointTransform.position, firePointTransform.rotation);
        Vector2 forceDirection = firePointTransform.right * force;
        bullet.GetComponent<Bullet>().AddForce(forceDirection);
    }

    public void SecondaryAction()
    {
        
    }
}