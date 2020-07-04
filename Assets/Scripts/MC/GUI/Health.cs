using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ZaneSpace; 

public class Health : MonoBehaviour
{
    MC_Controller mc;
    int hp, maxHP;
    float percent, height;
    public GameObject health, wave, tank;
    [SerializeField]Sprite[] states = new Sprite[6];
    RectTransform deathWave;

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
        print(percent + " <- Percent    State -> " + Mathf.Abs((int)(percent/25f) - 4));
        transform.GetChild(2).GetComponent<Image>().sprite = states[Mathf.Abs((int)(percent/.25f) - 4)];
        if (mc.health == 0)
        {
            transform.GetChild(2).GetComponent<Image>().sprite = states[5];
            StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        yield return new WaitForSecondsRealtime(1);
        deathWave = transform.GetChild(3).GetComponent<RectTransform>();
        Transform cam = GameObject.Find("Camera").transform;
        float scale = transform.parent.parent.GetComponent<RectTransform>().localScale.x;
        while (deathWave.localPosition.y < -85)
        {
            deathWave.localPosition += new Vector3(0f, 10f, 0);
            yield return new WaitForEndOfFrame();
        }
        //Do death things
        Info.mc.transform.position = Info.roomStartPosition;
        Info.mc.GetComponent<MC_Controller>().Heal();
        deathWave.localPosition = new Vector2(162.5f, -315);
        AsyncOperation loadingScene =  SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
        while(!loadingScene.isDone)
        {
            yield return new WaitForEndOfFrame();
        }
        Save_Manager.LoadSceneData(SceneManager.GetActiveScene().name);
        while (deathWave.localPosition.y < 165)
        {
            deathWave.localPosition += new Vector3(0f, 10f, 0);
            yield return new WaitForEndOfFrame();
        }


    }
}

