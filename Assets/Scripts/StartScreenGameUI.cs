using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class StartScreenGameUI : MonoBehaviour
{
    public static StartScreenGameUI Instance { get; private set; }
    
    private Button startButton_;
    private Label highScoreLabel;
    private const string GAME_SCENE_NAME = "Level One";

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
        
        startButton_ = root.Q<Button>("StartButton");
        highScoreLabel = root.Q<Label>("Score");
        
        if (startButton_ != null)
        {
            startButton_.clicked += OnStartButtonClicked;
        }
        
        UpdateHighScore();
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
        GameStateManager.ResetState();
        
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.ResetToLevel1();
        }
        
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(GAME_SCENE_NAME);
    }
    
    private void UpdateHighScore()
    {
        int highScore = GameStateManager.GetHighestScore();
        Debug.Log($"Loading high score: {highScore}");
        
        if (highScoreLabel != null)
        {
            highScoreLabel.text = highScore.ToString("D5");
            Debug.Log($"High score label updated to: {highScoreLabel.text}");
        }
        else
        {
            Debug.LogWarning("HighScore label not found in UI! Make sure there's a Label with name 'HighScore' in the UXML");
        }
    }
    
    public void LoadGameScene()
    {
        OnStartButtonClicked();
    }
}


