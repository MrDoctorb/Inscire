using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    MC_Controller mc;
    int hp, maxHP;
    float percent, height;
    public GameObject health, wave, tank, particles;

    void Start()
    {
        mc = ZaneSpace.Info.mc.GetComponent<MC_Controller>();
        hp = mc.health;
        maxHP = MC_Controller.maxHealth;
        height = health.GetComponent<RectTransform>().sizeDelta.y;
    }
    public void ChangeHealth()
    {
        hp = mc.health;
        percent = (float)hp / maxHP;
        health.GetComponent<RectTransform>().localScale = new Vector3(1, percent, 1);
        health.GetComponent<RectTransform>().localPosition = tank.GetComponent<RectTransform>().localPosition + new Vector3(0, ((-1 + percent) / 2 * 73 + 1.5f), 0);
        wave.GetComponent<RectTransform>().localPosition = tank.GetComponent<RectTransform>().localPosition + new Vector3(0, ((-1 + percent) * 73 + 32), 0);
        if (mc.health == 0)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        ParticleSystem system = particles.GetComponent<ParticleSystem>();
        system.Play();
        yield return new WaitForEndOfFrame();
        system.startColor = Color.red;
        float i = 0;
        while (i < 3)
        {
            system.Emit(1);
            i += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }


    }
}

