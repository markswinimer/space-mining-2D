using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class ShootingScript : MonoBehaviour
{

    public GameObject bulletPrefab;
    public Transform firePointTransform;

    public Transform turret;

    void Awake()
    {
    }
    void Update()
    {
        HandleAiming();
        HandleShooting();
    }

    void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(bulletPrefab, firePointTransform.position, firePointTransform.rotation);
        }
    }
    void HandleAiming()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 aimDirection = (mousePos - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        turret.eulerAngles = new Vector3(0, 0, angle);
    }
}
