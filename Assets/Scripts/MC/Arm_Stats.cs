using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Arm_Stats : MonoBehaviour
{
    public List<IDamageable> dmgList;
    [SerializeField] float propulsionForce = 0, cooldownTime = 0;
    [System.NonSerialized]public bool readyToAttack = true;

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(new Scene(), new LoadSceneMode());
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
        print(gameObject.name + " needs to chill");
        readyToAttack = false;
        yield return  ZaneSpace.Wait.WaitMySeconds(cooldownTime);
        readyToAttack = true;
        print(gameObject.name + " is ready to attack");
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
