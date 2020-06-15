using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZaneSpace;

public class Inventory_Menu : MonoBehaviour
{
    Animator anm;
    Text[] allChild;
    GameObject selectBar;
    int options; //number of different selections. Length of allChild
    int currentSelection;
    [System.NonSerialized] public float fadeTime = .25f;
    [System.NonSerialized] public bool submenu = false;
    void OnEnable()
    {
        Info.worldPause();
        anm = GetComponent<Animator>();
        allChild = transform.GetComponentsInChildren<Text>();
        options = allChild.Length;
        selectBar = transform.parent.GetChild(1).gameObject;
        selectBar.GetComponent<Image>().enabled = false;
        StartCoroutine(WaitTillOpen());
    }

    void Update()
    {

        if (!submenu)
        {
            if (Input.GetButtonDown("Secondary") || Input.GetButtonDown("Inventory"))
            {
                StartCoroutine(CloseBook());
            }
            else if (Input.GetKeyDown("w"))
            {
                currentSelection += options - 1;
                MoveSelectionBar();
            }
            else if (Input.GetKeyDown("s"))
            {
                currentSelection += 1;
                MoveSelectionBar();
            }
            else if (Input.GetButtonDown("Primary"))
            {
                submenu = true;
                Fade(allChild, new Image[0], 0);
                selectBar.GetComponent<Image>().enabled = false;
                allChild[currentSelection].transform.GetChild(0).gameObject.SetActive(true);
            }
        }
        else if (Input.GetButtonDown("Inventory"))
        {
            try{allChild[currentSelection].transform.GetChild(0).SendMessage("CloseBook");}
            catch{StartCoroutine(CloseBook());}
            submenu = true;
        }
    }

    void MoveSelectionBar()
    {
        currentSelection %= options;
        selectBar.transform.localPosition = new Vector2(selectBar.transform.localPosition.x,
        allChild[currentSelection].transform.localPosition.y * 1.5f);
    }

    public void Fade(Text[] texts, Image[] images, float alpha)
    {
        foreach (Text comp in texts)
        {
            comp.CrossFadeAlpha(alpha, fadeTime, false);
        }
        foreach (Image comp in images)
        {
            comp.CrossFadeAlpha(alpha, fadeTime, false);
        }
    }

    IEnumerator WaitTillOpen()
    {
        submenu = true;
        while (!anm.IsInTransition(0))
        {
            yield return new WaitForEndOfFrame();
        }
        Fade(allChild, new Image[0], 1f);
        selectBar.GetComponent<Image>().enabled = true;
        submenu = false;
    }

    IEnumerator WaitTillClosed()
    {
        while (!anm.IsInTransition(0))
        {
            yield return new WaitForEndOfFrame();
        }
        Info.worldPause();
        submenu = false;
        transform.parent.gameObject.SetActive(false);
    }

    public IEnumerator CloseBook()
    {
        submenu = true;
        Fade(allChild, new Image[0], 0);
        selectBar.GetComponent<Image>().enabled = false;
        yield return new WaitForSeconds(fadeTime);
        StartCoroutine(WaitTillClosed());
        anm.Play("Close_Book");
    }

    public void ExitSubmenu()
    {
        Fade(allChild, new Image[0], 1);
        selectBar.GetComponent<Image>().enabled = true;
        submenu = false;
    }
}
