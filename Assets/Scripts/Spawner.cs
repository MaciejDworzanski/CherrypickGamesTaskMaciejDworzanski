using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GridHandle grid;
    List<Slot> avaibleSlots;
    public CameraMove mainCamera;
    //private bool isDragging;
    // Start is called before the first frame update
    void Start()
    {
        avaibleSlots = new List<Slot>();
    }
    public void OnMouseDrag()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        transform.Translate(mousePosition);
        mainCamera.cameraStop = true;
    }



    public void OnMouseUp()
    {
        mainCamera.cameraStop = false;
        float radius = 0.5f;
        while(true)
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius);
            
            foreach (var hitCollider in hitColliders)
            {
                if(hitCollider.GetComponent<Slot>())
                {
                    if(hitCollider.GetComponent<Slot>().state == 0 || hitCollider.GetComponent<Slot>().state == 2)
                    {
                        avaibleSlots.Add(hitCollider.GetComponent<Slot>());
                    }
                }
            }
            if (avaibleSlots.Count > 0)
            {
                float distance;
                float nearestSlotDistance = Mathf.Infinity;
                Slot nearestSlot = null;
                foreach(Slot slot in avaibleSlots)
                {
                    distance = Vector2.Distance(new (transform.position.x,transform.position.y), new(slot.transform.position.x,slot.transform.position.y));
                    if (distance < nearestSlotDistance)
                    {
                        nearestSlotDistance = distance;
                        nearestSlot = slot;
                    }
                }
                if(nearestSlot != null)
                {
                    transform.position = nearestSlot.transform.position;
                    grid.allSlots[(int)grid.spawnSlot.positionOnGrid.x, (int)grid.spawnSlot.positionOnGrid.y].state = 0;
                    grid.spawnSlot = nearestSlot;
                    nearestSlot.state = 2;
                    grid.widthToCheck = (int) nearestSlot.positionOnGrid.x;
                    grid.heightToCheck = (int)nearestSlot.positionOnGrid.y;
                    grid.ResetVariables();
                    avaibleSlots.Clear();
                    break;
                }

            }
            radius += 1;
            avaibleSlots.Clear();
            if (radius > 50) break;
        }
    }
}
