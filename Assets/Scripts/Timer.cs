using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public UnityEngine.UI.Image img;
    public static float timeStart = 1200;
    public static int day = 1;
    public int speed;
    public Text textBox;
    public Text textBox2;
    public Color Morning;
    public Color Morning1;
    public Color Morning2;
    public Color Noon;
    public Color Evening1;
    public Color Evening2;
    public Color Night;
    public static Timer instance;

    // Start is called before the first frame update
    void Start()
    {
        textBox.text = TimeSpan.FromMinutes(timeStart).ToString(@"hh\:mm");
        changeSky();
        textBox2.text = "Day " + day.ToString();
    }

    // Update is called once per frame
    void Update()
    { 
        
        timeStart += Time.deltaTime*speed;
        //Debug.Log(Mathf.Round(timeStart/10)*10);
        textBox.text = TimeSpan.FromMinutes(Mathf.Round(timeStart/10)*10).ToString(@"hh\:mm");
        changeSky();
        if(Mathf.Round(timeStart/10)*10 == 1440){
            timeStart = 0;
            day++;
            textBox2.text = "Day " + day.ToString();
        }

    }

    public void GetTime(out float time, out int currDay)
    {
        time = timeStart;
        currDay = day;
    }

    public void SetTime(float Time, int Day)
    {
        timeStart = Time;
        day = Day;
    }

    void changeSky()
    {
        var t = Mathf.Round(timeStart);

        //06:00 - 09:00
        if (t >= 360 && t < 540)
        {
            img.color = Morning1;
        }
        else
        {
            //09:00 - 12:00
            if (t >= 540 && t < 720)
            {
                img.color = Morning2;
            }
            else
            {
                //12:00 - 18:00
                if (t >= 720 && t < 1080)
                {
                    img.color = Noon;
                }
                else
                {
                    //18:00 - 20:00
                    if (t >= 1080 && t < 1200)
                    {
                        img.color = Evening1;
                    }
                    else
                    {
                        //20:00 - 22:00
                        if (t >= 1200 && t < 1320)
                        {
                            img.color = Evening2;
                        }
                        else
                        {
                            //04:00 - 06:00
                            if (t >= 240 && t < 360)
                            {
                                img.color = Morning;
                            }
                            else
                            {
                                img.color = Night;
                            }
                            
                        }
                    }
                }
            }
        }
    }
}
