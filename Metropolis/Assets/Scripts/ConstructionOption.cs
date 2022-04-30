using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionOption : MonoBehaviour
{
    [SerializeField] GameObject preview;
    public BuildingTypes buildingType;
    GameObject optionPrefab;
    Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();
    Object[] prefabsArr;
    Texture2D previewTexture;
    bool previewLoaded = false;

    void Start()
    {
        prefabsArr = Resources.LoadAll("Prefabs/");
        foreach (Object prefab in prefabsArr)
        {
            prefabs.Add(prefab.name, (GameObject)prefab);
        }
        optionPrefab = prefabs[buildingType.ToString()];
        previewTexture = AssetPreview.GetAssetPreview(optionPrefab);
    }
    
    void Update()
    {
        if (previewTexture != null && !previewLoaded)
        {
            previewLoaded = true;
            Rect rect = new Rect(0, 0, previewTexture.width, previewTexture.height);
            Vector2 pivot = new Vector2(previewTexture.height / 2, previewTexture.width / 2);
            preview.GetComponent<Image>().sprite = Sprite.Create(previewTexture, rect, pivot);
        }
    }
}
