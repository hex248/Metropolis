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

    GameManager gameManager;

    void Start()
    {
        resetCamera = Camera.main.transform.position;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (gameManager.playing)
        {
            // make move speed relative to zoom
            panSpeed = Camera.main.orthographicSize;

            // wasd movement
            drone.position += (Input.GetAxisRaw("Horizontal") * drone.right * Time.fixedDeltaTime * panSpeed) +
                (Input.GetAxisRaw("Vertical") * drone.forward * Time.fixedDeltaTime * panSpeed);

            // camera reset
            if (Input.GetMouseButton(2))
            {
                transform.position = resetCamera;
            }

            // if not hovering over UI
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                // zoom out
                if (Input.mouseScrollDelta.y > 0)
                {
                    Camera.main.orthographicSize -= zoomChange * Time.deltaTime * smoothChange;
                }
                // zoom in
                if (Input.mouseScrollDelta.y < 0)
                {
                    Camera.main.orthographicSize += zoomChange * Time.deltaTime * smoothChange;
                }
            }

            // clamp zoom in allowed range
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minSize, maxSize);
        }
    }
}
