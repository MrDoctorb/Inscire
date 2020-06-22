using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZaneSpace;

public class Attack : MonoBehaviour
{
    GameObject leftArm, rightArm;
    RaycastHit2D[] raycastList;

    void OnEnable()
    {
        UpdateArms();
    }

    public void UpdateArms()
    {
        StartCoroutine(ChangeArms());
    }

    IEnumerator ChangeArms()
    {
        yield return new WaitForEndOfFrame();
        leftArm = transform.GetChild(1).gameObject;
        rightArm = transform.GetChild(2).gameObject;
    }

    void Update()
    {
        if (Info.time != 0)
        {
            if (Input.GetButtonDown("Primary"))
            {
                raycastList = new RaycastHit2D[100];
                if (leftArm.GetComponent<Arm_Stats>().readyToAttack)
                {
                    if (!ArmAttack(leftArm))
                    {
                        FindInteractable();
                    }
                }
                else
                {
                    FindInteractable();
                }
            }
            else if (Input.GetButtonDown("Secondary") && rightArm.GetComponent<Arm_Stats>().readyToAttack)
            {
                raycastList = new RaycastHit2D[100];
                if (!ArmAttack(rightArm))
                {
                    rightArm.GetComponent<Arm_Stats>().Propel();
                }
            }
        }


    }

    void FindInteractable()
    {
        GetComponent<BoxCollider2D>().Cast(new Vector2(), raycastList);
        bool first = true;
        foreach (RaycastHit2D obj in raycastList)
        {
            //Stops searching for Interactables when it hits an empty part of the list
            if (obj == new RaycastHit2D())
            {
                if (first)
                {
                    leftArm.GetComponent<Arm_Stats>().Propel();
                }
                break;
            }
            Component[] comps = obj.transform.gameObject.GetComponents<Component>();
            foreach (Component comp in comps)
            {
                if (comp is IInteractable)
                {
                    //Interacts with first found object then it stops
                    IInteractable interactable = (IInteractable)comp;
                    interactable.Interact();
                    break;
                }
            }
        }
    }

    bool ArmAttack(GameObject attackingArm)
    {
        Arm_Stats arm = attackingArm.GetComponent<Arm_Stats>();
        arm.GetComponent<Animator>().SetBool("Attacking", true);
        StartCoroutine(arm.Cooldown());
        //Get all of the things in the Raycast List
        arm.GetComponent<Collider2D>().Cast(new Vector2(), raycastList);
        foreach (RaycastHit2D obj in raycastList)
        {
            if (obj == new RaycastHit2D())
            {
                break;
            }
            Component[] comps = obj.transform.GetComponents<Component>();
            foreach (Component comp in comps)
            {
                if (comp is IDamageable)
                {
                    arm.transform.GetChild(0).gameObject.SetActive(true);
                    arm.Propel();
                    return true;
                }
            }
        }
        return false;
    }
}
