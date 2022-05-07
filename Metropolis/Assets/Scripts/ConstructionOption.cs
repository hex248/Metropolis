using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionOption : MonoBehaviour
{
    [SerializeField] Sprite previewSprite;
    [SerializeField] GameObject preview;
    public BuildingTypes buildingType;
    GameObject optionPrefab;
    Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();
    Object[] prefabsArr;
    bool previewLoaded = false;

    void Start()
    {
        prefabsArr = Resources.LoadAll("Prefabs/");
        foreach (Object prefab in prefabsArr)
        {
            prefabs.Add(prefab.name, (GameObject)prefab);
        }
        optionPrefab = prefabs[buildingType.ToString()];
        preview.GetComponent<Image>().sprite = previewSprite;
    }
}
