using UnityEngine;

public static class GameStateManager
{
    private static int savedLives = -1;
    private static int savedScore = -1;
    
    // Save the current game state (lives and score)
    public static void SaveState(int lives, int score)
    {
        savedLives = lives;
        savedScore = score;
        Debug.Log($"GameState saved - Lives: {savedLives}, Score: {savedScore}");
    }
    
    // Check if there is saved state available
    public static bool HasSavedState()
    {
        return savedLives >= 0 && savedScore >= 0;
    }
    
    // Get the saved lives count
    public static int GetSavedLives()
    {
        return savedLives;
    }
    

    // Get the saved score
    public static int GetSavedScore()
    {
        return savedScore;
    }
    
    // Clear the saved state after restoring
    public static void ClearSavedState()
    {
        savedLives = -1;
        savedScore = -1;
        Debug.Log("GameState cleared");
    }
    

    // Reset all game state (for new game)
    public static void ResetState()
    {
        savedLives = -1;
        savedScore = -1;
        Debug.Log("GameState reset for new game");
    }
}

