using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixCanvasReorderingBug : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Canvas>().sortingOrder++;
        GetComponent<Canvas>().sortingOrder--;
    }
}
