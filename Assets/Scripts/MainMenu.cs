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
        _master.SetFloat("MusicVol", PlayerPrefs.GetFloat("MusicVol"));
        _master.SetFloat("FxVol", PlayerPrefs.GetFloat("FxVol"));

        _hiScore = PlayerPrefs.GetInt("HiScore");
        _hiScoreText.text = "Hi-Score: " + _hiScore.ToString();
    }

    public void PlayGame()
    {
        // Begin Play
        Debug.Log("Starting game.");
    }

    public void QuitGame()
    {
        // Quit program
        Debug.Log("Quit game.");
        Application.Quit();
    }
}
