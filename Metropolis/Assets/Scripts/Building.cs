using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum BuildingTypes
{
    House,
    Office,
    Factory,
    Shop,
    School,
    Hospital,
    Tree,
    Hedge,
    Grass
}

[System.Serializable]
public class GameBuilding
{
    public int x;
    public int y;
    public int rotation;
    public string buildingName;
    public BuildingTypes buildingType;

    public GameBuilding(string name, BuildingTypes type)
    {
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
    [SerializeField] TextMeshProUGUI buildingNameText;
    GameObject canvas;
    public GameObject box;
    public GameObject model;
    Color boxOriginalColor;

    Hover hoverCheck;
    CityManager cityManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        building = new GameBuilding(buildingName, buildingType);
        buildingNameText.text = buildingName != "" ? buildingName : $"{buildingType}";
        canvas = buildingNameText.gameObject.transform.parent.gameObject;
        boxOriginalColor = box.GetComponent<Renderer>().material.color;
        hoverCheck = GameObject.Find("HoverChecker").GetComponent<Hover>();
        cityManager = GameObject.Find("CityManager").GetComponent<CityManager>();

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

        canvas.transform.position = new Vector3(canvas.transform.position.x, transform.localScale.y, canvas.transform.position.z);
        if (hoverCheck.hoverTransform == model.transform)
        {
            // show highlight
            box.GetComponent<Renderer>().material.color = new Color(boxOriginalColor.r*1.8f, boxOriginalColor.g * 1.8f, boxOriginalColor.b * 1.8f, boxOriginalColor.a);

            // show name

            buildingNameText.text = buildingName != "" ? buildingName : buildingType.ToString();
            // !TEMPORARILY DISABLED WHILE CANVAS CLIPS THROUGH OTHER BUILDINGS
            //buildingNameText.gameObject.SetActive(true);
            if (Input.GetMouseButtonDown(0) && gameManager.currentMode == GamePlayModes.none)
            {
                // building selected
                Debug.Log("SHOW STATS");
                OnClick();
            }
        }
        else
        {
            // hide name
            buildingNameText.gameObject.SetActive(false);

            // hide outline
            box.GetComponent<Renderer>().material.color = boxOriginalColor;
        }
    }

    void OnClick()
    {

    }
}
