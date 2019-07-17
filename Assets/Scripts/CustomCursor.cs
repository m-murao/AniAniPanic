using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBase
{
    private readonly Vector3 MouseOffset = new Vector3(70, -30, 0);

    private void Start()
    {
        Cursor.visible = false;
    }

    private void OnDestroy()
    {
        Cursor.visible = true;
    }

    private void Update()
    {
        transform.position = Input.mousePosition + MouseOffset;

        if (Input.GetMouseButtonDown(0)) transform.localRotation = Quaternion.Euler(0, 0, 25);
        if (Input.GetMouseButtonUp  (0)) transform.localRotation = Quaternion.identity;
    }
}
