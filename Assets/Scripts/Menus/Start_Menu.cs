using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ZaneSpace;

public class Start_Menu : MonoBehaviour
{
    public Sprite[] buttons;
    int selected = 0;
    bool finished, submenu;
    GameObject playMenu, currentMenu;

    void OnEnable()
    {
        playMenu = GameObject.Find("Play_Menu");
        currentMenu = gameObject;
        transform.GetChild(selected).GetComponent<Image>().sprite = buttons[1];
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/Saves");
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/Saves/Temp");
    }

    void Update()
    {
        if (Input.GetButtonDown("Primary") && !finished)
        {
            if (currentMenu == playMenu)
            {
                switch (selected)
                {
                    case 0:
                        finished = true;
                        StartCoroutine(StartGame());
                        break;
                    case 1:
                        finished = true;
                        Save_Manager.ClearAllData();
                        StartCoroutine(StartGame());
                        break;
                    case 2:
                        currentMenu.transform.GetChild(selected).GetComponent<Image>().sprite = buttons[0];
                        currentMenu = gameObject;
                        currentMenu.transform.GetChild(selected).GetComponent<Image>().sprite = buttons[1];
                        GetComponent<Canvas>().enabled = true;
                        break;
                }
            }
            else
            {
                switch (selected)
                {
                    case 0:
                        currentMenu.transform.GetChild(selected).GetComponent<Image>().sprite = buttons[0];
                        currentMenu = playMenu;
                        currentMenu.transform.GetChild(selected).GetComponent<Image>().sprite = buttons[1];
                        GetComponent<Canvas>().enabled = false;
                        break;
                    case 1:
                        //Feedback
                        Application.OpenURL("https://forms.gle/rMBKBSzggKiAMFjK8");
                        break;
                    case 2:
                        finished = true;
                        finished = false;
                        Application.Quit();
                        break;
                }
            }
        }
        else if (Input.GetKeyDown("a"))
        {
            currentMenu.transform.GetChild(selected).GetComponent<Image>().sprite = buttons[0];
            selected += 2;
            selected %= 3;
            currentMenu.transform.GetChild(selected).GetComponent<Image>().sprite = buttons[1];
        }
        else if (Input.GetKeyDown("d"))
        {
            currentMenu.transform.GetChild(selected).GetComponent<Image>().sprite = buttons[0];
            selected += 1;
            selected %= 3;
            currentMenu.transform.GetChild(selected).GetComponent<Image>().sprite = buttons[1];
        }
    }

    IEnumerator StartGame()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MC_Startup", LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        Info.mc.transform.position = new Vector2(0, -7);
        SceneManager.UnloadSceneAsync("MC_Startup");
        SceneManager.LoadScene("Squibble_Lab");
        Save_Manager.LoadPlayerData();
    }
}
