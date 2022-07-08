using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHandle : MonoBehaviour
{
    public int width;
    public int height;
    public GameObject emptyGrid1;
    public GameObject emptyGrid2;
    public GameObject blockedSlot;
    public GameObject spawner;
    public GameObject colorCircle1;
    public GameObject colorCircle2;
    public GameObject colorCircle3;
    public UIHandle UIHandle;
    public Slot[,] allSlots;
    public Slot spawnSlot;
    public List<Circles> spawnedCircles;
    public TextAsset textJSON;
    public bool spawn;
    public bool clear;
    public int widthToCheck;
    public int heightToCheck;
    public int howManyToCheck;
    public int howManyChecked;
    public int countFirstDirRepeats = 0;
    public int countSecoundDirRepeats = 0;
    public int countThirdDirRepeats = 0;
    public int countForthDirRepeats = 0;
    public bool noEmptyOnRight;
    public bool noEmptyOnLeft;
    public bool noEmptyUp;
    public bool noEmptyDown;
    public int direction;
    public bool firstTime;
    private bool isHeightEvenNumber;
    private bool isEven;


    void Awake()
    {
        Application.targetFrameRate = 60;
        LoadSize();
        if (height > width) UIHandle.cameraMaxSize = height / 2;
        else UIHandle.cameraMaxSize = width / 2;
        direction = 0;
        allSlots = new Slot[width, height];
        isHeightEvenNumber = height % 2 == 0;
        howManyToCheck = 1;
        countFirstDirRepeats = 0;
        MakeFullGrid();
    }

    private void Update()
    {
        if (spawn) Spawn(widthToCheck, heightToCheck);
        if (clear) Clear();
    }

    void LoadSize()
    {
        Size size = new();
        string path = Application.dataPath;
        Debug.Log(textJSON.text);

        if (System.IO.File.Exists(path + @"/WidthAndHeight.txt"))
        {
            string jsonText = System.IO.File.ReadAllText(path + @"/WidthAndHeight.txt");
            size = JsonUtility.FromJson<Size>(jsonText);
        }
        else
        {
            width = 50;
            height = 50;
        }
        width = size.width;
        height = size.height;
    }
    void MakeFullGrid()
    {
        Vector2 midPosition = new(width / 2, height / 2);
        float tempWidth = width;
        float tempHeihgt = height;
        for (int i = 0; i < width; i++)
        {
            float posX = -tempWidth / 2 + i + 0.5f;
            for (int z = 0; z < height; z++)
            {
                float posY = tempHeihgt / 2 - z - 0.5f;
                Vector3 position = new(posX, posY, 0);

                int randomNumber = Random.Range(0, 4);
                GameObject slotObject;
                int state;
                if (new Vector2(i, z) == midPosition)
                {
                    spawner.transform.position = new(posX, posY, 0);
                    widthToCheck = i;
                    heightToCheck = z;
                    if (isEven) slotObject = Instantiate(emptyGrid1, position, Quaternion.identity);
                    else slotObject = Instantiate(emptyGrid2, position, Quaternion.identity);
                    state = 2;
                }
                else if (randomNumber == 3)
                {
                    slotObject = Instantiate(blockedSlot, position, Quaternion.identity);
                    state = 1;
                }
                else
                {
                    if (isEven) slotObject = Instantiate(emptyGrid1, position, Quaternion.identity);
                    else slotObject = Instantiate(emptyGrid2, position, Quaternion.identity);
                    state = 0;
                }
                Slot slotScript;
                if (slotObject.GetComponent<Slot>() != null)
                {
                    slotScript = slotObject.GetComponent<Slot>();
                }
                else
                {
                    slotObject.AddComponent<Slot>();
                    slotScript = slotObject.GetComponent<Slot>();
                }
                slotScript.positionOnGrid = new(i, z);
                slotScript.state = state;
                allSlots[i, z] = slotScript;
                if (state == 2) spawnSlot = slotScript;
                //slotList.Add(slotScript);
                isEven = !isEven;
            }
            if (isHeightEvenNumber) isEven = !isEven;
        }
        //Vector2 midPosition = new(width / 2, height / 2);

    }

    void Spawn(int lastWidth, int lastHeight)
    {
        int nextWidth = lastWidth;
        int nextHeight = lastHeight;

        Slot nextEmptySlot = null;
        while (nextEmptySlot == null)
        {
            switch (direction)
            {
                case 0:
                    {
                        nextHeight -= 1;
                        if (nextHeight < 0) noEmptyUp = true;
                        break;
                    }
                case 1:
                    {
                        nextWidth += 1;
                        if (nextWidth > width - 1) noEmptyOnRight = true;
                        break;
                    }
                case 2:
                    {
                        nextHeight += 1;
                        if (nextHeight > height - 1) noEmptyDown = true;
                        break;
                    }
                case 3:
                    {
                        nextWidth -= 1;
                        if (nextWidth < 0) noEmptyOnLeft = true;
                        break;
                    }
            }
            howManyChecked++;
            nextEmptySlot = CheckIfEmpty(nextWidth, nextHeight);
            if (howManyChecked >= howManyToCheck)
            {
                if (direction == 1  || direction == 3) howManyToCheck++;
                direction++;
                howManyChecked = 0;
                if (direction > 3) direction = 0;
            }
            if (noEmptyDown && noEmptyOnLeft && noEmptyOnRight && noEmptyUp) break;
        }
        if (!(noEmptyDown && noEmptyOnLeft && noEmptyOnRight && noEmptyUp))
        {
            int randomColor = Random.Range(0, 3);
            GameObject circleObject;
            switch (randomColor)
            {
                case 0:
                    {
                        nextEmptySlot.state = 3;
                        circleObject = Instantiate(colorCircle1, spawner.transform.position, Quaternion.identity);
                        break;
                    }
                case 1:
                    {
                        nextEmptySlot.state = 4;
                        circleObject = Instantiate(colorCircle2, spawner.transform.position, Quaternion.identity);
                        break;
                    }
                default:
                    {
                        nextEmptySlot.state = 5;
                        circleObject = Instantiate(colorCircle3, spawner.transform.position, Quaternion.identity);
                        break;
                    }
            }
            Circles circleScript;
            if (circleObject.GetComponent<Circles>()) circleScript = circleObject.GetComponent<Circles>();
            else
            {
                circleObject.AddComponent<Circles>();
                circleScript = circleObject.GetComponent<Circles>();
            }
            circleScript.positionOnGrid = nextEmptySlot.transform.position;
            nextEmptySlot.circle = circleScript;
            widthToCheck = nextWidth;
            heightToCheck = nextHeight;
        }
    }

    Slot CheckIfEmpty(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            if (allSlots[x, y].state == 0) return allSlots[x, y];
            else return null;
        }
        else return null;
    }

    void Clear()
    {
        List<Slot> slotWithCirclesToDestroy = new();
        for (int i = 0; i < width; i++) 
        {
            for (int z = 0; z < height; z++) 
            {
                if (allSlots[i, z].circle != null)
                {
                    bool addedOnce = false;
                    if (allSlots[i, z].state > 2)
                    {
                        if (i < width - 1)
                        {
                            if (allSlots[i + 1, z].state == allSlots[i, z].state)
                            {
                                slotWithCirclesToDestroy.Add(allSlots[i + 1, z]);
                                if (!allSlots[i, z].circle.deathMark) slotWithCirclesToDestroy.Add(allSlots[i, z]);
                                allSlots[i + 1, z].circle.deathMark = true;
                                allSlots[i, z].circle.deathMark = true;
                                addedOnce = true;
                            }
                        }
                    }
                    {
                        if (z < height - 1)
                        {
                            if (allSlots[i, z + 1].state == allSlots[i, z].state)
                            {
                                slotWithCirclesToDestroy.Add(allSlots[i, z + 1]);
                                allSlots[i, z + 1].circle.deathMark = true;
                                if (!addedOnce && !allSlots[i, z].circle.deathMark) slotWithCirclesToDestroy.Add(allSlots[i, z]);
                                allSlots[i, z].circle.deathMark = true;
                            }
                        }
                    }
                }
            }
        }
        foreach (Slot slot in slotWithCirclesToDestroy)
        {
            slot.DestroyCircle();
        }
        slotWithCirclesToDestroy.Clear();
        ResetVariables();
        widthToCheck = (int)spawnSlot.positionOnGrid.x;
        heightToCheck = (int)spawnSlot.positionOnGrid.y;
        clear = false;
    }

    public void ResetVariables()
    {
        direction = 0;
        howManyToCheck = 1;
        howManyChecked = 0;
        countFirstDirRepeats = 0;
        countSecoundDirRepeats = 0;
        countThirdDirRepeats = 0;
        countForthDirRepeats = 0;
        noEmptyOnRight = false;
        noEmptyOnLeft = false;
        noEmptyDown = false;
        noEmptyUp = false;
    }
}


[System.Serializable]
public class Size
{
    public int width;
    public int height;
}
