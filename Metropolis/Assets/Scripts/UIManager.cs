using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject bridgeObject;
    [SerializeField] GameObject actionMenuParent;
    [SerializeField] GameObject constructionCloseButton;
    [SerializeField] GameObject actionButton;
    [SerializeField] GameObject constructionButton;
    [SerializeField] GameObject populationButton;
    [SerializeField] GameObject tasksButton;
    [SerializeField] GameObject constructionTooltipParent;
    [SerializeField] GameObject constructionTooltip;
    [SerializeField] GameObject destructionTooltip;
    [SerializeField] GameObject constructionOptions;

    [SerializeField] GameObject constructionParent;
    [SerializeField] GameObject populationParent;
    [SerializeField] GameObject taskParent;
    [SerializeField] GameObject npcEncounterParent;
    [SerializeField] GameObject buildingInfoParent;

    [SerializeField] Transform tasksPanel;
    [SerializeField] GameObject taskPrefab;

    [SerializeField] GameObject rotationArrows;

    [SerializeField] GameObject statsParent;
    [SerializeField] TextMeshProUGUI emissionsText;
    [SerializeField] TextMeshProUGUI balanceText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI costOfActionText;
    [SerializeField] TextMeshProUGUI populationText;

    [SerializeField] GameObject balanceChange;
    [SerializeField] TextMeshProUGUI balanceChangeText;

    [SerializeField] TextMeshProUGUI optionNamePopupText;

    Vector3 constructionDestination = new Vector3(110.0f, 240.0f, 0.0f);
    Vector3 populationDestination = new Vector3(230.0f, 230.0f, 0.0f);
    Vector3 tasksDestination = new Vector3(240.0f, 110.0f, 0.0f);
    Vector3 buttonOrigin = new Vector3(95.0f, 95.0f, 0.0f);

    [SerializeField][Range(0, 25)] float buttonAnimationTime = 7.5f;

    bool actionButtonActivated;

    GameManager gameManager;
    TaskManager taskManager;
    StatisticsManager statsManager;

    string buildingTypeStr = "";

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        taskManager = GameObject.Find("TaskManager").GetComponent<TaskManager>();
        statsManager = GameObject.Find("StatisticsManager").GetComponent<StatisticsManager>();
        actionButtonActivated = false;
        constructionButton.transform.position = buttonOrigin;
        populationButton.transform.position = buttonOrigin;
        tasksButton.transform.position = buttonOrigin;

        actionMenuParent.SetActive(true);
        rotationArrows.SetActive(true);
        constructionParent.SetActive(false);
        populationParent.SetActive(false);
        buildingInfoParent.SetActive(false);
        taskParent.SetActive(false);
        npcEncounterParent.SetActive(false);

        AddTasksToDisplay(true);
    }

    void Update()
    {
        if (gameManager.playing)
        {
            if (gameManager.currentMode == GamePlayModes.construction || gameManager.currentMode == GamePlayModes.destruction)
            {
                constructionParent.SetActive(true);
                constructionTooltip.SetActive(true);
                destructionTooltip.SetActive(true);
                costOfActionText.text = $"Cost of Action: ${gameManager.costOfAction}";
                optionNamePopupText.text = buildingTypeStr;
            }
            else
            {
                constructionParent.SetActive(false);
            }

            if (gameManager.currentMode == GamePlayModes.construction)
            {
                constructionParent.SetActive(true);
                destructionTooltip.SetActive(false);
                constructionTooltip.SetActive(true);
                constructionOptions.SetActive(true);
            }
            else if (gameManager.currentMode == GamePlayModes.destruction)
            {
                constructionParent.SetActive(true);
                destructionTooltip.SetActive(true);
                constructionTooltip.SetActive(false);
                constructionOptions.SetActive(false);
            }
        }
        else
        {
            constructionParent.SetActive(false);
        }


        if (actionButtonActivated)
        {
            // show buttons
            constructionButton.transform.position = Vector3.MoveTowards(constructionButton.transform.position,
                constructionDestination,
                Vector3.Distance(constructionButton.transform.position,
                constructionDestination) * buttonAnimationTime * Time.deltaTime);
            //populationButton.transform.position = Vector3.MoveTowards(populationButton.transform.position,
            //    populationDestination,
            //    Vector3.Distance(populationButton.transform.position,
            //    populationDestination) * buttonAnimationTime * Time.deltaTime);
            tasksButton.transform.position = Vector3.MoveTowards(tasksButton.transform.position,
                tasksDestination,
                Vector3.Distance(tasksButton.transform.position,
                tasksDestination) * buttonAnimationTime * Time.deltaTime);
            actionButton.transform.position = Vector3.MoveTowards(actionButton.transform.position,
                buttonOrigin,
                Vector3.Distance(actionButton.transform.position,
                buttonOrigin) * buttonAnimationTime * Time.deltaTime);
        }
        else
        {
            // hide buttons
            constructionButton.transform.position = Vector3.MoveTowards(constructionButton.transform.position,
                buttonOrigin,
                Vector3.Distance(constructionButton.transform.position,
                buttonOrigin) * buttonAnimationTime * Time.deltaTime);
            populationButton.transform.position = Vector3.MoveTowards(populationButton.transform.position,
                buttonOrigin,
                Vector3.Distance(populationButton.transform.position,
                buttonOrigin) * buttonAnimationTime * Time.deltaTime);
            tasksButton.transform.position = Vector3.MoveTowards(tasksButton.transform.position,
                buttonOrigin,
                Vector3.Distance(tasksButton.transform.position,
                buttonOrigin) * buttonAnimationTime * Time.deltaTime);

            actionButton.transform.position = Vector3.MoveTowards(actionButton.transform.position,
                buttonOrigin,
                Vector3.Distance(actionButton.transform.position,
                buttonOrigin) * buttonAnimationTime * Time.deltaTime);
        }
    }

    public void ShowPopulationMenu()
    {
        populationParent.SetActive(true);
    }
    public void HidePopulationMenu()
    {
        populationParent.SetActive(false);
    }

    public BuildingInfo ShowBuildingInfo()
    {
        buildingInfoParent.SetActive(true);
        return buildingInfoParent.GetComponent<BuildingInfo>();
    }

    public void HideBuildingInfo()
    {
        buildingInfoParent.SetActive(false);
    }

    public void ShowTaskMenu()
    {
        AddTasksToDisplay();
        taskParent.SetActive(true);
    }
    public void HideTaskMenu()
    {
        taskParent.SetActive(false);
    }

    public void QuitAll()
    {
        //gameManager.currentMode = GamePlayModes.none;
    }

    public void ActionButtonPressed()
    {
        if (actionButtonActivated)
        {
            actionButtonActivated = false;
        }
        else
        {
            actionButtonActivated = true;
        }
    }

    public void ActionTaken()
    {
        QuitAll();
        actionButtonActivated = false;
    }

    public void AddTasksToDisplay(bool initialLoad = false)
    {
        foreach (Transform child in tasksPanel.transform)
        {
            Destroy(child.gameObject);
        }
        float lastY = 0.0f;
        foreach(Task task in taskManager.tasks)
        {
            GameObject taskObject = Instantiate(taskPrefab, tasksPanel);
            taskObject.GetComponent<TaskComponent>().task = task;
            lastY = taskObject.transform.position.y;
        }

        RectTransform rt = tasksPanel.GetComponent<RectTransform>();
        float height = (215.0f * taskManager.tasks.Count) + 15.0f;
        if (height < 700.0f) height = 700.0f;
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, height);
    }

    public void UpdateStats()
    {
        emissionsText.text = $"Emissions: {Mathf.Round((statsManager.stats.emissions / statsManager.stats.maximumEmissions) * 100)}%";
        balanceText.text = $"${gameManager.balance}";

        // time
        int timeHours = 0;
        int timeMins = 0;

        timeHours = (int)Mathf.Floor(statsManager.stats.timeOfDay);
        timeMins = (int)Mathf.Floor((statsManager.stats.timeOfDay - timeHours) * 60);

        string timeHoursStr = timeHours < 10 ? $"0{timeHours}" : $"{timeHours}";
        string timeMinsStr = timeMins < 10 ? $"0{timeMins}" : $"{timeMins}";
        timeText.text = $"{timeHoursStr}:{timeMinsStr}";

        populationText.text = $"{statsManager.stats.population}";
    }

    public NPCEncounter NPCPopup()
    {
        actionMenuParent.SetActive(false);
        statsParent.SetActive(false);
        constructionParent.SetActive(false);
        populationParent.SetActive(false);
        buildingInfoParent.SetActive(false);
        taskParent.SetActive(false);
        rotationArrows.SetActive(false);
        npcEncounterParent.SetActive(true);


        return npcEncounterParent.GetComponent<NPCEncounter>();
    }

    public void HideNPCPopup()
    {
        actionMenuParent.SetActive(true);
        statsParent.SetActive(true);
        rotationArrows.SetActive(true);
        npcEncounterParent.SetActive(false);
    }

    public void HideActionButton()
    {
        actionButton.SetActive(false);
        constructionButton.SetActive(false);
        populationButton.SetActive(false);
        tasksButton.SetActive(false);
    }

    public void HideStatistics()
    {
        statsParent.SetActive(false);
    }

    public void HideArrows()
    {
        rotationArrows.SetActive(false);
    }

    public IEnumerator ChangeBalance(int amount)
    {
        balanceChange.SetActive(true);
        balanceChangeText.text = $"+${amount}";

        yield return new WaitForSeconds(1.0f);

        balanceChange.SetActive(false);

        yield return null;
    }

    public void HoverConstructionOption(GameObject gameObject)
    {
        BuildingTypes buildingType = gameObject.GetComponent<ConstructionOption>().buildingType;

        switch (buildingType)
        {
            case BuildingTypes.House:
                buildingTypeStr = "House";
                break;
            case BuildingTypes.Flats:
                buildingTypeStr = "Flats";
                break;
            case BuildingTypes.Factory:
                buildingTypeStr = "Factory";
                break;
            case BuildingTypes.Supermarket:
                buildingTypeStr = "Supermarket";
                break;
            case BuildingTypes.School:
                buildingTypeStr = "School";
                break;
            case BuildingTypes.Hospital:
                buildingTypeStr = "Hospital";
                break;
            case BuildingTypes.Tree:
                buildingTypeStr = "Tree";
                break;
            case BuildingTypes.Hedge:
                buildingTypeStr = "Hedge";
                break;
            case BuildingTypes.Grass:
                buildingTypeStr = "Grass";
                break;
            case BuildingTypes.IntersectionRoad:
                buildingTypeStr = "Road (Intersection)";
                break;
            case BuildingTypes.StraightRoad:
                buildingTypeStr = "Road (Straight)";
                break;
            case BuildingTypes.TurnRoad:
                buildingTypeStr = "Road (Turn)";
                break;
        }
    }

    public void StopHovering()
    {
        buildingTypeStr = "";
    }
}
