using UnityEngine;
using System.Collections.Generic;

public class SceneController : MonoBehaviour
{
    // Prefab of game tile, used to create game tiles with Asteroids, SpacePods, etc.
    [SerializeField] private GameObject gameTilePrefab;

    // Main Camera of game, used to check if game tiles are off screen
    [SerializeField] private Camera mainCamera;

    // Speed at which tiles move backward
    [SerializeField] private float tileMoveSpeed = 0.5f;

    // List of all active tiles
    private List<GameObject> activeTiles = new List<GameObject>();
    
    // Current tile index, used to create new tiles
    private int currentTileIndex = 0;
    
    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
            
        CreateInitialTiles();
    }

    void Update()
    {
        MoveTiles();
        CheckForTileReplacement();
    }
    
    private void MoveTiles()
    {
        // Move all tiles backward (toward negative Z)
        float moveDistance = tileMoveSpeed * Time.deltaTime;
        
        foreach (GameObject tile in activeTiles)
        {
            if (tile != null)
                tile.transform.Translate(0, 0, -moveDistance);
        }
    }
    
    private void CreateInitialTiles()
    {
        // Create three tiles at Z positions 0, 10, 20
        activeTiles.Add(CreateTile(new Vector3(0, 5, 0f), 1));
        activeTiles.Add(CreateTile(new Vector3(0, 5, 10f), 2));
        activeTiles.Add(CreateTile(new Vector3(0, 5, 20f), 3));
    }
    
    private GameObject CreateTile(Vector3 position, int tileNumber)
    {
        GameObject tile = Instantiate(gameTilePrefab, position, Quaternion.identity);
        tile.name = $"GameTile_{tileNumber}";
        tile.transform.localScale = new Vector3(7.5f, 10f, 10f);
        SetTileVisibility(tile);
        return tile;
    }
    
    
    private void CheckForTileReplacement()
    {
        if (activeTiles.Count == 0) return;
        
        float cameraZ = mainCamera.transform.position.z;
        
        // Check if any tile has moved completely past the camera
        for (int i = activeTiles.Count - 1; i >= 0; i--)
        {
            if (activeTiles[i] == null) continue;
            
            float tileEndZ = activeTiles[i].transform.position.z + 5f; // Tile is 10 units deep, so +5f is the end
            
            // If tile is well past the camera, destroy it
            if (tileEndZ < cameraZ - 10f) // 10f buffer to ensure it's well past
            {
                Destroy(activeTiles[i]);
                activeTiles.RemoveAt(i);
            }
        }
        
        // Create new tile if we have less than 3 tiles
        if (activeTiles.Count < 3)
        {
            CreateNewTile();
        }
    }
    
    private void CreateNewTile()
    {
        // Find the rightmost tile position
        float rightmostZ = 0f;
        foreach (GameObject tile in activeTiles)
        {
            if (tile != null && tile.transform.position.z > rightmostZ)
            {
                rightmostZ = tile.transform.position.z;
            }
        }
        
        // Create new tile 10 units ahead of the rightmost tile
        currentTileIndex++;
        float newTileZ = rightmostZ + 10f;
        activeTiles.Add(CreateTile(new Vector3(0, 5, newTileZ), currentTileIndex));
    }
    
    
    private void SetTileVisibility(GameObject tile)
    {
        Renderer tileRenderer = tile.GetComponent<Renderer>();
        if (tileRenderer != null)
        {
            // Make tile semi-transparent so you can see it for debugging
            Color visibleColor = new Color(0.2f, 0.2f, 0.2f, 0.3f);
            tileRenderer.material.color = visibleColor;
            
            // This will make tiles visible so you can see the positioning
        }
    }
}
