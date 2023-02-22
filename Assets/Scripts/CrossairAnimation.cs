using System.Collections;
using UnityEngine;

public class CrossairAnimation : MonoBehaviour
{
    private Animator _crossairAnimator;
    private float _cooldown = 1.3f;

    private void OnEnable()
    {
        GunController.OnGunReload += PlayCrossairReloadAnimation;
        _crossairAnimator = GetComponent<Animator>();
    }

    private void OnDisable()
    {
        GunController.OnGunReload -= PlayCrossairReloadAnimation;
    }

    private void PlayCrossairReloadAnimation()
    {
        StartCoroutine(AnimateCrossair());
    }

    private IEnumerator AnimateCrossair()
    {
        _crossairAnimator.SetBool("isReloading", true);
        yield return new WaitForSeconds(_cooldown);
        _crossairAnimator.SetBool("isReloading", false);
    }
}
