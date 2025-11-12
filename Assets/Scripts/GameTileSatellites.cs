using UnityEngine;
using System.Collections.Generic;

public class GameTileSatellites : MonoBehaviour
{
    [Header("Satellite Settings")]
    [SerializeField] private GameObject[] satellitePrefabs; 
    [SerializeField] private int numberOfSatellites = 2;
    [SerializeField] private Camera mainCamera;
    
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 0.5f;
    [SerializeField] private float rotationSpeed = 10f;

    private Vector3 areaSize; // size of the tile in world space
    private List<SatelliteData> satellites = new List<SatelliteData>();
    
    private class SatelliteData
    {
        public GameObject gameObject;
        public float direction; // 1 for right, -1 for left
    }

    void Start()
    {
        // Find the main camera if not assigned
        if (mainCamera == null)
            mainCamera = Camera.main;
            
        // Get tile size
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
            areaSize = rend.bounds.size;
        else
        {
            Collider col = GetComponent<Collider>();
            areaSize = col != null ? col.bounds.size : Vector3.one;
        }
    }

    void Update()
    {
        // Update each satellite's movement
        foreach (SatelliteData satellite in satellites)
        {
            if (satellite.gameObject != null)
            {
                // Drift slowly left or right in world space (no rotation to keep Y stable)
                float drift = moveSpeed * satellite.direction * Time.deltaTime;
                satellite.gameObject.transform.Translate(drift, 0f, 0f, Space.World);
            }
        }
    }
    
    // Public function to spawn satellites, called by SceneController
    public void SpawnSatellites()
    {
        Debug.Log($"SpawnSatellites called on {gameObject.name}. Number to spawn: {numberOfSatellites}");
        
        if (satellitePrefabs == null || satellitePrefabs.Length == 0)
        {
            Debug.LogError("No satellite prefabs assigned!");
            return;
        }
        
        for (int i = 0; i < numberOfSatellites; i++)
        {
            GameObject prefab = satellitePrefabs[Random.Range(0, satellitePrefabs.Length)];

            // Spawn satellites at either left or right edge
            // Randomly choose left edge (-3.75) or right edge (3.75)
            bool spawnOnLeft = Random.value > 0.5f;
            float xPos = spawnOnLeft ? -3.75f : 3.75f;
            
            Vector3 localPos = new Vector3(
                xPos, // At left or right edge
                0f, // At Y = 0
                Random.Range(-5f, 5f) // Random Z within tile depth (10/2 = 5)
            );

            // Instantiate prefab as child of this tile (moves with tile)
            GameObject satellite = Instantiate(prefab, transform.position + localPos, Random.rotation, transform);

            // Apply scale for satellites
            satellite.transform.localScale = new Vector3(0.015f, 0.015f, 0.015f);
            
            // Ensure satellite has the correct tag
            if (!satellite.CompareTag("Satellite"))
            {
                satellite.tag = "Satellite";
                Debug.Log($"Set satellite tag to 'Satellite'");
            }
            
            // Check for required collision components
            if (satellite.GetComponent<Collider>() == null)
            {
                Debug.LogWarning($"Satellite prefab '{prefab.name}' is missing a Collider component!");
            }
            
            if (satellite.GetComponent<Rigidbody>() == null)
            {
                Debug.LogWarning($"Satellite prefab '{prefab.name}' is missing a Rigidbody component!");
            }
            
            if (satellite.GetComponent<SatelliteCollision>() == null)
            {
                Debug.LogWarning($"Satellite prefab '{prefab.name}' is missing SatelliteCollision script!");
            }
            
            // Determine direction: move toward center
            // If spawned on left edge, move right (positive direction = toward center)
            // If spawned on right edge, move left (negative direction = toward center)
            float direction = spawnOnLeft ? 1f : -1f;
            
            // Store the satellite data for animation
            satellites.Add(new SatelliteData
            {
                gameObject = satellite,
                direction = direction
            });
            
            Debug.Log($"Spawned satellite {i+1}/{numberOfSatellites} at {(spawnOnLeft ? "left" : "right")} edge, moving toward center");
        }
    }

    void OnDrawGizmosSelected()
    {
        Renderer rend = GetComponent<Renderer>();
        Vector3 drawSize = rend != null ? rend.bounds.size : Vector3.one;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, drawSize);
    }
}

