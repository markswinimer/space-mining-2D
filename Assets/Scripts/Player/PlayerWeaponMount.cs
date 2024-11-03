using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponMount : MonoBehaviour 
{
    public Vector2 mousePosition;
    public Vector2 direction;
    private float angle;

    public Weapon _mountedWeapon;

    private float _shootDelay;
    private float _timeToShoot;

    InputAction primaryAction;
    InputAction secondaryAction;

    private void Awake()
    {
        primaryAction = InputSystem.actions.FindAction("PrimaryAction");
        secondaryAction = InputSystem.actions.FindAction("SecondaryAction");
    }

    void Start()
    {
        _shootDelay = _mountedWeapon.fireRate;
        _timeToShoot = 0f;
    }

    void Update()
    {
        _timeToShoot -= Time.deltaTime;

        HandleDirection();
        HandleInput();
    }

    void HandleDirection()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 aimDirection = (mousePos - transform.position).normalized;
        angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        _mountedWeapon.transform.eulerAngles = new Vector3(0, 0, angle);
    }

    void HandleInput()
    {
        if ( primaryAction.IsPressed() && _timeToShoot <= 0f )
        {
            _timeToShoot = _shootDelay;
            _mountedWeapon.PrimaryAction();
        }
        else if (secondaryAction.IsPressed())
        {
            _mountedWeapon.SecondaryAction();
        }
    }
}