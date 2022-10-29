using System.Collections.Generic;
using UnityEngine;

public class SkyboxRandomizer : MonoBehaviour
{
    [SerializeField] List<Material> skyboxes = new List<Material>();

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            RandomizeSkybox();
        }
    }

    private void RandomizeSkybox()
    {
        int randomIndex = Random.Range(0, skyboxes.Count);
        RenderSettings.skybox = skyboxes[randomIndex];
    }
}
