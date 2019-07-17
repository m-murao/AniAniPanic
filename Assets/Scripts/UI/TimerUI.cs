using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerUI : MonoBase
{
    [SerializeField] private TextMeshProUGUI text;

    void Update()
    {
        text.SetText(Mathf.FloorToInt(GameManager.Instance.RemainGameTime).ToString());
        if (GameManager.Instance.IsFinishFadeInState)
        {
            gameObject.SetActive(false);
        }
    }
}
