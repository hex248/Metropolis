using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hover : MonoBehaviour
{
    [SerializeField] LayerMask hitLayer;
    public Transform hoverTransform;
    MapBuilder mapBuilder;

    Ray ray;
    RaycastHit hit;

    void Start()
    {
        hoverTransform = null;
        mapBuilder = GameObject.Find("CityManager").GetComponent<MapBuilder>();
    }

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (EventSystem.current.IsPointerOverGameObject())
        {
            hoverTransform = null;
            mapBuilder.ClearPreview();
        }
        else if (Physics.Raycast(ray, out hit, hitLayer))
        {
            hoverTransform = hit.transform.gameObject.transform;
        }
        else
        {
            hoverTransform = null;
            mapBuilder.ClearPreview();
        }
    }
}
