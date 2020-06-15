using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset_Button : MonoBehaviour, IInteractable
{

#pragma warning disable 0649
    [SerializeField] GameObject[] buttons;
#pragma warning restore 1234

    public void Interact()
    {
        foreach (GameObject button in buttons)
        {
            button.SendMessage("Reset");
            GetComponent<Animator>().SetTrigger("Push");
        }
    }
}
