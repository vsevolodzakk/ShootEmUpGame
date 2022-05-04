using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioClip _playerShotSound;

    private void OnEnable()
    {
        GunController.onGunFire += PlayShotSound;
    }

    private void PlayShotSound()
    {
        // Play sound
    }
}
