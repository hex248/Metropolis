using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum BuildingTypes
{
    House,
    Flats,
    Factory,
    Supermarket,
    School,
    Hospital,
    Tree,
    Hedge,
    Grass,
    IntersectionRoad,
    StraightRoad,
    TurnRoad
}

[System.Serializable]
public class GameBuilding
{
    public int x;
    public int y;
    public int rotation;
    public string buildingName;
    public BuildingTypes buildingType;

    public GameBuilding(int x, int y, string name, BuildingTypes type)
    {
        this.x = x;
        this.y = y;
        this.buildingName = name;
        this.buildingType = type;
    }
}

public class Building : MonoBehaviour
{
    GameManager gameManager;

    public GameBuilding building;
    [SerializeField] string buildingName;
    public BuildingTypes buildingType;
    public GameObject box;
    public GameObject model;
    Color boxOriginalColor;

    Hover hoverCheck;
    CityManager cityManager;
    StatisticsManager statsManager;
    UIManager uiManager;
    CursorManager cursorManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        boxOriginalColor = box.GetComponent<Renderer>().material.color;
        hoverCheck = GameObject.Find("HoverChecker").GetComponent<Hover>();
        cityManager = GameObject.Find("CityManager").GetComponent<CityManager>();
        statsManager = GameObject.Find("StatisticsManager").GetComponent<StatisticsManager>();
        uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        cursorManager = GameObject.Find("CursorManager").GetComponent<CursorManager>();
    }

    void Update()
    {
        if (gameManager.currentMode == GamePlayModes.destruction)
        {
            box.SetActive(true);

            if (hoverCheck.hoverTransform == box.transform || hoverCheck.hoverTransform == model.transform)
            {
                box.GetComponent<Renderer>().material.color = new Color(boxOriginalColor.r * 1.8f, boxOriginalColor.g * 1.8f, boxOriginalColor.b * 1.8f, boxOriginalColor.a * 1.8f);

                if (Input.GetMouseButtonDown(0) && gameManager.currentMode == GamePlayModes.destruction)
                {
                    // building right clicked
                    int cellX = (int)(-1 * transform.position.z);
                    int cellY = (int)(transform.position.x);
                    var a = cityManager.map[cellX][cellY];
                    cityManager.ClearCell(cellX, cellY, gameObject);
                }
            }
        }
        else if (hoverCheck.hoverTransform != model.transform)
        {
            box.SetActive(false);
        }

        if (hoverCheck.hoverTransform == model.transform)
        {
            // show highlight
            box.GetComponent<Renderer>().material.color = new Color(boxOriginalColor.r*1.8f, boxOriginalColor.g * 1.8f, boxOriginalColor.b * 1.8f, boxOriginalColor.a);

            if (Input.GetMouseButtonDown(0) && gameManager.currentMode == GamePlayModes.none)
            {
                // building selected
                OnClick();
            }
        }
        else
        {
            // hide outline
            box.GetComponent<Renderer>().material.color = boxOriginalColor;
        }
    }

    void OnClick()
    {
        if (CanClick())
        {
            BuildingInfo info = uiManager.ShowBuildingInfo();
            info.buildingType = buildingType;
            Debug.Log(building.x);
            Debug.Log(building.y);
            Debug.Log(cityManager.map[building.x][building.y].occupants);
            info.occupants = cityManager.map[building.x][building.y].occupants;
            info.moneyGenerated = statsManager.GetMoneyGenerated(buildingType);
            info.UpdateUI();
        }
    }

    bool CanClick()
    {
        BuildingTypes[] acceptedBuildingTypes = { BuildingTypes.House, BuildingTypes.Flats, BuildingTypes.Factory, BuildingTypes.Supermarket, BuildingTypes.Hospital };
        foreach (BuildingTypes type in acceptedBuildingTypes)
        {
            Debug.Log(type);
            if (type == buildingType) return true;
        }
        return false;
    }
}
