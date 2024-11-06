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
    private InputAction removeOreAction;

    private void Awake()
    {
        removeOreAction = InputSystem.actions.FindAction("Interact2");

        if (removeOreAction != null)
        {
            removeOreAction.Enable();  // Enable the action to listen for input
        }
        else
        {
            Debug.LogError("Remove ore action (Interact2) not found!");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activeLines.Count >= maxTetheredItems) return;

        if (other.gameObject.layer == LayerMask.NameToLayer(oreLayerName))
        {
            LineRenderer line = other.gameObject.GetComponent<LineRenderer>();
            if (line == null)
            {
                other.gameObject.tag = "Tethered";

                // Add LineRenderer and configure it
                line = other.gameObject.AddComponent<LineRenderer>();
                ConfigureLineRenderer(line);
            }

            // Only add to activeLines if it’s not already in the dictionary
            if (!activeLines.ContainsKey(other.gameObject))
            {
                activeLines.Add(other.gameObject, line);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (activeLines.ContainsKey(other.gameObject))
        {
            float distanceToShip = Vector2.Distance(other.transform.position, shipTransform.position);
            if (distanceToShip >= removeDistance)
            {
                RemoveOre(other.gameObject);
            }
        }
    }

    private void Update()
    {
        if (removeOreAction != null && removeOreAction.triggered)
        {
            Debug.Log("Remove ore action triggered!");
            RemoveNewestOre();
        }

        // Update lines and apply pulling force
        foreach (var oreEntry in new List<GameObject>(activeLines.Keys))
        {
            if (oreEntry == null) continue;

            LineRenderer line = activeLines[oreEntry];
            line.SetPosition(0, shipTransform.position);
            line.SetPosition(1, oreEntry.transform.position);

            PullOreWithDistanceEffect(oreEntry);
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

        if (distanceToShip < removeDistance)
        {
            float dynamicPullStrength = pullStrength * (distanceToShip / maxDistance);
            oreRb.AddForce(directionToShip * dynamicPullStrength);
        }
    }

    public void RemoveNewestOre()
    {
        if (activeLines.Count > 0)
        {
            GameObject newestOre = null;
            foreach (var entry in activeLines)
            {
                newestOre = entry.Key;
            }

            if (newestOre != null)
            {
                Debug.Log("should be removed");
                RemoveOre(newestOre);
            }
        }
    }

    private void RemoveOre(GameObject ore)
    {
        if (!activeLines.ContainsKey(ore)) return;
        Debug.Log("Removing ore: " + ore.name);
        LineRenderer line = activeLines[ore];
        Debug.Log("Removing line: " + line.name);
        if (line != null)
        {
            Destroy(line);
        }

        ore.tag = "Untagged";
        activeLines.Remove(ore);

        UntetherOre(ore);
    }

    public void UntetherOre(GameObject ore)
    {
        Collider2D collider = ore.GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false;
            collider.enabled = true;
        }
    }
}
