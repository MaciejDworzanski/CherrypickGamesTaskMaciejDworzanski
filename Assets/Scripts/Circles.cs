using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circles : MonoBehaviour
{
    public float posx;
    public float posy;
    public Vector3 positionOnGrid;
    public bool deathMark;
    private bool isOnPosition;
    void Start()
    {
        isOnPosition = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isOnPosition) MoveToPosition();
    }

    void MoveToPosition()
    {
        transform.position = Vector3.MoveTowards(transform.position, positionOnGrid, 0.5f);
        if (transform.position == positionOnGrid) isOnPosition = true;
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
