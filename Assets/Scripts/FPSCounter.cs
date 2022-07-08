using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    public int avgFrameRate;
    public TextMeshProUGUI display_Text;
    int frames;
    float timer;
    float temp;

    public void Update()
    {
        frames++;
        timer += Time.deltaTime;
        if (timer > 1f)
        {
            temp = frames / timer;
            avgFrameRate =(int) temp;
            display_Text.text = avgFrameRate.ToString() + " FPS";
            timer = 0;
            frames = 0;
        }
    }
}
