using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    [SerializeField] float zoomChange;
    [SerializeField] float smoothChange;
    [SerializeField] float minSize, maxSize;
    float panSpeed;

    Vector3 resetCamera;
    [SerializeField] Transform drone;

    void Start()
    {
        resetCamera = Camera.main.transform.position;
    }

    void Update()
    {
        panSpeed = Camera.main.orthographicSize;
        // wasd movement
        drone.position += (Input.GetAxisRaw("Horizontal") * drone.right * Time.fixedDeltaTime * panSpeed) +
            (Input.GetAxisRaw("Vertical") * drone.forward * Time.fixedDeltaTime * panSpeed);
        
        if (Input.GetMouseButton(2))
        {
            transform.position = resetCamera;
        }

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.mouseScrollDelta.y > 0)
            {
                Camera.main.orthographicSize -= zoomChange * Time.deltaTime * smoothChange;
            }
            if (Input.mouseScrollDelta.y < 0)
            {
                Camera.main.orthographicSize += zoomChange * Time.deltaTime * smoothChange;
            }
        }

        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minSize, maxSize);
    }
}
