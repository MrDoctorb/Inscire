using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class Fade_Prep : MonoBehaviour
{
    [SerializeField]float startAlpha = 0;
    void OnEnable()
    {
        GetComponent<CanvasRenderer>().SetAlpha(startAlpha);
    }
}
