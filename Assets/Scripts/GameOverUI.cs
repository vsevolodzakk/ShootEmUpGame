using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private Text _gameOverText;
    [SerializeField] private GameObject _gameOverScreen;

    private void OnEnable()
    {
        PlayerController.OnPlayerDeath += GameOverMessage;
    }
 
    private void GameOverMessage()
    {
        _gameOverScreen.SetActive(true);
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerDeath -= GameOverMessage;
    }
}
