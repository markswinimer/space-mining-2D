using UnityEngine;

public class Ore : MonoBehaviour {
    public string oreName;
    public string description;

    [SerializeField] private Rigidbody2D _rigidBody;
    
    void Awake()
    {

    }
    
    void Update()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * 10);
        _rigidBody.linearVelocity = Vector2.Lerp(_rigidBody.linearVelocity, Vector2.zero, Time.deltaTime * 0.5f);
    }

    public void ApplyForce(Vector2 forceDirection)
    {
        _rigidBody.AddForce(forceDirection, ForceMode2D.Impulse);
    }
}