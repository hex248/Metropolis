using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GamePlayModes
{
    none,
    construction,
    destruction
}

enum RotationTypes
{
    clockwise,
    anticlockwise
}

public class GameManager : MonoBehaviour
{
    public string saveName;

    public GamePlayModes currentMode;

    [SerializeField] Transform buildingsParent;
    [SerializeField] Transform pavementParent;
    [SerializeField] Color oldPavementColor;
    [SerializeField] Color constructionBlockedColor;
    [SerializeField] Color destroyableColor;
    public BuildingTypes chosenBuilding;
    public int desiredRotation;

    CityManager cityManager;
    MapBuilder mapBuilder;
    TaskManager taskManager;

    UIManager ui;

    void Start()
    {
        desiredRotation = 0;
        cityManager = GameObject.Find("CityManager").GetComponent<CityManager>();
        mapBuilder = GameObject.Find("CityManager").GetComponent<MapBuilder>();
        taskManager = GameObject.Find("TaskManager").GetComponent<TaskManager>();

        ui = GameObject.Find("Canvas").GetComponent<UIManager>();
    }

    void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F8))
        {
            taskManager.CreateTask("Citizen", "Build a school", "Use the construction menu to construct a school for Citizen's children", 1);
        }
        if (currentMode == GamePlayModes.construction || currentMode == GamePlayModes.destruction)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                currentMode = GamePlayModes.none;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                currentMode = GamePlayModes.construction;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                mapBuilder.ClearPreview();
                currentMode = GamePlayModes.destruction;
            }
        }
        if (currentMode == GamePlayModes.construction)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                desiredRotation = Rotate(desiredRotation, 90, RotationTypes.clockwise);
            }
        }
        if (1 < 0) // disabled
        {
            if (currentMode == GamePlayModes.construction)
            {
                Renderer[] allPavementRenderers = pavementParent.GetComponentsInChildren<Renderer>();

                for (int i = 0; i < allPavementRenderers.Length; i++)
                {
                    allPavementRenderers[i].material.color = constructionBlockedColor;
                }

                // make buildings invisible

                buildingsParent.gameObject.SetActive(false);

            }
            else if (currentMode == GamePlayModes.destruction)
            {
                Renderer[] allPavementRenderers = pavementParent.GetComponentsInChildren<Renderer>();

                for (int i = 0; i < allPavementRenderers.Length; i++)
                {
                    Plot plot = allPavementRenderers[i].gameObject.GetComponent<Plot>();

                    if (cityManager.map[plot.x][plot.y].occupied)
                    {
                        allPavementRenderers[i].material.color = destroyableColor;
                    }
                    else
                    {
                        allPavementRenderers[i].material.color = oldPavementColor;
                    }
                }

                // make buildings invisible

                buildingsParent.gameObject.SetActive(false);
            }
            else if (currentMode == GamePlayModes.none)
            {
                Renderer[] allPavementRenderers = pavementParent.GetComponentsInChildren<Renderer>();

                for (int i = 0; i < allPavementRenderers.Length; i++)
                {
                    allPavementRenderers[i].material.color = oldPavementColor;
                }

                // make buildings visible

                buildingsParent.gameObject.SetActive(true);
            }
        }
    }

    public void ConstructionButtonPressed()
    {
        currentMode = GamePlayModes.construction;
        ui.ActionTaken();
    }

    public void PopulationButtonPressed()
    {
        currentMode = GamePlayModes.none;
        ui.ShowPopulationMenu();
        ui.ActionTaken();
    }
    public void TaskButtonPressed()
    {
        currentMode = GamePlayModes.none;
        ui.ShowTaskMenu();
        ui.ActionTaken();
    }
    public void ExitConstructionMode()
    {
        currentMode = GamePlayModes.none;
    }

    public void ExitPopulationMenu()
    {
        ui.HidePopulationMenu();
    }
    public void ExitTaskMenu()
    {
        ui.HideTaskMenu();
    }

    public void ConstructionOptionSelected(ConstructionOption option)
    {
        Debug.Log(option.buildingType);
        chosenBuilding = option.buildingType;
    }

    int Rotate(int originalRotation, int amountToRotate, RotationTypes rotationType)
    {
        int newRotation = originalRotation + amountToRotate;

        while (newRotation > 360)
        {
            newRotation -= 360;
        }
        if (newRotation > 180)
        {
            newRotation = -1 * (360 - newRotation);
        }
        return newRotation;
    }
}