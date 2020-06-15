using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZaneSpace;

public class Unlocked_Parts : MonoBehaviour
{
    MC_Controller mc;
    Inventory_Menu inv;
    GameObject selectBar, partImage;
    Text partInfo, partName;
    int _bodyTypeSelection, _sameTypeSelection;
    int bodyTypeSelection
    {
        get { return _bodyTypeSelection; }
        set
        {
            _bodyTypeSelection = value;
            _bodyTypeSelection %= 5;

            selectBar.transform.localPosition =
            new Vector2(selectBar.transform.localPosition.x, transform.GetChild(_bodyTypeSelection).localPosition.y);

            sameTypeSelection = 0;
            Vector2 tempLoc = new Vector2();
            switch (_bodyTypeSelection)
            {
                case 0:
                    tempLoc = new Vector2(20, 7);
                    partImage.GetComponent<RectTransform>().localScale = new Vector2(-2.5f, 2.5f);
                    break;
                case 1:
                    tempLoc = new Vector2(-45, 7);
                    partImage.GetComponent<RectTransform>().localScale = new Vector2(2.5f, 2.5f);
                    break;
                case 2:
                    tempLoc = new Vector2(-9, 100);
                    break;
                case 3:
                    tempLoc = new Vector2(-9, 25);
                    break;
                case 4:
                    tempLoc = new Vector2(-9, -5);
                    break;
            }
            partImage.GetComponent<RectTransform>().localPosition = tempLoc;
        }
    }
    int sameTypeSelection
    {
        get { return _sameTypeSelection; }
        set
        {
            _sameTypeSelection = value;
            _sameTypeSelection %= mc.unlockedParts[bodyTypeSelection].Count;
            GameObject currentSelectedPart = mc.unlockedParts[bodyTypeSelection][sameTypeSelection];
            //Change Image
            partImage.GetComponent<Image>().sprite = currentSelectedPart.GetComponent<SpriteRenderer>().sprite;
            //Change Name
            partName.text = currentSelectedPart.name;
            //Change Description
            partInfo.text = currentSelectedPart.GetComponent<Part_Info>().partDescription;
        }
    }

    void OnEnable()
    {
        mc = Info.mc.GetComponent<MC_Controller>();
        inv = transform.parent.parent.GetComponent<Inventory_Menu>();
        selectBar = transform.GetChild(5).gameObject;
        partImage = transform.GetChild(6).GetChild(1).gameObject;
        partName = transform.GetChild(6).GetChild(2).GetComponent<Text>();
        partInfo = transform.GetChild(6).GetChild(3).GetComponent<Text>();

        inv.Fade(GetComponentsInChildren<Text>(), transform.GetChild(6).GetComponentsInChildren<Image>(), 1);
        selectBar.GetComponent<Image>().enabled = true;

        bodyTypeSelection = bodyTypeSelection;
    }

    void Update()
    {
        if (Input.GetKeyDown("w"))
        {
            bodyTypeSelection += 4;
        }
        else if (Input.GetKeyDown("s"))
        {
            bodyTypeSelection += 1;
        }
        else if(Input.GetKeyDown("d"))
        {
            sameTypeSelection += 1;
        }
        else if(Input.GetKeyDown("a"))
        {
            sameTypeSelection += mc.unlockedParts[bodyTypeSelection].Count - 1;
        }
        else if(Input.GetButton("Secondary"))
        {
            StartCoroutine(CloseSubmenu());
        }
    }

    IEnumerator CloseSubmenu()
    {
        inv.Fade(GetComponentsInChildren<Text>(), transform.GetChild(6).GetComponentsInChildren<Image>(), 0);
        yield return new WaitForSeconds(inv.fadeTime);
        selectBar.GetComponent<Image>().enabled = false;
        inv.ExitSubmenu();
        gameObject.SetActive(false);
    }

    public IEnumerator CloseBook()
    {
        inv.Fade(GetComponentsInChildren<Text>(), new Image[0], 0);
        StartCoroutine(inv.CloseBook());
        yield return new WaitForSeconds(inv.fadeTime);
        selectBar.GetComponent<Image>().enabled = false;
        gameObject.SetActive(false);
        inv.submenu = false;
    }
}
