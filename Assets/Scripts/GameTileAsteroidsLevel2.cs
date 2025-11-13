using UnityEngine;

public class GameTileAsteroidsLevel2 : MonoBehaviour
{
    [Header("Asteroid Settings - Level 2")]
    [SerializeField] private GameObject[] rockPrefabs; 
    [SerializeField] private int numberOfAsteroids = 5;
    [SerializeField] private Camera mainCamera;

    private Vector3 areaSize; // size of the tile in world space

    void Start()
    {
        // Find the main camera if not assigned
        if (mainCamera == null)
            mainCamera = Camera.main;
            
        // Get tile size, to create the asteroids within the tile bounds
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
            areaSize = rend.bounds.size;
        else
        {
            // we can check for a collider instead of a renderer, if no gametile is found
            Collider col = GetComponent<Collider>();
            areaSize = col != null ? col.bounds.size : Vector3.one;
        }
    }

    // Public function to spawn asteroids, called by SceneController
    public void SpawnAsteroids()
    {
        Debug.Log($"SpawnAsteroids (Level 2) called on {gameObject.name}. Number to spawn: {numberOfAsteroids}");
        
        if (rockPrefabs == null || rockPrefabs.Length == 0)
        {
            Debug.LogError("No rock prefabs assigned!");
            return;
        }
        
        for (int i = 0; i < numberOfAsteroids; i++)
        {
            GameObject prefab = rockPrefabs[Random.Range(0, rockPrefabs.Length)];

            // Spawn asteroids within the game tile bounds (7.5 x 10 x 10)
            // Level 2: Increased scale to 0.05 (from 0.02)
            float scale = 0.05f;
            Vector3 localPos = new Vector3(
                Random.Range(-3.75f, 3.75f), // Within tile width (7.5/2 = 3.75)
                0f, // Keep at tile center (world y = 5)
                Random.Range(-5f, 5f) // Within tile depth (10/2 = 5)
            );

            // Instantiate prefab as child of this tile
            GameObject asteroid = Instantiate(prefab, transform.position + localPos, Random.rotation, transform);

            // Scale asteroids - Level 2 has larger asteroids
            asteroid.transform.localScale = prefab.transform.localScale * scale;
            
            Debug.Log($"Spawned Level 2 asteroid {i+1}/{numberOfAsteroids} at position {asteroid.transform.position}");
        }
    }

    // Draw the tile bounds in the editor help with visuals
    void OnDrawGizmosSelected()
    {
        Renderer rend = GetComponent<Renderer>();
        Vector3 drawSize = rend != null ? rend.bounds.size : Vector3.one;
        Gizmos.color = Color.red; // Different color for Level 2
        Gizmos.DrawWireCube(transform.position, drawSize);
    }
}

