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

    [SerializeField] float xMovement = 0;
    [SerializeField] float yMovement = 0;
    [SerializeField] float xRange = 16;
    [SerializeField] float yRange = 9;

    bool zoom = false;

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
            xMovement += Input.GetAxisRaw("Horizontal") * Time.fixedDeltaTime;

            if (xMovement == Mathf.Clamp(xMovement, -xRange / 2, xRange / 2))
            {
                drone.position += (Input.GetAxisRaw("Horizontal") * drone.right * Time.fixedDeltaTime * panSpeed);
            }
            else
            {
                if (xMovement < -xRange / 2)
                {
                    xMovement = -xRange / 2;
                }
                else if (xMovement > xRange / 2)
                {
                    xMovement = xRange / 2;
                }
            }

            yMovement += Input.GetAxisRaw("Vertical") * Time.fixedDeltaTime;

            if (yMovement == Mathf.Clamp(yMovement, -yRange / 2, yRange / 2))
            {
                drone.position += (Input.GetAxisRaw("Vertical") * drone.forward * Time.fixedDeltaTime * panSpeed);
            }
            else
            {
                if (yMovement < -yRange / 2)
                {
                    yMovement = -yRange / 2;
                }
                else if (yMovement > yRange / 2)
                {
                    yMovement = yRange / 2;
                }
            }


            // camera reset
            if (Input.GetMouseButton(2))
            {
                transform.position = resetCamera;
                xMovement = 0;
                yMovement = 0;
            }

            // if not hovering over UI
            if (zoom)
            {
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
            }

            // clamp zoom in allowed range
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minSize, maxSize);
        }
    }
}
