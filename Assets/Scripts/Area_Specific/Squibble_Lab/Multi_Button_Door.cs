﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multi_Button_Door : MonoBehaviour, ITextEvent, ISaveable
{
    public Four_State_Button_Controller[] buttons;
    public int[] values;
    int currentValue;
    SpriteRenderer rend;
    [SerializeField] Sprite[] states = new Sprite[3];
    Display_Text text;
    string saveKey;

    ITextEventContainer tempEvent;

    void OnEnable()
    {        
        saveKey = gameObject.name + " " + transform.position.x + " " + transform.position.y;
    }

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        text = GetComponent<Display_Text>();
        tempEvent = text.textInfo[0].textEvents[0];
    }

    public void SaveData(Scene_Data data)
    {
        data.Store<object>(saveKey, currentValue);
    }

    public void LoadData(Scene_Data data)
    {
        currentValue = (int)data.Take<object>(saveKey);
        if (currentValue == 2)
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    void Update()
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].health != values[i] && currentValue != 2)
            {
                currentValue = 0;
                text.textInfo[0].text = "The door seems to be locked. There must be a way to open it.";
                text.textInfo[0].textEvents = new ITextEventContainer[0];
                break;
            }
            else
            {
                if (currentValue != 2)
                {
                    currentValue = 1;
                }
                if (!text.textInfo[0].dialogue)
                {
                    text.textInfo[0].text = "Click!";
                    text.textInfo[0].textEvents = new ITextEventContainer[1];
                    text.textInfo[0].textEvents[0] = tempEvent;
                }
            }
        }
        rend.sprite = states[currentValue];
    }

    public void TextFinished()
    {
        if (currentValue >= 1)
        {
            currentValue = 2;
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
