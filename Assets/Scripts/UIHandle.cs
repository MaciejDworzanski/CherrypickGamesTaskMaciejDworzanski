using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHandle : MonoBehaviour
{
    public Camera cam;
    public float cameraMaxSize;
    public Slider cameraSlider;
    public GridHandle grid;
    public CameraMove mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        OnCameraSliderUpdate();
    }

    

    public void OnCameraSliderUpdate()
    {
        cam.orthographicSize = cameraMaxSize * cameraSlider.value + 1;
    }

    public void Spawn()
    {
        grid.spawn = true;
        mainCamera.cameraStop = true;
    }

    public void StopSpawn()
    {
        grid.spawn = false;
        mainCamera.cameraStop = false;
    }

    public void DebugMe()
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (GameObject go in allObjects)
            if (go.activeInHierarchy)
                if(go.GetComponent<Slot>())
                {
                    Debug.Log($"State dla {go.GetComponent<Slot>().positionOnGrid.x}, {go.GetComponent<Slot>().positionOnGrid.y} ma {go.GetComponent<Slot>().state}");
                }
    }

    public void Clear()
    {
        grid.clear = true;
    }

    public void StopCamera()
    {
        mainCamera.cameraStop = true;
    }

    public void EnableCameraMove()
    {
        mainCamera.cameraStop = false;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
