using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public UnityEngine.UI.Image img;
    public float timeStart;
    public int day;
    public Text textBox;
    public Text textBox2;
    public Color Morning;
    public Color Morning2;
    public Color Noon;
    public Color Evening1;
    public Color Evening2;
    public Color Night;
    // Start is called before the first frame update
    void Start()
    {
        textBox.text = TimeSpan.FromMinutes(timeStart).ToString(@"hh\:mm");
        textBox2.text = "Day " + day.ToString();
    }

    // Update is called once per frame
    void Update()
    { 
        
        timeStart += Time.deltaTime*5;
        Debug.Log(Mathf.Round(timeStart/10)*10);
        textBox.text =TimeSpan.FromMinutes(Mathf.Round(timeStart/10)*10).ToString(@"hh\:mm");
        changeSky();
        if(Mathf.Round(timeStart/10)*10 == 1440){
            timeStart = 0;
            day++;
            textBox2.text = "Day " + day.ToString();
        }

    }

    void changeSky()
    {
        switch (Mathf.Round(timeStart))
        {
          case 360: //06:00
              img.color = Morning;
              break;
          case 540: //09:00
              img.color = Morning2;
              break;
          case 720: //12:00
              img.color = Noon;
              break;
          case 1080:// 18:00
              img.color = Evening1;
              break;     
          case 1200:// 20:00
              img.color = Evening2;
              break;     
          case 1320:// 22:00
              img.color = Night;
              break;     
          default:
              break;
        }


    }
}
