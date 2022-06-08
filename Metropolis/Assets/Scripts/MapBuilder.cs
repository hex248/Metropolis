using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBuilder : MonoBehaviour
{
    Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();
    Object[] prefabsArr;
    [SerializeField] Transform buildingPreviewParent;
    [SerializeField] Transform buildingsParent;
    [SerializeField] Transform pavementParent;

    public Vector2 currentPreviewPos;

    GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        prefabsArr = Resources.LoadAll("Prefabs/");
        foreach (Object prefab in prefabsArr)
        {
            prefabs.Add(prefab.name, (GameObject)prefab);
        }
    }

    public void Build(List<List<Cell>> map)
    {
        List<Cell> buildingCells = new List<Cell>();
        for (int x = 0; x < map.Count; x++)
        {
            for (int y = 0; y < map[x].Count; y++)
            {
                if (map[x][y].building != null)
                {
                    buildingCells.Add(map[x][y]);
                }
                Plot pavement = Instantiate(prefabs["Pavement"], new Vector3(y, -0.75f, -x), Quaternion.identity, pavementParent).GetComponent<Plot>();
                pavement.x = x;
                pavement.y = y;
            }
        }

        foreach (Cell cell in buildingCells)
        {
            Instantiate(prefabs[cell.building.buildingType.ToString()], new Vector3(cell.y, 0, -cell.x), Quaternion.Euler(0, cell.building.rotation, 0), buildingsParent);
        }
    }

    public void NewBuilding(int x, int y, GameBuilding building)
    {
        GameObject tempBuildingObj = Instantiate(prefabs[building.buildingType.ToString()], new Vector3(y, 0, -x), Quaternion.Euler(0, gameManager.desiredRotation, 0), buildingsParent);
        tempBuildingObj.GetComponent<Building>().building.rotation = gameManager.desiredRotation;
        tempBuildingObj.GetComponent<Building>().building = building;
        Debug.Log(tempBuildingObj.GetComponent<Building>().building.x);
        Debug.Log(tempBuildingObj.GetComponent<Building>().building.y);
    }

    public void ClearPreview()
    {
        foreach (Transform child in buildingPreviewParent.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void PreviewBuilding(int x, int y, GameBuilding building)
    {
        foreach (Transform child in buildingPreviewParent.transform)
        {
            if (child.gameObject.transform.position.x == x && child.gameObject.transform.position.y == y) // if the preview is already present
            {
                return;
            }
            Destroy(child.gameObject);
        }
        GameObject tempBuildingObj = Instantiate(prefabs[building.buildingType.ToString()], new Vector3(y, 0, -x), Quaternion.Euler(0, gameManager.desiredRotation, 0), buildingPreviewParent);
        foreach (BoxCollider col in tempBuildingObj.GetComponent<Building>().model.GetComponents<BoxCollider>())
        {
            col.enabled = false;
        }

        currentPreviewPos = new Vector2(x, y);
    }

    public void DestroyBuilding(GameObject buildingObj)
    {
        Destroy(buildingObj);
    }
}
