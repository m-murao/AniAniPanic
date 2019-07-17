using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionUI : MonoBehaviour
{
    private void Awake()
    {
        var text = GetComponent<TMPro.TextMeshProUGUI>();
        if (text)
        {
            text.SetText($"Ver {Application.version}");
        }
        Destroy(this);
    }
}
