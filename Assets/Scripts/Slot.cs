using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public Vector2 positionOnGrid;
    public float posx;
    public float posy;
    public int state;           //0 - empty; 1 - blocked; 2 - spawner; 3 - blue; 4 - green; 5 - red
    public Circles circle;

    public void DestroyCircle()
    {
        if(circle != null) circle.DestroyMe();

        circle = null;
        state = 0;
    }
}
