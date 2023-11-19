using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    public Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;

        if (camera == null)
        {
            camera = Camera.main;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 cursorPos = camera.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = cursorPos;
    }
}