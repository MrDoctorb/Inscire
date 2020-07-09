using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Startup_Screen : MonoBehaviour
{
    void Start()
    {
        //Fade In Images
        foreach (Image image in GetComponentsInChildren<Image>())
        {
            image.CrossFadeAlpha(1, 3, true);
        }
        GetComponentInChildren<Text>().CrossFadeAlpha(1, 3, true);
        //Hide Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetButtonDown("Primary"))
        {
            foreach (Image image in GetComponentsInChildren<Image>())
            {
                image.CrossFadeAlpha(0, 2, true);
            }
            GetComponentInChildren<Text>().CrossFadeAlpha(0, 2, true);
            StartCoroutine(ChangeScene());
        }
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(2);
        ZaneSpace.ZScene.Load("Main_Menu");
    }
}
