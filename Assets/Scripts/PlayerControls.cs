using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference primaryAbilityAction;
    [SerializeField] private InputActionReference secondaryAbilityAction;
    
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;

    [Header("Ability Settings")]
    [SerializeField] private GameObject abilityBulletPrefab;
    [SerializeField] private GameObject abilityMissilePrefab;
    [SerializeField] private Transform LeftWingHardPoint;
    [SerializeField] private Transform RightWingHardPoint;

    private float primaryDelay = 0.1f;
    private float secondaryDelay = 1.0f;
    private bool primaryAbilityDown_ = false;
    private bool secondaryAbilityDown_ = false;
    private float primaryDelayTimer = 0.0f;
    private float secondaryDelayTimer = 0.0f;

    private float maxX = 6.5f;
    private float minX = -.37f;
    private float maxZ = -1.5f;
    private float minZ = -7f;
    
    void Start()
    {
        // Enable the input action so the player can move
        if (moveAction != null)
        {
            moveAction.action.Enable();
        }
        
        // Enable primary ability action
        if (primaryAbilityAction != null)
        {
            primaryAbilityAction.action.Enable();
            OnEnablePrimaryAbility();
            primaryAbilityDown_ = false;
            primaryDelayTimer = 0.0f;
        }
        
        // Enable secondary ability action
        if (secondaryAbilityAction != null)
        {
            secondaryAbilityAction.action.Enable();
            OnEnableSecondaryAbility();
            secondaryAbilityDown_ = false;
            secondaryDelayTimer = 0.0f;
        }
    }
    
    void OnDestroy()
    {
        // Disable input actions when object is destroyed
        if (moveAction != null)
        {
            moveAction.action.Disable();
        }
        
        if (primaryAbilityAction != null)
        {
            OnDisablePrimaryAbility();
            primaryAbilityAction.action.Disable();
        }
        
        if (secondaryAbilityAction != null)
        {
            OnDisableSecondaryAbility();
            secondaryAbilityAction.action.Disable();
        }
    }
    
    void Update()
    {
        OnMovement();

        // Handle primary ability
        HandleAbility(primaryAbilityDown_, ref primaryDelayTimer, primaryDelay, abilityBulletPrefab, "bullet");

        // Handle secondary ability
        HandleAbility(secondaryAbilityDown_, ref secondaryDelayTimer, secondaryDelay, abilityMissilePrefab, "missile");
    }
    
    private void OnMovement()
    {
        // Get input from InputActionReference
        Vector2 moveInput = Vector2.zero;
        if (moveAction != null && moveAction.action.enabled)
        {
            moveInput = moveAction.action.ReadValue<Vector2>();
        }
        
        // Calculate movement direction from input
        // X axis: left/right (moveInput.x)
        // Z axis: forward/backward (moveInput.y)
        // Y axis: up/down (0f)
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y);
        
        // Apply direct transform movement
        transform.Translate(movement * moveSpeed * Time.deltaTime);
        
        // Apply movement bounds
        // similar to Max and Min values in other languages
        ClampWithinBounds();
    }
    
    
    private void ClampWithinBounds()
    {
        // Clamp position within bounds
        // get position of the player
        // clamp the x and z position within the bounds
        // set the position of the player
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);
        transform.position = pos;
    }

    private void HandleAbility(bool abilityDown, ref float delayTimer, float delay, GameObject prefab, string type)
    {
        if (abilityDown) {
            delayTimer += Time.deltaTime;
            if (delayTimer >= delay) {
                if (prefab != null) {
                    // Different rotations for bullets vs missiles
                    Quaternion rotation = (type == "bullet") ? Quaternion.Euler(100f, 0f, 0f) : Quaternion.Euler(0f, 0f, 0f);
                    
                    Instantiate(prefab, LeftWingHardPoint.position, rotation);
                    Instantiate(prefab, RightWingHardPoint.position, rotation);
                } else {
                    Debug.LogError($"Ability {type} prefab is null!");
                }
                // Reset timer for next shot
                delayTimer = 0.0f; 
            }
        } else {
            // Reset timer when not holding
            delayTimer = 0.0f; 
        }
    }


    private void OnPrimaryAbilityDown(InputAction.CallbackContext context) {
        Debug.Log("Primary ability key pressed!");
        primaryAbilityDown_ = true;
    }

    private void OnPrimaryAbilityUp(InputAction.CallbackContext context) {
        Debug.Log("Primary ability key released!");
        primaryAbilityDown_ = false;
    }

    private void OnEnablePrimaryAbility() {
        primaryAbilityAction.action.performed += OnPrimaryAbilityDown;
        primaryAbilityAction.action.canceled += OnPrimaryAbilityUp;
    }

    private void OnDisablePrimaryAbility() {
        primaryAbilityAction.action.performed -= OnPrimaryAbilityDown;
        primaryAbilityAction.action.canceled -= OnPrimaryAbilityUp;
    }

    private void OnSecondaryAbilityDown(InputAction.CallbackContext context) {
        secondaryAbilityDown_ = true;
    }

    private void OnSecondaryAbilityUp(InputAction.CallbackContext context) {
        secondaryAbilityDown_ = false;
    }

    private void OnEnableSecondaryAbility() {
        secondaryAbilityAction.action.performed += OnSecondaryAbilityDown;
        secondaryAbilityAction.action.canceled += OnSecondaryAbilityUp;
    }

    private void OnDisableSecondaryAbility() {
        secondaryAbilityAction.action.performed -= OnSecondaryAbilityDown;
        secondaryAbilityAction.action.canceled -= OnSecondaryAbilityUp;
    }
    


}

