using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ZaneSpace;

public class Pause_Menu : MonoBehaviour
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
                    Info.gamePause();
                    gameObject.SetActive(false);
                    break;
                case 1:
                    //Feedback
                    Application.OpenURL("https://forms.gle/rMBKBSzggKiAMFjK8");
                    break;
                case 2:
                    Info.gamePause();
                    gameObject.SetActive(false);
                    SceneManager.sceneLoaded -= Info.gm.GetComponent<Game_Manager>().OnSceneLoaded;
                    ZScene.Load("Main_Menu");
                    Destroy(Info.mc);
                    Destroy(transform.parent.gameObject);
                    break;
            }
        }
        else if (Input.GetKeyDown("w"))
        {
            transform.GetChild(selected).GetComponent<Image>().sprite = buttons[0];
            selected += 2;
            selected %= 3;
            transform.GetChild(selected).GetComponent<Image>().sprite = buttons[1];
        }
        else if (Input.GetKeyDown("s"))
        {
            transform.GetChild(selected).GetComponent<Image>().sprite = buttons[0];
            selected += 1;
            selected %= 3;
            transform.GetChild(selected).GetComponent<Image>().sprite = buttons[1];
        }
    }
}
