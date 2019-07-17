using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountUI : MonoBase
{
    [SerializeField] private TextMeshProUGUI text;

    void Update()
    {
        if (GameManager.Instance.IsCountState)
        {
            text.enabled = true;
            text.SetText(GameManager.Instance.CountTime.ToString());
        }
        else
        {
            text.enabled = false;
        }
    }
}
