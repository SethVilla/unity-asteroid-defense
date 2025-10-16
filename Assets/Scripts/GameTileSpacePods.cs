using UnityEngine;

public class GameTileSpacePods : MonoBehaviour
{
    [Header("Space Pod Settings")]
    [SerializeField] private GameObject[] spacePodPrefabs; 
    [SerializeField] private int numberOfSpacePods = 3;
    [SerializeField] private Camera mainCamera;

    private Vector3 areaSize; // size of the tile in world space

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

        SpawnSpacePods();
    }

    void SpawnSpacePods()
    {
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
