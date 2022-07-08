using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    //public Spawner spawn;
    public float speed;
    public bool cameraStop;


    private void Start()
    {
        speed = GetComponent<Camera>().orthographicSize;
    }
    void Update()
    {
        if (!cameraStop)
        {
            if (Input.GetMouseButton(0))
            {
                transform.position = transform.position - speed * Time.deltaTime * new Vector3(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"), 0);
            }
        }
    }
}
