using System.Collections;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] GameObject _tutorial;

    void Start()
    {
        StartCoroutine(HideTutorialInfo());
    }
    IEnumerator HideTutorialInfo()
    {
        yield return new WaitForSeconds(5f);
        _tutorial.SetActive(false);
    }
}
