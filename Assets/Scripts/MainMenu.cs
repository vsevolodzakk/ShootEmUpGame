using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Slider _musicControl;
    [SerializeField] private AudioMixer _master;

    [SerializeField] private Text _hiScoreText;
    private int _hiScore;

    private void Start()
    {
        // Set saved user Settings
        _master.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVol"));
        _master.SetFloat("FxVol", PlayerPrefs.GetFloat("FxVol"));

        // Get Hi-Score
        _hiScore = PlayerPrefs.GetInt("HiScore");
        _hiScoreText.text = "Hi-Score: " + _hiScore.ToString();
    }
}
