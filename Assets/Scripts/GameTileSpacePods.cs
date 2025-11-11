using UnityEngine;
using System.Collections.Generic;

public class GameTileSpacePods : MonoBehaviour
{
    [Header("Space Pod Settings")]
    [SerializeField] private GameObject[] spacePodPrefabs; 
    [SerializeField] private int numberOfSpacePods = 3;
    [SerializeField] private Camera mainCamera;
    
    [Header("Movement Settings")]
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float diagonalSpeed = 0.5f;
    [SerializeField] private float diagonalRange = 0.3f;

    private Vector3 areaSize; // size of the tile in world space
    private List<SpacePodData> spacePods = new List<SpacePodData>();
    
    private class SpacePodData
    {
        public GameObject gameObject;
        public Vector3 startPosition;
        public float timeOffset;
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
        // Update each space pod's rotation and left-right oscillation
        foreach (SpacePodData pod in spacePods)
        {
            if (pod.gameObject != null)
            {
                // Rotate the space pod
                pod.gameObject.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
                
                // Apply left-to-right oscillation (using localPosition since it's a child)
                float time = Time.time * diagonalSpeed + pod.timeOffset;
                Vector3 offset = new Vector3(
                    Mathf.Sin(time) * diagonalRange,
                    0f,
                    0f
                );
                
                pod.gameObject.transform.localPosition = pod.startPosition + offset;
            }
        }
    }
    
    // Public function to spawn space pods, called by SceneController
    public void SpawnSpacePods()
    {
        Debug.Log($"SpawnSpacePods called on {gameObject.name}. Number to spawn: {numberOfSpacePods}");
        
        if (spacePodPrefabs == null || spacePodPrefabs.Length == 0)
        {
            Debug.LogError("No space pod prefabs assigned!");
            return;
        }
        
        for (int i = 0; i < numberOfSpacePods; i++)
        {
            GameObject prefab = spacePodPrefabs[Random.Range(0, spacePodPrefabs.Length)];

            // Spawn space pods within the game tile bounds (7.5 x 10 x 10)
            Vector3 localPos = new Vector3(
                Random.Range(-3.75f, 3.75f), // Within tile width (7.5/2 = 3.75)
                0f, // Keep at tile center (world y = 5)
                Random.Range(-5f, 5f) // Within tile depth (10/2 = 5)
            );

            // Instantiate prefab using its original scale
            GameObject spacePod = Instantiate(prefab, transform.position + localPos, Random.rotation, transform);

            // Apply uniform scale to maintain sphere shape
            spacePod.transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
            
            // Store the space pod data for animation (using localPosition since it's a child)
            spacePods.Add(new SpacePodData
            {
                gameObject = spacePod,
                startPosition = spacePod.transform.localPosition,
                timeOffset = Random.Range(0f, Mathf.PI * 2f) // Random starting phase
            });
            
            Debug.Log($"Spawned space pod {i+1}/{numberOfSpacePods} at position {spacePod.transform.position}");
        }
    }

    void OnDrawGizmosSelected()
    {
        Renderer rend = GetComponent<Renderer>();
        Vector3 drawSize = rend != null ? rend.bounds.size : Vector3.one;
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, drawSize);
    }
}
