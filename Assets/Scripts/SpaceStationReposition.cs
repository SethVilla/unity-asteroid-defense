using UnityEngine;

public class SpaceStationReposition : MonoBehaviour
{
    [Header("Space Station Position Settings")]
    public float targetZPosition = -20f;
    public float moveSpeed = 0.5f;
    
    [Header("Rotation Settings")]
    public float rotationSpeed = 0.5f;
    public float maxRotationAngle = 1f; // Rotate between -1 and 1 degrees
    
    // Store the start position and rotation of the space station
    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool isMoving = false;
    private float rotationTime = 0f;
    
    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        isMoving = true;
        
        // Start rotation at 0
        rotationTime = 0f;
    }
    
    void Update()
    {
        if (isMoving)
        {
            // Move towards target Z position (backward movement only)
            Vector3 currentPosition = transform.position;
            currentPosition.z = Mathf.MoveTowards(currentPosition.z, targetZPosition, moveSpeed * Time.deltaTime);
            transform.position = currentPosition;
            
            // Apply slight rotation back and forth (oscillating between -1 and 1 degrees)
            rotationTime += Time.deltaTime * rotationSpeed;
            float rotationAngle = Mathf.Sin(rotationTime) * maxRotationAngle;
            
            // Apply rotation on the Y-axis (yaw)
            transform.rotation = startRotation * Quaternion.Euler(0f, rotationAngle, 0f);
            
            // Check if we've reached the target position or moved too far
            if (currentPosition.z <= targetZPosition)
            {
                isMoving = false;
                Destroy(gameObject);
            }
        }
    }
}

