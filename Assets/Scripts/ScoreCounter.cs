using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour
{
    [SerializeField] private Text _text;

    [SerializeField] private Text _hiScoreText;
    [SerializeField] private Animator _hiScoreTextAnimation;

    private int _killCount;
    private int _hiScore;

    private void OnEnable()
    {
        EnemyControllerPooled.OnEnemyDies += ScoreKill;
        PlayerController.OnPlayerDeath += CheckHiScore;
    }

    private void Start()
    {
        _killCount = 0;
        LoadHiScore();
    }

    private void ScoreKill(int score)
    {
        _killCount += score;

        if (_killCount > 9999)
            _killCount -= 9999;

        _text.text = _killCount.ToString();
    }

    private void OnDisable()
    {
        EnemyControllerPooled.OnEnemyDies -= ScoreKill;
        PlayerController.OnPlayerDeath -= CheckHiScore;
    }

    private void SaveHiScore()
    {
        _hiScoreText.color = Color.red;
        _hiScoreText.text = "New Hi-Score: " + _killCount.ToString();
        _hiScoreTextAnimation.SetTrigger("hiscore");
        PlayerPrefs.SetInt("HiScore", _killCount);
    }

    private void LoadHiScore()
    {
        _hiScore = PlayerPrefs.GetInt("HiScore");
        _hiScoreText.text = "Hi-Score: " + _hiScore.ToString();
        _hiScoreText.color = Color.yellow;
    }

    private void CheckHiScore()
    {
        if (_killCount > _hiScore)
            SaveHiScore();
    }
}
