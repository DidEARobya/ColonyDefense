using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterPathDisplay : MonoBehaviour
{
    private CharacterControl character;
    private AIPathCustom aiPath;
    private LineRenderer pathLine;

    void Start()
    {
        if(this.transform.GetComponent<CharacterControl>() == null)
        {
            return;
        }
        if (this.transform.GetComponent<AIPathCustom>() == null)
        {
            return;
        }

        character = this.transform.GetComponent<CharacterControl>();
        aiPath = this.transform.GetComponent<AIPathCustom>();

        if (pathLine == null)
        {
            pathLine = this.AddComponent<LineRenderer>();

            if (Resources.Load("Materials/PathLineMaterial") != null)
            {
                Object temp = Resources.Load("Materials/PathLineMaterial");
                pathLine.material = temp as Material;
            }

            pathLine.startColor = Color.white;
            pathLine.endColor = Color.white;

            pathLine.startWidth = 0.1f;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (aiPath == null)
        {
            return;
        }

        if (character.isSelected == true)
        {
            UpdatePathDisplay();
        }
        else
        {
            if (pathLine.positionCount != 0)
            {
                pathLine.positionCount = 0;
            }
        }
    }

    private void UpdatePathDisplay()
    {
        if(aiPath.hasPath)
        {
            pathLine.positionCount = aiPath.GetPathLength();

            for(int i = 0; i < aiPath.GetPathLength(); i++)
            {
                pathLine.SetPosition(i, aiPath.GetWaypointPos(i));
            }
        }
    }
    private void OnDestroy()
    {
        Destroy(pathLine.material);
    }
}
