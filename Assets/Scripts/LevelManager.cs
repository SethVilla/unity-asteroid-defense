using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }
    
    [Header("Level Settings")]
    [SerializeField] private string level1SceneName = "Level One";
    [SerializeField] private string level2SceneName = "Level Two";
    [SerializeField] private string bossBattleSceneName = "Boss Battle";
    
    [Header("Progression Thresholds")]
    [SerializeField] private int level2ScoreThreshold = 5000;
    [SerializeField] private int bossBattleScoreThreshold = 10000;
    
    [Header("Transition Settings")]
    [SerializeField] private float transitionDelay = 2f;
    
    [Header("Boss Battle Settings")]
    [SerializeField] private int alienDestroyersRequired = 3;
    
    private int currentLevel = 1;
    private bool isTransitioning = false;
    private int alienDestroyersDestroyed = 0;
    private int levelStartScore = 0; // Score when entering current level
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        // Set current level based on active scene
        // Delay slightly to ensure GameUI is initialized
        Invoke(nameof(UpdateCurrentLevelFromScene), 0.1f);
    }
    
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Delay slightly to ensure GameUI is initialized
        Invoke(nameof(UpdateCurrentLevelFromScene), 0.1f);
    }
    
    private void UpdateCurrentLevelFromScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        
        if (currentSceneName == level1SceneName)
        {
            currentLevel = 1;
            // Set level start score when entering level 1
            if (GameUI.Instance != null)
                levelStartScore = GameUI.Instance.getScore();
            Debug.Log($"LevelManager: Set to Level 1, starting score: {levelStartScore}");
        }
        else if (currentSceneName == level2SceneName)
        {
            currentLevel = 2;
            // Set level start score when entering level 2
            if (GameUI.Instance != null)
                levelStartScore = GameUI.Instance.getScore();
            Debug.Log($"LevelManager: Set to Level 2, starting score: {levelStartScore}");
        }
        else if (currentSceneName == bossBattleSceneName)
        {
            currentLevel = 3;
            // Reset destroyer count when entering boss battle
            int previousCount = alienDestroyersDestroyed;
            alienDestroyersDestroyed = 0;
            // Set level start score when entering boss battle
            if (GameUI.Instance != null)
                levelStartScore = GameUI.Instance.getScore();
            Debug.Log($"LevelManager: Set to Level 3 (Boss Battle), starting score: {levelStartScore}, destroyer count reset from {previousCount} to 0");
        }
    }
    
    void Update()
    {
        if (isTransitioning || GameUI.Instance == null) return;
        
        int currentScore = GameUI.Instance.getScore();
        
        // Check thresholds relative to level start score
        if (currentLevel == 1 && currentScore >= levelStartScore + level2ScoreThreshold)
        {
            Debug.Log($"Level 2 threshold reached! Score: {currentScore}, Start: {levelStartScore}, Threshold: {level2ScoreThreshold}");
            StartLevelTransition(2);
        }
        else if (currentLevel == 2 && currentScore >= levelStartScore + bossBattleScoreThreshold)
        {
            Debug.Log($"Boss Battle threshold reached! Score: {currentScore}, Start: {levelStartScore}, Threshold: {bossBattleScoreThreshold}");
            StartLevelTransition(3);
        }
        else if (currentLevel == 3)
        {
            if (alienDestroyersDestroyed >= alienDestroyersRequired)
            {
                Debug.Log($"Update: Boss battle completion detected! Level: {currentLevel}, Destroyers: {alienDestroyersDestroyed}/{alienDestroyersRequired}");
                CompleteBossBattle();
            }
        }
    }
    
    private void StartLevelTransition(int nextLevel)
    {
        if (isTransitioning) return;
        
        isTransitioning = true;
        currentLevel = nextLevel;
        Invoke(nameof(LoadNextLevel), transitionDelay);
    }
    
    private void LoadNextLevel()
    {
        if (GameUI.Instance != null)
        {
            GameStateManager.SaveState(GameUI.Instance.getLives(), GameUI.Instance.getScore());
        }
        
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(GetSceneNameForLevel(currentLevel));
        isTransitioning = false;
    }
    
    private string GetSceneNameForLevel(int level)
    {
        switch (level)
        {
            case 1: return level1SceneName;
            case 2: return level2SceneName;
            case 3: return bossBattleSceneName;
            default: return level1SceneName;
        }
    }
    
    public int GetCurrentLevel() => currentLevel;
    
    public void SetLevel(int level) => currentLevel = level;
    
    public void ResetToLevel1()
    {
        currentLevel = 1;
        isTransitioning = false;
        levelStartScore = 0;
        GameStateManager.ResetState();
    }
    
    public void ReturnToLevel1AfterGameOver()
    {
        Debug.Log("Game Over - Returning to Level 1");
        currentLevel = 1;
        alienDestroyersDestroyed = 0;
        levelStartScore = 0;
        isTransitioning = false;
        GameStateManager.ResetState();
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(level1SceneName);
    }
    
    public void OnLifeLost()
    {
        // Reset destroyer count if in boss battle when life is lost
        if (currentLevel == 3)
        {
            alienDestroyersDestroyed = 0;
            Debug.Log("Life lost in Boss Battle - Reset destroyer count to 0");
        }
    }
    
    public void OnAlienDestroyerDestroyed()
    {
        Debug.Log($"OnAlienDestroyerDestroyed called. Current Level: {currentLevel}, Current Count: {alienDestroyersDestroyed}");
        
        if (currentLevel == 3)
        {
            alienDestroyersDestroyed++;
            Debug.Log($"Alien Destroyer destroyed! Count incremented to: {alienDestroyersDestroyed}/{alienDestroyersRequired}");
            
            if (alienDestroyersDestroyed >= alienDestroyersRequired)
            {
                Debug.Log($"Boss battle completion triggered! Final count: {alienDestroyersDestroyed}/{alienDestroyersRequired}");
            }
            else
            {
                Debug.Log($"Still need {alienDestroyersRequired - alienDestroyersDestroyed} more destroyers");
            }
        }
        else
        {
            Debug.LogWarning($"Alien Destroyer destroyed but not in Boss Battle (current level: {currentLevel})");
        }
    }
    
    private void CompleteBossBattle()
    {
        if (isTransitioning)
        {
            Debug.Log("CompleteBossBattle called but already transitioning - ignoring");
            return;
        }
        
        isTransitioning = true;
        Debug.Log($"Boss Battle Complete! Destroyers defeated: {alienDestroyersDestroyed}. Returning to Level 1 with score preserved.");
        
        // Save current state (lives and score) to preserve for Level 1
        if (GameUI.Instance != null)
        {
            int currentScore = GameUI.Instance.getScore();
            int currentLives = GameUI.Instance.getLives();
            
            // Update high score if current score is higher
            if (currentScore > GameStateManager.GetHighestScore())
            {
                GameStateManager.SaveHighestScore(currentScore);
            }
            
            // Save state to continue with current score and lives
            GameStateManager.SaveState(currentLives, currentScore);
            Debug.Log($"Saved state: Lives={currentLives}, Score={currentScore}");
        }
        
        // Reset level and destroyer count
        currentLevel = 1;
        alienDestroyersDestroyed = 0;
        
        // Load Level 1
        Invoke(nameof(LoadLevel1), transitionDelay);
    }
    
    private void LoadLevel1()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(level1SceneName);
        isTransitioning = false;
    }
}
