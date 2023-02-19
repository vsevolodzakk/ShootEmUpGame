using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _gameOverScreen;
    
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private Slider _loadingSlider;

    [SerializeField] private PlayerController _player;
    [SerializeField] private HealthComponent _playerHealth;

    [SerializeField] private AudioSource _mainMenuMusic;
    [SerializeField] private AudioSource _gameOverMusic;
    [SerializeField] private AudioSource _pauseSound;

    private PlayerInputActions _input;

    private Scene _scene;
    
    private bool _gameOnPause = false;

    public bool GameOnPause => _gameOnPause;

    private void Awake()
    {
        _input= new PlayerInputActions();
    }

    private void OnEnable()
    {
        _scene = SceneManager.GetActiveScene();

        _input.Player.Pause.Enable();
        _input.Player.Pause.performed += Pause;

        #region Clear HiScore for dev 

        _input.Player.ClearHiScore.Enable();
        _input.Player.ClearHiScore.performed += ClearHiScore;

        #endregion

        // If MainMenuScene is active play music
        if (_scene.buildIndex == 0)
            _mainMenuMusic.Play();

        if(_scene.buildIndex == 1)
            PlayerController.OnPlayerDeath += GameOverMenu; // Subscribe to OnPlayerDeath event in Gameplay Scene

        Debug.Log(Screen.currentResolution); // Debug Info
    }

    private void ClearHiScore(InputAction.CallbackContext context)
    {
        if(_scene.buildIndex == 1)
        {
            RestartLevel();
            PlayerPrefs.SetInt("HiScore", 0);
            Debug.Log("RECORD CLEAR!");
        }
    }

    /// <summary>
    /// Restart Gameplay Scene
    /// </summary>
    public void RestartLevel()
    {
        SceneManager.LoadScene(1);
        NormalMode();
    }

    /// <summary>
    /// Load Main Menu scene
    /// </summary>
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Quit from Game
    /// </summary>
    public void QuitFromGame()
    {
        //SavePlayerDataToFile();
        Application.Quit();
    }

    /// <summary>
    /// Set Game of Pause
    /// </summary>
    public void Pause(InputAction.CallbackContext context)
    {
        _pauseSound.Play();
        if (!_gameOnPause && _playerHealth.IsAlive)
        {
            _pauseMenu.SetActive(true);
            SlowMode();
        } 
        else
        {
            Resume();
        }

    }


    /// <summary>
    /// Set Game to play state
    /// </summary>
    public void Resume()
    {
        _pauseMenu.SetActive(false);
        NormalMode();
    }

    /// <summary>
    /// Set Pause state and GameOver screen
    /// </summary>
    public void GameOverMenu()
    {
        _gameOverScreen.SetActive(true);
        _gameOverMusic.Play();
        SlowMode();
    }

    /// <summary>
    /// Set Normal speed of the Game
    /// </summary>
    private void NormalMode()
    {
        Cursor.visible = false;
        _gameOnPause = false;
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Set Slow speed of the Game
    /// </summary>
    private void SlowMode()
    {
        Cursor.visible = true;
        _gameOnPause = true;
        Time.timeScale = 0.1f;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerDeath -= GameOverMenu;

        _input.Player.Pause.performed -= Pause;
        _input.Player.Pause.Disable();

        #region Clear HiScore for dev 
        _input.Player.ClearHiScore.Enable();
        _input.Player.ClearHiScore.performed += ClearHiScore;
        #endregion
    }

    /// <summary>
    /// Load Gameplay scene
    /// </summary>
    /// <param name="scneneIndex"></param>
    public void LoadLevel(int scneneIndex)
    {
        StartCoroutine(LoadInBackground(scneneIndex));
        NormalMode();
    }

    /// <summary>
    /// Async load of the scene
    /// </summary>
    /// <param name="sceneIndex">Scene ot load</param>
    /// <returns></returns>
    IEnumerator LoadInBackground(int sceneIndex)
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(sceneIndex);

        _loadingScreen.SetActive(true);

        while (!load.isDone)
        {
            float progress = Mathf.Clamp01(load.progress / .9f);
            _loadingSlider.value = progress;

            Debug.Log(progress.ToString());

            yield return null;
        }
    }

    // Trying to save player data to file
    private void SavePlayerDataToFile()
    {
        string path = @"Assets\Resources\SomeFile.txt";

        if (!File.Exists(path))
        {
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine(PlayerPrefs.GetInt("HiScore"));
                sw.WriteLine(PlayerPrefs.GetFloat("FxVol"));
                sw.WriteLine(PlayerPrefs.GetFloat("MusicVol"));
            }
        }
        else
        {
            File.WriteAllText(path, string.Empty);

            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine(PlayerPrefs.GetInt("HiScore"));
                sw.WriteLine(PlayerPrefs.GetFloat("FxVol"));
                sw.WriteLine(PlayerPrefs.GetFloat("MusicVol"));
            }
        }
    }
}
