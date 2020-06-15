using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial_Controller : MonoBehaviour
{
    Inventory_Menu inv;
    int selected = 0;
    Image[] images;
    Text[] allText;
    Image[] arrows = new Image[2];
    void OnEnable()
    {
        inv = transform.parent.parent.GetComponent<Inventory_Menu>();
        transform.GetChild(selected).gameObject.SetActive(true);
        images = transform.GetChild(selected).GetComponentsInChildren<Image>();
        allText = transform.GetChild(selected).GetComponentsInChildren<Text>();
        inv.Fade(allText, images, 1);
        arrows[0] = transform.parent.GetChild(1).GetComponent<Image>();
        arrows[1] = transform.parent.GetChild(2).GetComponent<Image>();
        inv.Fade(new Text[0], arrows, 1);
    }

    void Update()
    {
        if (Input.GetKeyDown("d"))
        {
            StartCoroutine(ChangeTutorial(1));
        }
        else if (Input.GetKeyDown("a"))
        {
            StartCoroutine(ChangeTutorial(transform.childCount - 1));
        }
        else if (Input.GetButtonDown("Secondary"))
        {
            StartCoroutine(CloseTutorial());
        }
    }

    IEnumerator ChangeTutorial(int direction)
    {
        inv.Fade(allText, images, 0);
        yield return new WaitForSeconds(inv.fadeTime);
        transform.GetChild(selected).gameObject.SetActive(false);
        selected += direction;
        selected %= transform.childCount;
        transform.GetChild(selected).gameObject.SetActive(true);
        images = transform.GetChild(selected).GetComponentsInChildren<Image>();
        allText = transform.GetChild(selected).GetComponentsInChildren<Text>();
        inv.Fade(allText, images, 1);
    }

    IEnumerator CloseTutorial()
    {
        inv.Fade(allText, images, 0);
        inv.Fade(new Text[0], arrows, 0);
        yield return new WaitForSeconds(inv.fadeTime);
        transform.GetChild(selected).gameObject.SetActive(false);
        inv.ExitSubmenu();
        gameObject.SetActive(false);
    }

    public IEnumerator CloseBook()
    {
        inv.Fade(allText, images, 0);
        inv.Fade(new Text[0], arrows, 0);
        StartCoroutine(inv.CloseBook());
        yield return new WaitForSeconds(inv.fadeTime);
        transform.GetChild(selected).gameObject.SetActive(false);
        gameObject.SetActive(false);
        inv.submenu = false;
    }
}
