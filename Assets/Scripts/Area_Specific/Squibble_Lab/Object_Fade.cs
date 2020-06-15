using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Fade : MonoBehaviour, ITextEvent
{
    SpriteRenderer sprite;
    [SerializeField] float fadeSpeed = .05f;
    [SerializeField] bool fadeIn = false;
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void TextFinished()
    {
        gameObject.SetActive(true);
        StartCoroutine(Fade(fadeSpeed));
    }

    public IEnumerator Fade(float speed)
    {
        yield return new WaitForEndOfFrame();
        Color alpha = sprite.color;
        if (fadeIn)
        {
            alpha.a = 0;
            sprite.color = alpha;
            while (alpha.a < 1)
            {
                alpha.a += speed;
                sprite.color = alpha;
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (alpha.a > 0)
            {
                alpha.a -= speed;
                sprite.color = alpha;
                yield return new WaitForEndOfFrame();
            }
            gameObject.SetActive(false);
        }
        fadeIn = !fadeIn;
    }
}
