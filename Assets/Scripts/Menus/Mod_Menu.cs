using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Mod_Menu : MonoBehaviour
{
    [SerializeField] Text text;
    GameObject[] buttons = new GameObject[5];
    MC_Controller mc;
    Transform currentBox;
    public Sprite[] states = new Sprite[3];//0 Normal, 1 Hover, 2 Select
    int selected = 0;
    int partSelected = 0;
    bool submenu = false;

    void Start()
    {
        text.text = "";
        int i = 0;
        foreach (RectTransform child in GameObject.Find("Buttons").GetComponent<RectTransform>())
        {
            buttons[i] = child.gameObject;
            i++;
        }
        buttons[selected].GetComponent<Image>().sprite = states[1];
        mc = ZaneSpace.Info.mc.GetComponent<MC_Controller>();
        print(mc);
        mc.GetComponent<Move>().enabled = false;
        mc.GetComponent<Attack>().enabled = false;
        mc.GetComponent<Inventory>().enabled = false;
        mc.transform.eulerAngles = Vector3.zero;
        mc.transform.position = new Vector2(1, 0);
        ZaneSpace.Info.gm.transform.GetChild(0).gameObject.SetActive(false);


    }

    void Update()
    {
        StartCoroutine(mc.UpdateRenderers());
        if (!submenu)
        {
            if (Input.GetKeyDown("w"))
            {
                buttons[selected].GetComponent<Image>().sprite = states[0];
                selected += 4;
                selected %= 5;
                buttons[selected].GetComponent<Image>().sprite = states[1];
            }
            else if (Input.GetKeyDown("s"))
            {
                buttons[selected].GetComponent<Image>().sprite = states[0];
                selected++;
                selected %= 5;
                buttons[selected].GetComponent<Image>().sprite = states[1];
            }
            else if (Input.GetButtonDown("Primary"))
            {
                StartCoroutine(Select());
            }
            else if (Input.GetButtonDown("Secondary"))
            {
                ResetPlayer(new Vector2(-13,39), -90, "Squibble_Lab");
            }
        }
        else
        {
            currentBox.GetChild(1).GetComponent<Image>().sprite = mc.transform.GetChild(selected + 1).GetComponent<SpriteRenderer>().sprite;
            currentBox.GetChild(4).GetComponent<Text>().text = mc.transform.GetChild(selected + 1).name;
            if (Input.GetKeyDown("d"))
            {
                //Move Selection Right
                partSelected += 1;
                partSelected %= mc.unlockedParts[selected].Count;
                ReplacePart();
            }
            else if (Input.GetKeyDown("a"))
            {
                //Move Selection Left
                partSelected += mc.unlockedParts[selected].Count - 1;
                partSelected %= mc.unlockedParts[selected].Count;
                ReplacePart();
            }
            else if (Input.GetButtonDown("Primary") || Input.GetButtonDown("Secondary"))
            {
                Deselect();
            }
        }
    }

    void ReplacePart()
    {
        Destroy(mc.transform.GetChild(selected + 1).gameObject);
        GameObject newPart = Instantiate(mc.unlockedParts[selected][partSelected], mc.transform.position, Quaternion.identity);
        newPart.name = mc.unlockedParts[selected][partSelected].name;
        newPart.transform.parent = mc.transform;
        newPart.transform.SetSiblingIndex(selected + 1);

        text.text = newPart.GetComponent<Part_Info>().partDescription;
    }
    IEnumerator Select()
    {
        ReplacePart();
        submenu = true;
        Animator anm = buttons[selected].transform.GetChild(1).GetComponent<Animator>();
        anm.Play("Open_Menu");
        while (!anm.IsInTransition(0))
        {
            yield return null;
        }
        anm.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        currentBox = buttons[selected].transform.GetChild(1).GetChild(0);
        currentBox.GetChild(1).GetComponent<Image>().sprite = mc.transform.GetChild(selected + 1).GetComponent<SpriteRenderer>().sprite;
        currentBox.GetChild(4).GetComponent<Text>().text = mc.transform.GetChild(selected + 1).name;
        partSelected = mc.unlockedParts[selected + 1].IndexOf(mc.transform.GetChild(selected + 1).gameObject);
    }

    void Deselect()
    {
        Animator anm = buttons[selected].transform.GetChild(1).GetComponent<Animator>();
        anm.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        anm.Play("Close_Menu");
        submenu = false;
        text.text = "";
    }

    void ResetPlayer(Vector2 pos, int rot, string scene)
    {
        mc = ZaneSpace.Info.mc.GetComponent<MC_Controller>();
        mc.GetComponent<Move>().enabled = true;
        mc.GetComponent<Attack>().enabled = true;
        mc.GetComponent<Inventory>().enabled = true;
        mc.transform.eulerAngles = new Vector3(0, 0, rot);
        mc.transform.position = pos;
        ZaneSpace.Info.gm.transform.GetChild(0).gameObject.SetActive(true);
        SceneManager.LoadScene(scene);
    }

}
