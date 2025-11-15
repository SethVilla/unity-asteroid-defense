using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class StartScreenGameUI : MonoBehaviour
{
    public static StartScreenGameUI Instance { get; private set; }
    
    private Button startButton_;
    private Label highScoreLabel;
    private VisualElement titleElement;
    private VisualElement scoreContainer;
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
        titleElement = root.Q<VisualElement>("Title");
        scoreContainer = root.Q<VisualElement>("ScoreContainer");
        
        Debug.Log($"StartButton found: {startButton_ != null}, ScoreContainer found: {scoreContainer != null}, Title found: {titleElement != null}");
        
        if (startButton_ != null)
        {
            startButton_.clicked += OnStartButtonClicked;
        }
        else
        {
            Debug.LogWarning("StartButton not found! Make sure there's a Button with name 'StartButton' in the UXML");
        }
        
        // Initially show title, hide score container and start button
        if (titleElement != null)
        {
            titleElement.style.display = DisplayStyle.Flex;
            titleElement.style.visibility = Visibility.Visible;
        }
        
        if (scoreContainer != null)
        {
            scoreContainer.style.display = DisplayStyle.None;
            scoreContainer.style.visibility = Visibility.Hidden;
        }
        
        if (startButton_ != null)
        {
            startButton_.style.display = DisplayStyle.None;
            startButton_.style.visibility = Visibility.Hidden;
        }
        
        UpdateHighScore();
        
        // After 5 seconds, show score container and start button, hide title
        Invoke(nameof(ShowStartUI), 5f);
    }
    
    private void ShowStartUI()
    {
        if (titleElement != null)
        {
            titleElement.style.display = DisplayStyle.None;
            titleElement.style.visibility = Visibility.Hidden;
        }
        
        if (scoreContainer != null)
        {
            scoreContainer.style.display = DisplayStyle.Flex;
            scoreContainer.style.visibility = Visibility.Visible;
        }
        
        if (startButton_ != null)
        {
            startButton_.style.display = DisplayStyle.Flex;
            startButton_.style.visibility = Visibility.Visible;
            Debug.Log("Start button is now visible!");
        }
    }

    void OnDisable()
    {
        CancelInvoke(nameof(ShowStartUI));
        
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


