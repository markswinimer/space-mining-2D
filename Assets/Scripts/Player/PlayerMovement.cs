using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour {
    
    private Rigidbody2D _rigidBody;

    private bool _thrusting;

    [SerializeField] private float _thrustForce = 10f;
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _maxVelocity = 5f;

    InputAction moveAction;
    InputAction boostAction;

    Vector2 moveInput;

    private void Awake() {
        _rigidBody = GetComponent<Rigidbody2D>();
        moveAction = InputSystem.actions.FindAction("Move");
        boostAction = InputSystem.actions.FindAction("Boost");
    }

    private void Update() {
        moveInput = moveAction.ReadValue<Vector2>();

        HandleRotation();
        ThrustForward(moveInput.y);
    }

    private void HandleRotation()
    {
        float rotationInput = moveInput.x;
        _rigidBody.rotation -= rotationInput * _rotationSpeed * Time.deltaTime;
    }

    void ClampVelocity()
    {
        float x = Mathf.Clamp(_rigidBody.linearVelocity.x, -_maxVelocity, _maxVelocity);
        float y = Mathf.Clamp(_rigidBody.linearVelocity.y, -_maxVelocity, _maxVelocity);

        _rigidBody.linearVelocity = new Vector2(x, y);
    }

    void ThrustForward(float thrustAmount)
    {
        // Apply forward thrust if thrustAmount is greater than 0
        if (thrustAmount > 0)
        {
            // Apply thrust force in the direction the ship is facing
            Vector2 force = transform.right * _thrustForce;
            _rigidBody.AddForce(force);
        }
        // Decelerate when thrustAmount is less than 0 (e.g., "S" key)
        else if (thrustAmount < 0)
        {
            // Reduce speed to zero without going backward
            if (_rigidBody.linearVelocity.magnitude > 0.1f)
            {
                // Apply a deceleration force opposite to the current velocity
                Vector2 decelerationForce = -_rigidBody.linearVelocity.normalized * _thrustForce * 0.5f;
                _rigidBody.AddForce(decelerationForce);
            }
            else
            {
                // If speed is almost zero, set velocity to zero
                _rigidBody.linearVelocity = Vector2.zero;
            }
        }

        // Gradually slow down if there is no input
        else
        {
            _rigidBody.linearVelocity = Vector2.Lerp(_rigidBody.linearVelocity, Vector2.zero, Time.deltaTime * 0.5f);
        }

        ClampVelocity();
    }


}