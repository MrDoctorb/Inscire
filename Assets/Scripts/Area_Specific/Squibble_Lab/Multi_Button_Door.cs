using System.Collections;
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

    void OnEnable()
    {        
        saveKey = gameObject.name + " " + transform.position.x + " " + transform.position.y;
    }

    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        text = GetComponent<Display_Text>();
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
                text.text[0] = "The door seems to be locked. There must be a way to open it.";
                break;
            }
            else
            {
                if (currentValue != 2)
                {
                    currentValue = 1;
                }
                if (!text.dialogue)
                {
                    text.text[0] = "Click!";
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
