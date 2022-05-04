﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _gameOverScreen;
    
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private Slider _loadingSlider;

    [SerializeField] private PlayerController _player;

    [SerializeField] private AudioSource _mainMenuMusic;
    [SerializeField] private AudioSource _gameOverMusic;
    [SerializeField] private AudioSource _pauseSound;

    private Scene _scene;
    
    public bool gameOnPause = false;
    

    private void OnEnable()
    {
        _scene = SceneManager.GetActiveScene();
        if (_scene.buildIndex == 0)
            _mainMenuMusic.Play();

        if(_scene.buildIndex == 1)
            PlayerController.onPlayerDeath += GameOverMenu;

        Debug.Log(Screen.currentResolution);
    }

    void Update()
    {
        if (_scene.buildIndex == 1)
        {
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                RestartLevel();
                PlayerPrefs.SetInt("HiScore", 0);
                Debug.Log("RECORD CLEAR!");
            }

            if (Input.GetKeyDown(KeyCode.Escape) && gameOnPause == false && _player.isAlive)
                Pause();
            else if (Input.GetKeyDown(KeyCode.Escape) && gameOnPause && _player.isAlive)
                Resume();
        }

    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(1);
        NormalMode();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void QuitFromGame()
    {
        Application.Quit();
    }
    public void Pause()
    {
        _pauseMenu.SetActive(true);
        _pauseSound.Play();
        SlowMode();
    }
    public void Resume()
    {
        _pauseMenu.SetActive(false);
        _pauseSound.Play();
        NormalMode();
    }

    public void GameOverMenu()
    {
        _gameOverScreen.SetActive(true);
        _gameOverMusic.Play();
        SlowMode();
    }
    private void NormalMode()
    {
        Cursor.visible = false;
        gameOnPause = false;
        Time.timeScale = 1f;
    }
    private void SlowMode()
    {
        Cursor.visible = true;
        gameOnPause = true;
        Time.timeScale = 0.1f;
    }

    private void OnDisable()
    {
        PlayerController.onPlayerDeath -= GameOverMenu;
    }

    public void LoadLevel(int scneneIndex)
    {
        StartCoroutine(LoadInBAckbround(scneneIndex));
        NormalMode();
    }

    IEnumerator LoadInBAckbround(int scheneIndex)
    {
        AsyncOperation load = SceneManager.LoadSceneAsync(scheneIndex);

        _loadingScreen.SetActive(true);

        while (!load.isDone)
        {
            float progress = Mathf.Clamp01(load.progress / .9f);
            _loadingSlider.value = progress;

            Debug.Log(progress.ToString());

            yield return null;
        }
    }
}