using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class OptionsUI : MonoBehaviour
{
    public static OptionsUI Instance { get; private set; }
    
    [Header("Audio Settings")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string masterVolumeParameter = "MasterVolume";
    [SerializeField] private string musicVolumeParameter = "MusicVoume";
    [SerializeField] private string sfxVolumeParameter = "SoundFXVolume";
    
    private VisualElement optionsMenu;
    private Slider masterSlider;
    private Slider musicSlider;
    private Slider sfxSlider;
    private Label scoreLabel;
    private bool isPaused = false;
    
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    
    void Start()
    {
        if (optionsMenu != null)
        {
            HideMenu();
            Time.timeScale = 1f;
        }
    }
    
    void OnEnable()
    {
        UIDocument uiDoc = GetComponent<UIDocument>();
        if (uiDoc == null) return;
        
        VisualElement root = uiDoc.rootVisualElement;
        if (root == null) return;
        
        optionsMenu = root.Q<VisualElement>("OptionsMenu");
        if (optionsMenu == null) return;
        
        masterSlider = root.Q<Slider>("MasterSlider");
        musicSlider = root.Q<Slider>("MusicSlider");
        sfxSlider = root.Q<Slider>("SFXSlider");
        scoreLabel = root.Q<Label>("Score");
        
        RegisterSlider(masterSlider, "MasterVolume", SetMasterVolume);
        RegisterSlider(musicSlider, "MusicVolume", SetMusicVolume);
        RegisterSlider(sfxSlider, "SFXVolume", SetSFXVolume);
        
        UpdateHighScore();
        HideMenu();
    }
    
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            TogglePause();
    }
    
    private void RegisterSlider(Slider slider, string prefsKey, System.Action<float> setter)
    {
        if (slider == null) return;
        
        float savedVolume = PlayerPrefs.GetFloat(prefsKey, 1f);
        slider.value = savedVolume * 100f;
        setter(savedVolume);
        slider.RegisterValueChangedCallback(evt => setter(evt.newValue / 100f));
    }
    
    public void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }
    
    public void PauseGame()
    {
        if (optionsMenu == null) return;
        
        isPaused = true;
        Time.timeScale = 0f;
        
        if (GameUI.Instance != null)
            GameUI.Instance.Hide();
        
        UpdateHighScore();
        ShowMenu();
    }
    
    public void ResumeGame()
    {
        if (optionsMenu == null) return;
        
        isPaused = false;
        Time.timeScale = 1f;
        
        if (GameUI.Instance != null)
            GameUI.Instance.Show();
        
        HideMenu();
    }
    
    private void ShowMenu()
    {
        optionsMenu.style.display = DisplayStyle.Flex;
        optionsMenu.style.visibility = Visibility.Visible;
        optionsMenu.style.opacity = 0f;
        StartCoroutine(FadeIn());
    }
    
    private void HideMenu()
    {
        optionsMenu.style.display = DisplayStyle.None;
        optionsMenu.style.visibility = Visibility.Hidden;
        isPaused = false;
    }
    
    private System.Collections.IEnumerator FadeIn()
    {
        float elapsed = 0f;
        float duration = 0.2f;
        
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            optionsMenu.style.opacity = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }
        
        optionsMenu.style.opacity = 1f;
    }
    
    private void SetMasterVolume(float volume)
    {
        SetVolume(masterVolumeParameter, volume, "MasterVolume");
    }
    
    private void SetMusicVolume(float volume)
    {
        SetVolume(musicVolumeParameter, volume, "MusicVolume");
    }
    
    private void SetSFXVolume(float volume)
    {
        SetVolume(sfxVolumeParameter, volume, "SFXVolume");
    }
    
    private void SetVolume(string parameter, float volume, string prefsKey)
    {
        if (audioMixer == null) return;
        
        float db = volume > 0.0001f ? Mathf.Log10(volume) * 20f : -80f;
        audioMixer.SetFloat(parameter, db);
        
        PlayerPrefs.SetFloat(prefsKey, volume);
        PlayerPrefs.Save();
    }
    
    private void UpdateHighScore()
    {
        if (scoreLabel != null)
        {
            int highScore = GameStateManager.GetHighestScore();
            scoreLabel.text = highScore.ToString("D5");
        }
    }
    
    public bool IsPaused() => isPaused;
}
