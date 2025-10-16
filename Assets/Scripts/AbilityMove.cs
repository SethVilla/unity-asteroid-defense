using UnityEngine;

public class AbilityMove : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    
    // Extra distance before destroying object
    private Camera mainCamera;
    private float boundsBuffer = 5f;
    
    private void Start()
    {
        mainCamera = Camera.main;
    }
    
    private void Update()
    {
        // Move only in Z direction
        Vector3 movement = new Vector3(0, 0, moveSpeed * Time.deltaTime);
        transform.Translate(movement, Space.World);
        
        // Check bounds and destroy if out of camera frame
        CheckBounds();
    }
    
    private void CheckBounds()
    {   
        if (mainCamera != null) {

            // Get camera bounds based on the camera's position, size, and aspect ratio
            float cameraZ = mainCamera.transform.position.z;
            float cameraSize = mainCamera.orthographicSize;
            float aspectRatio = (float)Screen.width / Screen.height;
            
            // Calculate world bounds with buffer
            float leftBound = -cameraSize * aspectRatio - boundsBuffer;
            float rightBound = cameraSize * aspectRatio + boundsBuffer;
            float topBound = cameraSize + boundsBuffer;
            float bottomBound = -cameraSize - boundsBuffer;

            // Buffer
            float frontBound = cameraZ  + boundsBuffer;
            float backBound = cameraZ  - boundsBuffer;
            
            Vector3 pos = transform.position;
            
            // Destroy if outside camera bounds
            if (pos.x < leftBound || pos.x > rightBound || 
                pos.y < bottomBound || pos.y > topBound ||
                pos.z < backBound || pos.z > frontBound)
            {
                Destroy(gameObject);
            }
        }
    }

}
