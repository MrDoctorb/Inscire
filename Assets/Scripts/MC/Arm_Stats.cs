using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ZaneSpace;

public class Arm_Stats : MonoBehaviour
{
    public List<IDamageable> dmgList;
    [SerializeField] float propulsionForce = 0, cooldownTime = 0;
    [HideInInspector] public bool readyToAttack = true;
    GameObject rechargeBar;
    Image rechargeImage;

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(new Scene(), new LoadSceneMode());
        Transform rechargeLeftOrRight = Info.gm.transform.GetChild(0).GetChild(1).GetChild((int)(transform.localScale.x + 1) / 2);
        rechargeBar = rechargeLeftOrRight.GetChild(0).gameObject;
        rechargeImage = rechargeLeftOrRight.GetChild(2).GetComponent<Image>();
        rechargeImage.sprite = GetComponent<SpriteRenderer>().sprite;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        dmgList = new List<IDamageable>();
    }

    public void Propel()
    {
        int angle = transform.parent.GetComponent<Move>().rot;
        transform.parent.GetComponent<Rigidbody2D>().velocity = propulsionForce *
        new Vector2(Mathf.Cos(Mathf.Deg2Rad * (angle + 90) * -1), Mathf.Sin(Mathf.Deg2Rad * (angle - 90) * -1));
    }

    public IEnumerator Cooldown()
    {
        readyToAttack = false;
        float timeElapsed = 0;
        while (timeElapsed < cooldownTime)
        {
            yield return new WaitForEndOfFrame();
            if (!Info.worldPaused)
            {
                timeElapsed += Time.deltaTime;
            }
            rechargeBar.GetComponent<RectTransform>().sizeDelta = new Vector2((timeElapsed / cooldownTime) * 120, 25);
        }
        readyToAttack = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Component[] comps = other.gameObject.GetComponents<Component>();
        foreach (Component comp in comps)
        {
            if (comp is IDamageable)
            {
                dmgList.Add((IDamageable)comp);
            }
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        Component[] comps = other.gameObject.GetComponents<Component>();
        foreach (Component comp in comps)
        {
            if (comp is IDamageable)
            {
                dmgList.Remove((IDamageable)comp);
            }
        }
    }
}
