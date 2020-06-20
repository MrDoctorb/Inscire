using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Startup_Screen : MonoBehaviour
{
    void Start()
    {
        foreach (Image image in GetComponentsInChildren<Image>())
        {
            image.CrossFadeAlpha(1, 3, true);
        }
        GetComponentInChildren<Text>().CrossFadeAlpha(1, 3, true);
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
