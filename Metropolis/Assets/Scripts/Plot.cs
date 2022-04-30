using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    GameManager gameManager;

    public int x;
    public int y;

    Hover hoverCheck;
    Color originalColor;
    CityManager cityManager;
    MapBuilder mapBuilder;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        hoverCheck = GameObject.Find("HoverChecker").GetComponent<Hover>();
        originalColor = GetComponent<Renderer>().material.color;
        cityManager = GameObject.Find("CityManager").GetComponent<CityManager>();
        mapBuilder = GameObject.Find("CityManager").GetComponent<MapBuilder>();
    }

    void Update()
    {
        if (hoverCheck.hoverTransform == transform && gameManager.currentMode == GamePlayModes.construction)
        {
            // show highlight
            GetComponent<Renderer>().material.color = new Color(originalColor.r * 1.8f, originalColor.g * 1.8f, originalColor.b * 1.8f, originalColor.a);

            // show building preview
            cityManager.ShowBuildingPreview(x, y, new GameBuilding("", gameManager.chosenBuilding));

            if (Input.GetMouseButtonDown(0))
            {
                // gameManager.chosenBuilding = BuildingTypes.Hospital; // this is an override. remove once contruction system is being developed
                // on click:
                if (cityManager.map.Count >= x && cityManager.map[x].Count >= y && !cityManager.map[x][y].occupied)
                {
                    cityManager.PlaceBuilding(x, y, new GameBuilding("", gameManager.chosenBuilding));
                }
            }
            if (mapBuilder.currentPreviewPos != new Vector2(x, y))
            {
                // if preview is not at these coordinates

                cityManager.ShowBuildingPreview(x, y, new GameBuilding("", gameManager.chosenBuilding));
            }
        }
        else
        {
            // hide highlight
            GetComponent<Renderer>().material.color = originalColor;
        }
    }
}
