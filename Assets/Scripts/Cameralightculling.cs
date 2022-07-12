using System.Collections.Generic;
using UnityEngine;

public class Cameralightculling : MonoBehaviour
{
    [SerializeField] private List<Light> _lights;

    private void OnPreCull()
    {
        // Culling lights from Minimap Camera
        for(int i = 0; i < _lights.Count; i++)
            _lights[i].enabled = false;
    }

    private void OnPostRender()
    {
        for(int i = 0; i < _lights.Count; i++)
            _lights[i].enabled = true;
    }
}
