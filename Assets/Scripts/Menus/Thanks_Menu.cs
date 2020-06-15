using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ZaneSpace;

public class Thanks_Menu : MonoBehaviour
{
    public Sprite[] buttons;
    int selected = 0;

    void OnEnable()
    {
        transform.GetChild(selected).GetComponent<Image>().sprite = buttons[1];
    }

    void Update()
    {
        if (Input.GetButtonDown("Primary"))
        {
                switch (selected)
                {
                    case 0:
                    //Gamejolt Link
                    print("g-GAME");
                        break;
                    case 1:
                        //Feedback Lonk

                        print("Fedd");
                        break;
                    case 2:
                        Application.Quit();
                        break;
                }
            
        }
        else if (Input.GetKeyDown("a"))
        {
            transform.GetChild(selected).GetComponent<Image>().sprite = buttons[0];
            selected += 2;
            selected %= 3;
            transform.GetChild(selected).GetComponent<Image>().sprite = buttons[1];
        }
        else if (Input.GetKeyDown("d"))
        {
            transform.GetChild(selected).GetComponent<Image>().sprite = buttons[0];
            selected += 1;
            selected %= 3;
            transform.GetChild(selected).GetComponent<Image>().sprite = buttons[1];
        }
    }
}
