using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZaneSpace;
using UnityEditor;

public class MC_Controller : MonoBehaviour, IDamageable, IKnockbackable
{ 
    Health healthDisplay;
    public static int maxHealth = 3;
    public int _health = maxHealth;
    [HideInInspector] public bool invincible = false;
    public List<GameObject>[] unlockedParts = new List<GameObject>[5];
    [SerializeField]
    List<GameObject> unLeft = new List<GameObject>(),
    unRight = new List<GameObject>(), unCaudal = new List<GameObject>(),
    unDorsal = new List<GameObject>(), unEyes = new List<GameObject>();


    public int health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
            healthDisplay.ChangeHealth();
        }
    }

    List<SpriteRenderer> renderers = new List<SpriteRenderer>(0);

    void Start()
    {
        unlockedParts[0] = unLeft;
        unlockedParts[1] = unRight;
        unlockedParts[2] = unCaudal;
        unlockedParts[3] = unDorsal;
        unlockedParts[4] = unEyes;
        StartCoroutine(UpdateRenderers());
        healthDisplay = Info.gm.transform.GetChild(0).GetChild(0).GetComponent<Health>();
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown("`"))
        {
            transform.position = SceneView.lastActiveSceneView.camera.transform.position;
        }
#endif
        foreach (SpriteRenderer rend in renderers)
        {
            float temp = transform.position.y * -100;
            rend.sortingOrder = (int)temp;
            if (rend != renderers[0])
            {
                rend.sortingOrder += 1;
            }
        }
    }

    public IEnumerator UpdateRenderers()
    {
        yield return new WaitForEndOfFrame();
        renderers = new List<SpriteRenderer>();
        for (int i = 0; i < 6; i++)
        {
            renderers.Add(transform.GetChild(i).GetComponent<SpriteRenderer>());
        }
    }

    public void Knockback(Vector2 direction, float force)
    {
        GetComponent<Rigidbody2D>().velocity += direction * force;
        StartCoroutine(Invincibility());
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        if (health < 0)
        {
            health = 0;
        }
    }

    IEnumerator Invincibility()
    {
        invincible = true;
        for (int i = 0; i < 8; i++)
        {
            yield return Wait.WaitMySeconds(.25f);
            foreach (SpriteRenderer rend in renderers)
            {
                rend.enabled = !rend.enabled;
            }
        }
        invincible = false;
    }

    public void DealDamage(List<IDamageable> dmgList, int dmg)
    {
        foreach (IDamageable target in dmgList)
        {
            target.TakeDamage(dmg);
            if (target is IKnockbackable)
            {
                IKnockbackable tempTarget = (IKnockbackable)target;
                Vector2 direction = (tempTarget.gameObject.transform.position - transform.position) / (tempTarget.gameObject.transform.position - transform.position).magnitude;
                tempTarget.Knockback(direction, 10);
            }
        }
    }

    public void Heal(int amount = -1)
    {
        health += amount;
        if (health > maxHealth || amount == -1)
        {
            health = maxHealth;
        }
    }

    int currentPart = 0;
    public void LoadSavedParts(List<int> partIndexes)
    {
        currentPart = 0;
        foreach (int partIndex in partIndexes)
        {
            ReplacePart(partIndex);
        }
        StartCoroutine(UpdateRenderers());
    }

    void ReplacePart(int index)
    {
        Destroy(transform.GetChild(currentPart + 1).gameObject);
        GameObject newPart = Instantiate(unlockedParts[currentPart][index], transform.position, transform.rotation);
        newPart.transform.parent = transform;
        newPart.name = unlockedParts[currentPart][index].name;
        currentPart++;
    }


}
