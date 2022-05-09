using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private Slider _musicControl;
    [SerializeField] private Slider _soundFxControl;

    [SerializeField] private AudioMixer _master;


    private void Awake()
    {
        _musicControl.value = PlayerPrefs.GetFloat("MusicVol");
        _soundFxControl.value = PlayerPrefs.GetFloat("FxVol");
    }

    void Update()
    {
        _master.SetFloat("MusicVol", _musicControl.value);
        _master.SetFloat("FxVol", _soundFxControl.value);
        PlayerPrefs.SetFloat("MusicVol", _musicControl.value);
        PlayerPrefs.SetFloat("FxVol", _soundFxControl.value);
    }
}
