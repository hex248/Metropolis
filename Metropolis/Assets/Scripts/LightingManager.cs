using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    [SerializeField] Light directionalLight;
    [SerializeField] LightingPreset preset;

    float timeOfDay;

    StatisticsManager stats;
    private void Start()
    {
        stats = GameObject.Find("StatisticsManager").GetComponent<StatisticsManager>();
    }

    private void Update()
    {
        timeOfDay = stats.stats.timeOfDay;
        UpdateLighting(timeOfDay / 24);
    }

    void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = preset.ambientColor.Evaluate(timePercent);
        RenderSettings.fogColor = preset.fogColor.Evaluate(timePercent);

        directionalLight.color = preset.directionalColor.Evaluate(timePercent);

        directionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360) - 90, 180, 0));
    }
}
