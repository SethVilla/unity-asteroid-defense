using UnityEngine;
using System.Collections.Generic;

public class GameTileAlienFighters : MonoBehaviour
{
    [Header("Alien Fighter Settings")]
    [SerializeField] private GameObject[] alienFighterPrefabs; 
    [SerializeField] private int numberOfFighters = 2;
    [SerializeField] private Camera mainCamera;
    
    [Header("Movement Settings")]
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float diagonalSpeed = 0.5f;
    [SerializeField] private float diagonalRange = 0.3f;

    private Vector3 areaSize;
    private List<FighterData> fighters = new List<FighterData>();
    
    private class FighterData
    {
        public GameObject gameObject;
        public Vector3 startPosition;
        public float timeOffset;
    }

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
            
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
        // Update each alien fighter's left-right oscillation
        foreach (FighterData fighter in fighters)
        {
            if (fighter.gameObject != null)
            {
                // Apply left-to-right oscillation (using localPosition since it's a child)
                float time = Time.time * diagonalSpeed + fighter.timeOffset;
                Vector3 offset = new Vector3(
                    Mathf.Sin(time) * diagonalRange,
                    0f,
                    0f
                );
                
                fighter.gameObject.transform.localPosition = fighter.startPosition + offset;
            }
        }
    }
    
    public void SpawnAlienFighters()
    {
        Debug.Log($"SpawnAlienFighters called on {gameObject.name}. Number to spawn: {numberOfFighters}");
        
        if (alienFighterPrefabs == null || alienFighterPrefabs.Length == 0)
        {
            Debug.LogError("No alien fighter prefabs assigned!");
            return;
        }
        
        for (int i = 0; i < numberOfFighters; i++)
        {
            GameObject prefab = alienFighterPrefabs[Random.Range(0, alienFighterPrefabs.Length)];

            // Spawn within game tile bounds
            Vector3 localPos = new Vector3(
                Random.Range(-0.3f, 0.3f), // Constrained X range
                0f, // Keep at tile center (world y = 5)
                Random.Range(-1.5f, .05f) // Constrained Z range to keep on tile
            );

            // Instantiate prefab with fixed rotation (as child of tile, will be destroyed with tile)
            Quaternion rotation = Quaternion.Euler(40f, 0f, 0f);
            GameObject fighter = Instantiate(prefab, transform.position + localPos, rotation, transform);

            // Apply scale (similar to asteroids)
            float scale = 0.03f;
            fighter.transform.localScale = prefab.transform.localScale * scale;
            
            // Set the local position explicitly to ensure proper bounds
            fighter.transform.localPosition = localPos;
            
            // Ensure correct tag
            if (!fighter.CompareTag("Alien Fighter"))
            {
                fighter.tag = "Alien Fighter";
                Debug.Log($"Set alien fighter tag to 'Alien Fighter'");
            }
            
            // Check for required components
            if (fighter.GetComponent<Collider>() == null)
            {
                Debug.LogWarning($"Alien Fighter prefab '{prefab.name}' is missing a Collider component!");
            }
            
            if (fighter.GetComponent<Rigidbody>() == null)
            {
                Debug.LogWarning($"Alien Fighter prefab '{prefab.name}' is missing a Rigidbody component!");
            }
            
            if (fighter.GetComponent<AlienFighterCollision>() == null)
            {
                Debug.LogWarning($"Alien Fighter prefab '{prefab.name}' is missing AlienFighterCollision script!");
            }
            
            // Store fighter data using the actual local position from the transform
            fighters.Add(new FighterData
            {
                gameObject = fighter,
                startPosition = fighter.transform.localPosition,
                timeOffset = Random.Range(0f, 10f)
            });
            
            Debug.Log($"Spawned alien fighter {i+1}/{numberOfFighters} at world position {fighter.transform.position}, local position {fighter.transform.localPosition}");
        }
    }

    void OnDrawGizmosSelected()
    {
        Renderer rend = GetComponent<Renderer>();
        Vector3 drawSize = rend != null ? rend.bounds.size : Vector3.one;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, drawSize);
    }
}

