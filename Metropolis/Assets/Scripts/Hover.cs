using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Hover : MonoBehaviour
{
    [SerializeField] LayerMask hitLayer;
    public Transform hoverTransform;
    MapBuilder mapBuilder;
    CursorManager cursorManager;

    Ray ray;
    RaycastHit hit;

    void Start()
    {
        hoverTransform = null;
        mapBuilder = GameObject.Find("CityManager").GetComponent<MapBuilder>();
        cursorManager = GameObject.Find("CursorManager").GetComponent<CursorManager>();
    }

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (EventSystem.current.IsPointerOverGameObject())
        {
            hoverTransform = null;
            mapBuilder.ClearPreview();
            
            if (EventSystem.current.currentSelectedGameObject && EventSystem.current.currentSelectedGameObject.GetComponent<Button>())
            {
                cursorManager.Hand();
            }
            else
            {
                cursorManager.Pointer();
            }
        }
        else if (Physics.Raycast(ray, out hit, hitLayer))
        {
            hoverTransform = hit.transform.gameObject.transform;
            if (hoverTransform.parent && hoverTransform.parent.gameObject.GetComponent<Building>() && IsClickable(hoverTransform.parent.gameObject.GetComponent<Building>().buildingType))
            {
                cursorManager.Hand();
            }
            else
            {
                cursorManager.Pointer();
            }
        }
        else
        {
            cursorManager.Pointer();
            hoverTransform = null;
            mapBuilder.ClearPreview();
        }
    }

    bool IsClickable(BuildingTypes buildingType)
    {
        BuildingTypes[] clickable = { BuildingTypes.House, BuildingTypes.Flats, BuildingTypes.Factory, BuildingTypes.Supermarket, BuildingTypes.Hospital };
        foreach (BuildingTypes type in clickable)
        {
            if (type == buildingType)
            {
                Debug.Log("hand");
                return true;
            }
        }
        Debug.Log("pointer");
        return false;
    }
}
