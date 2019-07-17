using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointUI : MonoBase
{
    [SerializeField] private TextMeshProUGUI text;

    void Update()
    {
        text.SetText(GameManager.Instance.GamePoint.ToString());
    }
}
