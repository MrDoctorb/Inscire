using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Hide_Room : MonoBehaviour
{
    [Range(0f, 2f)] public float brightness;
    float fadeSpeed = .015f;
    public bool stillCam;
    public Vector3 camPos;
    float fade = 0, startA;
    SpriteRenderer hide;
    Camera_Controller cam;
    Light2D worldLight;
    static GameObject headed;
    IEnumerator Start()
    {
        hide = GetComponent<SpriteRenderer>();
        hide.enabled = true;
        cam = GameObject.Find("Camera").GetComponent<Camera_Controller>();
        worldLight = GameObject.Find("World_Light").GetComponent<Light2D>();
        startA = hide.color.a;
        yield return new WaitForEndOfFrame();
        if (GetComponent<BoxCollider2D>().IsTouching(ZaneSpace.Info.mc.GetComponent<CapsuleCollider2D>()))
        {
            worldLight.intensity = brightness;
        }
    }

    void Update()
    {
        if (hide.color.a <= startA)
        {
            hide.color += new Color(0, 0, 0, fade);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other == ZaneSpace.Info.mc.GetComponent<CapsuleCollider2D>())
        {
            fade = -.03f;
            hide.color = new Color(0, 0, 0, startA);
            headed = gameObject;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other == ZaneSpace.Info.mc.GetComponent<CapsuleCollider2D>())
        {
            fade = .03f;
            hide.color = new Color(0, 0, 0, 0);
            if (headed != gameObject)
            {
                StartCoroutine(ChangeBrightness());
                cam.still = headed.GetComponent<Hide_Room>().stillCam;
                cam.slidePos = headed.GetComponent<Hide_Room>().camPos;
            }
        }
    }

    IEnumerator ChangeBrightness()
    {
        float headLight = headed.GetComponent<Hide_Room>().brightness;
        if (worldLight.intensity < headLight)
        {
            while (worldLight.intensity <= headLight)
            {
                worldLight.intensity += fadeSpeed;
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (worldLight.intensity >= headLight)
            {
                worldLight.intensity -= fadeSpeed;
                yield return new WaitForEndOfFrame();
            }
        }
    }
}