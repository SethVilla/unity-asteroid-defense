using UnityEngine;
using UnityEngine.UIElements;

public class GameUI : MonoBehaviour {
  public static GameUI Instance { get; private set; }
  
  private Label score_;
  private Label gameOver_;
  private VisualElement health_;
  private VisualElement[] lives_;
  private int currentScore_ = 0;
  private int currentLives_ = 2;
  private VisualElement root_;

  void Awake() {
    // Singleton pattern for the GameUI
    if (Instance == null) {
      Instance = this;
    } else {
      Destroy(gameObject);
    }
  }

  public void OnEnable() {
    root_ = GetComponent<UIDocument>().rootVisualElement;
    score_ = root_.Q<Label>("Score");
    gameOver_ = root_.Q<Label>("GameOver");
    health_ = root_.Q<VisualElement>("HitPoints");
    lives_ = new VisualElement[3];
    lives_[0] = root_.Q<VisualElement>("Life1");
    lives_[1] = root_.Q<VisualElement>("Life2");
    lives_[2] = root_.Q<VisualElement>("Life3");
    
    // Initialize score display
    SetScore(currentScore_);
    
    // Initialize lives display
    UpdateLives();
  }

  public void SetScore(int score) {
    currentScore_ = score;
    if (score_ != null) {
      score_.text = currentScore_.ToString("D5");
    }
  }

  public void IncreaseScore(int points) {
    SetScore(currentScore_ + points);
    Debug.Log("Score increased by " + points + ". Total score: " + currentScore_);
  }

  public void UpdateHP(float currentHP, float maxHP) {
    if (health_ != null) {
      // Calculate HP percentage and set width (Max HP is 50%)
      float hpPercentage = (currentHP / maxHP) * 100f;
      health_.style.width = Length.Percent(hpPercentage / 2f); 
    }
  }

  public int getLives() {
    return currentLives_;
  }
  
  public int getScore() {
    return currentScore_;
  }

  public void SetLives(int lives) {
    // for future scenerio with live up power ups
    currentLives_ = Mathf.Clamp(lives, 0, 3);
    UpdateLives();
  }

  // Lose a life by decrementing the current lives
  public void LoseLife() {
    if (currentLives_ > 0) {
      currentLives_--;
      UpdateLives();
      Debug.Log("Life lost! Remaining lives: " + currentLives_);
    }
  }

  // Update the lives display based on the current lives
  private void UpdateLives() {
    for (int i = 0; i < lives_.Length; i++) {
      if (lives_[i] != null) {
        lives_[i].style.display = (i < currentLives_) ? DisplayStyle.Flex : DisplayStyle.None;
      }
    }
  }

  // Show the game over screen
  public void ShowGameOver() {
    gameOver_.style.visibility = Visibility.Visible;
    
    // Save high score when game ends
    if (currentScore_ > GameStateManager.GetHighestScore()) {
      GameStateManager.SaveHighestScore(currentScore_);
    }
    
    // Wait 3 seconds then return to level 1
    Invoke(nameof(ReturnToLevel1AfterDelay), 3f);
  }
  
  private void ReturnToLevel1AfterDelay() {
    if (LevelManager.Instance != null) {
      LevelManager.Instance.ReturnToLevel1AfterGameOver();
    }
  }

  // Hide the entire GameUI (used when pausing)
  public void Hide() {
    if (root_ != null) {
      root_.style.display = DisplayStyle.None;
    }
  }

  // Show the entire GameUI (used when resuming)
  public void Show() {
    if (root_ != null) {
      root_.style.display = DisplayStyle.Flex;
    }
  }

}
