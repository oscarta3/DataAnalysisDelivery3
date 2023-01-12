using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 previousPosition;
    [SerializeField] public Camera cam;
    HeatmapManager _heatmap;

    public void UpdateF()
    { 
        if (_heatmap.isActiveAndEnabled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
            }

            if(Input.GetMouseButton(0))
            {
                Vector3 direction = previousPosition - cam.ScreenToViewportPoint(Input.mousePosition);
                cam.transform.position = new Vector3();
                cam.transform.Rotate(new Vector3(1,0,0), direction.y * 180);
                cam.transform.Rotate(new Vector3(0,1,0), direction.y * 180, Space.World);
                cam.transform.Rotate(new Vector3(0,0,-10));

                previousPosition = cam.ScreenToViewportPoint(Input.mousePosition);
            }
        }
    }
}
