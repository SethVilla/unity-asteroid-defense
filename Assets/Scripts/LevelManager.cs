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
    [SerializeField] private int level2ScoreThreshold = 500;
    [SerializeField] private int bossBattleScoreThreshold = 1000;
    
    [Header("Transition Settings")]
    [SerializeField] private float transitionDelay = 2f;
    
    private int currentLevel = 1;
    private bool isTransitioning = false;
    
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
    
    void Update()
    {
        if (isTransitioning || GameUI.Instance == null) return;
        
        int currentScore = GameUI.Instance.getScore();
        
        if (currentLevel == 1 && currentScore >= level2ScoreThreshold)
        {
            StartLevelTransition(2);
        }
        else if (currentLevel == 2 && currentScore >= bossBattleScoreThreshold)
        {
            StartLevelTransition(3);
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
        GameStateManager.ResetState();
    }
}
