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
        // Get Settings from PlayerPerf
        _musicControl.value = PlayerPrefs.GetFloat("MusicVol");
        _soundFxControl.value = PlayerPrefs.GetFloat("FxVol");
        _master.SetFloat("MusicVol", _musicControl.value);
        _master.SetFloat("FxVol", _soundFxControl.value);
    }

    void Update()
    {
        // Settings controls
        _master.SetFloat("MusicVol", _musicControl.value);
        _master.SetFloat("FxVol", _soundFxControl.value);

        // Save Settings to PlayerPerf
        PlayerPrefs.SetFloat("MusicVol", _musicControl.value);
        PlayerPrefs.SetFloat("FxVol", _soundFxControl.value);
    }
}
