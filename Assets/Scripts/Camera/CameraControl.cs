using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public new CinemachineVirtualCamera camera;
    private CinemachineConfiner2D confiner;

    public GameObject cursor;

    private float minOrthoSize;
    private float maxOrthoSize;
    // Start is called before the first frame update
    void Awake()
    {
        minOrthoSize = camera.m_Lens.OrthographicSize;
        maxOrthoSize = 20;

        cursor.SetActive(true);
        confiner = camera.GetComponent<CinemachineConfiner2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.mouseScrollDelta.y != 0)
        {
            Zoom();
        }
    }

    private void Zoom()
    {
        if(Input.mouseScrollDelta.y > 0)
        {
            if(camera.m_Lens.OrthographicSize - 0.25f < minOrthoSize)
            {
                camera.m_Lens.OrthographicSize = minOrthoSize;
            }
            else
            {
                camera.m_Lens.OrthographicSize -= 0.25f;
            }
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            if (camera.m_Lens.OrthographicSize + 0.25f > maxOrthoSize)
            {
                camera.m_Lens.OrthographicSize = maxOrthoSize;
            }
            else
            {
                camera.m_Lens.OrthographicSize += 0.25f;
            }
        }

        confiner.InvalidateCache();
    }

    
}
