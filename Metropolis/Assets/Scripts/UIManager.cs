using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
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

    [SerializeField] Transform tasksPanel;
    [SerializeField] GameObject taskPrefab;
    List<Task> oldtasks = new List<Task>();


    [SerializeField] GameObject statsParent;
    [SerializeField] TextMeshProUGUI emissionsText;

    Vector3 constructionDestination = new Vector3(110.0f, 240.0f, 0.0f);
    Vector3 populationDestination = new Vector3(230.0f, 230.0f, 0.0f);
    Vector3 tasksDestination = new Vector3(240.0f, 110.0f, 0.0f);
    Vector3 buttonOrigin = new Vector3(95.0f, 95.0f, 0.0f);

    [SerializeField][Range(0, 25)] float buttonAnimationTime = 7.5f;

    bool actionButtonActivated;

    GameManager gameManager;
    TaskManager taskManager;
    StatisticsManager statsManager;

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
        constructionParent.SetActive(false);
        populationParent.SetActive(false);
        taskParent.SetActive(false);
        npcEncounterParent.SetActive(false);

        AddTasksToDisplay();
    }

    void Update()
    {
        if (gameManager.currentMode == GamePlayModes.construction || gameManager.currentMode == GamePlayModes.destruction)
        {
            constructionParent.SetActive(true);
            constructionTooltip.SetActive(true);
            destructionTooltip.SetActive(true);
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


        if (actionButtonActivated)
        {
            // show buttons
            constructionButton.transform.position = Vector3.MoveTowards(constructionButton.transform.position,
                constructionDestination,
                Vector3.Distance(constructionButton.transform.position,
                constructionDestination) * buttonAnimationTime * Time.deltaTime);
            populationButton.transform.position = Vector3.MoveTowards(populationButton.transform.position,
                populationDestination,
                Vector3.Distance(populationButton.transform.position,
                populationDestination) * buttonAnimationTime * Time.deltaTime);
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

    public void AddTasksToDisplay()
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
    }

    public NPCEncounter NPCPopup()
    {
        //! NOTE: PARENTS DONT GET DISABLED
        Debug.Log("adwadadad");
        actionMenuParent.SetActive(false);
        constructionParent.SetActive(false);
        populationParent.SetActive(false);
        taskParent.SetActive(false);
        npcEncounterParent.SetActive(true);


        return npcEncounterParent.GetComponent<NPCEncounter>();
    }
}
