using UnityEngine;

public static class GameStateManager
{
    private static int savedLives = -1;
    private static int savedScore = -1;
    private static int highestScore = -1;
    
    public static void SaveState(int lives, int score)
    {
        savedLives = lives;
        savedScore = score;
        
        if (score > GetHighestScore())
            SaveHighestScore(score);
    }
    
    public static bool HasSavedState()
    {
        return savedLives >= 0 && savedScore >= 0;
    }
    
    public static int GetSavedLives()
    {
        return savedLives;
    }

    public static int GetSavedScore()
    {
        return savedScore;
    }
    
    public static void SaveHighestScore(int score)
    {
        highestScore = score;
        PlayerPrefs.SetInt("HighestScore", score);
        PlayerPrefs.Save();
    }
    
    public static int GetHighestScore()
    {
        if (highestScore == -1)
            highestScore = PlayerPrefs.GetInt("HighestScore", 0);
        
        return highestScore;
    }
    
    public static void ClearSavedState()
    {
        savedLives = -1;
        savedScore = -1;
    }

    public static void ResetState()
    {
        savedLives = -1;
        savedScore = -1;
    }
}
