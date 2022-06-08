using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] Transform pivotPoint;
    public void Left()
    {
        transform.RotateAround(pivotPoint.position, new Vector3(0, 90, 0), 90);
    }

    public void Right()
    {
        transform.RotateAround(pivotPoint.position, new Vector3(0,90,0), -90);
    }
}
