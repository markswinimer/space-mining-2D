using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class ShootState : State
{
    public Transform firePointTransform;
    public GameObject bulletPrefab;
    public GameObject fireEffect;

    [SerializeField] private Weapon _weapon;

    public AudioSource shootSound;

    public override void Enter()
    {

    }

    public override void Do()
    {
        Shoot();
        Debug.Log("Shooting still");
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePointTransform.position, firePointTransform.rotation);
        Vector2 forceDirection = firePointTransform.right * _weapon.force;
        bullet.GetComponent<Bullet>().AddForce(forceDirection);
    }

  
    // // Spawn fire effect at the fire point
    // GameObject fireEffectVFX = Instantiate(fireEffect, firePoint.position, firePoint.rotation);
    // Destroy(fireEffectVFX, 2f);

    // // Play shooting sound
    // shootSound?.Play();
}