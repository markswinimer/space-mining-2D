using UnityEngine;
using System.Collections;

public class CollectZone : MonoBehaviour
{
    [SerializeField] private float pullStrength = 2f;  // Base strength of the pulling force
    [SerializeField] private GameObject _collectRadius;
    [SerializeField] private GameObject _collectPoint;

    [SerializeField] private string oreLayerName = "Ore";       // Layer name to check against
    [SerializeField] private string excludedTag = "Tethered";   // Tag name to exclude

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object is on the specified layer and its tag does not match the excluded tag
        if (other.gameObject.layer == LayerMask.NameToLayer(oreLayerName) && other.CompareTag(excludedTag) == false)
        {
            // Begin pulling the ore toward the collect point
            StartCoroutine(PullOreToCollectPoint(other.transform));
        }
    }

    private IEnumerator PullOreToCollectPoint(Transform ore)
    {
        while (Vector2.Distance(ore.position, _collectPoint.transform.position) > 0.1f)
        {
            Vector2 direction = (_collectPoint.transform.position - ore.position).normalized;
            ore.position = Vector2.MoveTowards(ore.position, _collectPoint.transform.position, pullStrength * Time.deltaTime);
            yield return null;
        }

        // Add ore to ship's inventory and destroy it
        CollectOre(ore.gameObject);
    }

    private void CollectOre(GameObject ore)
    {
        // Add ore to the ship's inventory (implement inventory logic as needed)
        Destroy(ore);
    }
}
