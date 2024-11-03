using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class AttractionSphere : MonoBehaviour
{
    [SerializeField] private string oreLayerName = "Ore";
    [SerializeField] private Transform shipTransform;  // Reference to the ship position
    [SerializeField] private float maxDistance = 3f;   // Maximum tether distance for strong pull
    [SerializeField] private float removeDistance = 5f; // Distance at which the ore is released
    [SerializeField] private float pullStrength = 2f;  // Base strength of the pulling force

    [SerializeField] private int maxTetheredItems;

    private Dictionary<GameObject, LineRenderer> activeLines = new Dictionary<GameObject, LineRenderer>();

    InputAction removeOreAction;

    private void Awake()
    {
        removeOreAction = InputSystem.actions.FindAction("Interact2");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activeLines.Count >= maxTetheredItems) return;

        if (other.gameObject.layer == LayerMask.NameToLayer(oreLayerName))
        {
            // Check if the ore already has a LineRenderer component
            LineRenderer line = other.gameObject.GetComponent<LineRenderer>();
            if (line == null)
            {
                // Add LineRenderer only if it doesn’t already exist
                line = other.gameObject.AddComponent<LineRenderer>();
                ConfigureLineRenderer(line);
                activeLines.Add(other.gameObject, line);
            }
            else
            {
                // If it already has a LineRenderer, make sure it's tracked in the dictionary
                if (!activeLines.ContainsKey(other.gameObject))
                {
                    activeLines.Add(other.gameObject, line);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (activeLines.ContainsKey(other.gameObject) && Vector2.Distance(other.transform.position, shipTransform.position) >= removeDistance)
        {
            // Only remove the line and release ore if it exceeds the remove distance
            Destroy(activeLines[other.gameObject]);
            activeLines.Remove(other.gameObject);
        }
    }

    private void Update()
    {
        if (removeOreAction.triggered)
        {
            RemoveNewestOre();
        }

        foreach (var entry in activeLines)
        {
            GameObject ore = entry.Key;
            LineRenderer line = entry.Value;

            // Update line positions
            line.SetPosition(0, shipTransform.position);
            line.SetPosition(1, ore.transform.position);

            // Apply pulling force based on distance
            PullOreWithDistanceEffect(ore);
        }
        
    }

    private void ConfigureLineRenderer(LineRenderer line)
    {
        line.startWidth = 0.05f;
        line.endWidth = 0.05f;
        line.positionCount = 2;
        line.material = new Material(Shader.Find("Sprites/Default"));
        line.startColor = Color.cyan;
        line.endColor = Color.cyan;
    }

    private void PullOreWithDistanceEffect(GameObject ore)
    {
        Rigidbody2D oreRb = ore.GetComponent<Rigidbody2D>();
        if (oreRb == null) return;

        Vector2 directionToShip = (shipTransform.position - ore.transform.position).normalized;
        float distanceToShip = Vector2.Distance(shipTransform.position, ore.transform.position);

        // Only apply pull if within maxDistance and not exceeding removeDistance
        if (distanceToShip < removeDistance)
        {
            // Increase the pull strength as the ore gets closer to maxDistance
            float dynamicPullStrength = pullStrength * (distanceToShip / maxDistance);
            oreRb.AddForce(directionToShip * dynamicPullStrength);
        }
    }


    public void RemoveNewestOre()
    {
        // Check if there is at least one ore in the activeLines dictionary
        if (activeLines.Count > 0)
        {
            // Get the newest ore (last added entry in the dictionary)
            GameObject newestOre = null;
            foreach (var entry in activeLines)
            {
                newestOre = entry.Key;  // The last entry in the foreach loop is the newest
            }

            // Remove the newest ore's line renderer and dictionary entry
            if (newestOre != null)
            {
                Destroy(activeLines[newestOre]);
                activeLines.Remove(newestOre);
                Debug.Log("Removed newest ore: " + newestOre.name);
            }
        }
    }
}
