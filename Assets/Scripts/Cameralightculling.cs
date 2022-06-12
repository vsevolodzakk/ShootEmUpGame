using System.Collections.Generic;
using UnityEngine;

public class Cameralightculling : MonoBehaviour
{
    [SerializeField] private List<Light> _lights;

    private void OnPreCull()
    {
        // Culling lights from Minimap Camera
        foreach (Light light in _lights)
            light.enabled = false;
    }

    private void OnPostRender()
    {
        foreach (Light light in _lights)
            light.enabled = true;
    }
}
