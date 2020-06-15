using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    MC_Controller mc;
    int hp, maxHP;
    float percent, height;
    public GameObject health, wave, tank;

    void Start()
    {
        mc = ZaneSpace.Info.mc.GetComponent<MC_Controller>();
        hp = mc.health;
        maxHP = MC_Controller.maxHealth;
        height = health.GetComponent<RectTransform>().sizeDelta.y;
    }
    void Update()
    {
        hp = mc.health;
        maxHP = MC_Controller.maxHealth;
        percent = (float)hp / maxHP;
        health.GetComponent<RectTransform>().localScale = new Vector3(1, percent, 1);
        health.GetComponent<RectTransform>().localPosition = tank.GetComponent<RectTransform>().localPosition + new Vector3(0, ((-1 + percent) / 2 * 73 + 1.5f), 0);
        wave.GetComponent<RectTransform>().localPosition = tank.GetComponent<RectTransform>().localPosition + new Vector3(0, ((-1 + percent) * 73 + 32), 0);
    }
}

