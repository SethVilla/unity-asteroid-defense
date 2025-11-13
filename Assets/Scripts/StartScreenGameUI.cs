using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class StartScreenGameUI : MonoBehaviour
{
    public static StartScreenGameUI Instance { get; private set; }
    
    [Header("Scene Settings")]
    [SerializeField] private string gameSceneName = "Assignment7Scene";
    
    private Button startButton_;

    void Awake()
    {
        // Singleton pattern for the StartScreenGameUI
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        
        // Find the start button
        startButton_ = root.Q<Button>("StartButton");
        
        if (startButton_ != null)
        {
            // Register click event
            startButton_.clicked += OnStartButtonClicked;
            Debug.Log("Start button found and registered");
        }
        else
        {
            Debug.LogError("Start button not found! Make sure there's a Button element with name 'StartButton' in the UI Document");
        }
    }

    void OnDisable()
    {
        // Unregister event to prevent memory leaks
        if (startButton_ != null)
        {
            startButton_.clicked -= OnStartButtonClicked;
        }
    }

    private void OnStartButtonClicked()
    {
        Debug.Log($"Start button clicked! Loading scene: {gameSceneName}");
        
        // Reset game state for a new game
        GameStateManager.ResetState();
        
        // Reset time scale in case it was paused
        Time.timeScale = 1.0f;
        
        // Load the game scene
        SceneManager.LoadScene(gameSceneName);
    }
    
    /// <summary>
    /// Public method to load the game scene (can be called from other scripts)
    /// </summary>
    public void LoadGameScene()
    {
        OnStartButtonClicked();
    }
}

