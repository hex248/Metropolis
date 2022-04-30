using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [SerializeField] Light directionalLight;
    [SerializeField] LightingPreset preset;

    [SerializeField, Range(0, 24)] float timeOfDay;
    [SerializeField, Range(0, 50)] float timeScale;

    private void Update()
    {
        if (Application.isPlaying)
        {
            timeOfDay += Time.deltaTime / 60; // one minute is equal to one hour in game
            timeOfDay %= 24;
            UpdateLighting(timeOfDay / 24);
        }
        else
        {
            UpdateLighting(timeOfDay / 24);
        }
    }

    void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = preset.ambientColor.Evaluate(timePercent);
        RenderSettings.fogColor = preset.fogColor.Evaluate(timePercent);

        directionalLight.color = preset.directionalColor.Evaluate(timePercent);

        directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360) - 90, 180, 0));
    }
}
