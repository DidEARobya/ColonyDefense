using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using UnityEditor;
using UnityEngine;

public class SelectionScript : MonoBehaviour
{
    public new Camera camera;
    public Cinemachine.CinemachineVirtualCamera virtualCamera;
    private Cinemachine.CinemachineFramingTransposer transposer;

    public GameObject worldBackground;
    public GameObject cursor;

    public GameObject noneCursor, selectableCursor, interactableCursor;

    public CharacterControl selectedControl;
    private ISelectable selectedObject;

    private Task activeTask;

    void Awake()
    {
        if(camera == null)
        {
            camera = Camera.main;
        }

        if(virtualCamera != null)
        {
            transposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(activeTask != null)
        {
            if(selectedControl == null)
            {
                activeTask = null;
                return;
            }

            if(activeTask.IsTargetComplete() == true)
            {
                activeTask.StopTask();
                selectedControl.RemoveTask(activeTask);
                activeTask = null;
            }
        }

        if(BuildingHandler.GetStructure_Static() == null)
        {
            GetSelection();
        }
        else
        {
            if(selectedControl != null)
            {
                selectedObject.DeSelect();
                selectedObject = null;
                selectedControl = null;
            }
        }

        CursorState();
        CameraUpdate();
    }
    private void CameraUpdate()
    {
        if (selectedControl != null)
        {
            if (virtualCamera.m_Follow != selectedControl.transform)
            {
                virtualCamera.m_Follow = selectedControl.transform;
                transposer.m_DeadZoneHeight = 0.1f;
                transposer.m_DeadZoneWidth = 0.1f;
            }

            GetAction();
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                virtualCamera.m_Follow = null;
                return;
            }

            if (virtualCamera.m_Follow != cursor.transform)
            {
                virtualCamera.m_Follow = cursor.transform;
                transposer.m_DeadZoneHeight = 1.0f;
                transposer.m_DeadZoneWidth = 1.0f;
            }
        }
    }
    private void CursorState()
    {
        Vector2 rayStart = camera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hitData = Physics2D.Raycast(new Vector2(rayStart.x, rayStart.y), Vector2.zero, 0);

        if (hitData.collider == null)
        {
            return;
        }

        if (hitData.collider.GetComponent<ISelectable>() != null)
        {
            selectableCursor.SetActive(true);

            noneCursor.gameObject.SetActive(false);
            interactableCursor.gameObject.SetActive(false);
        }
        else if (selectedControl != null && hitData.collider.GetComponent<IInteractable>() != null)
        {
            interactableCursor.gameObject.SetActive(true);

            selectableCursor.SetActive(false);
            noneCursor.gameObject.SetActive(false);

        }
        else
        {
            noneCursor.gameObject.SetActive(true);

            interactableCursor.gameObject.SetActive(false);
            selectableCursor.SetActive(false);
        }
    }
    private void GetSelection()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 rayStart = camera.ScreenToWorldPoint(Input.mousePosition);

            LayerMask mask = LayerMask.GetMask("Default");
            mask += LayerMask.GetMask("Obstacle");

            RaycastHit2D hitData = Physics2D.Raycast(new Vector2(rayStart.x, rayStart.y), Vector2.zero, 0, mask);

            if (hitData.collider == null)
            {
                return;
            }

            if (hitData.collider.TryGetComponent(out ISelectable selectableObject))
            {
                if (selectedObject == selectableObject)
                {
                    return;
                }

                if (selectedObject != null)
                {
                    selectedObject.DeSelect();
                }

                if (hitData.collider.GetComponent<CharacterControl>() != null)
                {
                    selectedControl = hitData.collider.GetComponent<CharacterControl>();
                    activeTask = selectedControl.GetCurrentTask();
                }
                else
                {
                    selectedControl = null;
                    activeTask = null;
                }

                selectedObject = selectableObject;
                selectableObject.Select();
            }
            else
            {
                if (hitData.collider.gameObject == worldBackground)
                {
                    if (selectedObject == null)
                    {
                        return;
                    }

                    selectedObject.DeSelect();
                    selectedObject = null;
                    selectedControl = null;
                }
            }
        }
    }
    private void GetAction()
    {
        if (Input.GetMouseButtonUp(1))
        {
            Vector2 rayStart = camera.ScreenToWorldPoint(Input.mousePosition);

            LayerMask mask = LayerMask.GetMask("Default");
            mask += LayerMask.GetMask("Obstacle");
            mask += LayerMask.GetMask("Checkpoint");

            RaycastHit2D hitData = Physics2D.Raycast(new Vector2(rayStart.x, rayStart.y), Vector2.zero, 0, mask);

            if(hitData.collider == null)
            {
                return;
            }

            if (hitData.collider.gameObject == worldBackground)
            {
                if(activeTask != null)
                {
                    GameObject target = activeTask.GetTaskTarget();

                    if(target.GetComponent<CarryableObject>() != true)
                    {
                        selectedControl.RemoveTask(activeTask);
                        activeTask = null;
                    }
                }

                Vector2 clickPos = camera.ScreenToWorldPoint(Input.mousePosition);

                selectedControl.MoveCharacterTo(clickPos);
                return;
            }

            if (hitData.collider.TryGetComponent(out IInteractable interactableObject))
            {
                if(activeTask != null)
                {
                    if(activeTask.GetTaskTarget() == selectedControl.heldObject)
                    {
                        return;
                    }

                    selectedControl.RemoveTask(activeTask);
                    activeTask = null;
                }

                activeTask = interactableObject.Task(selectedControl);

                if(activeTask != null)
                {
                    selectedControl.AddTask(activeTask);
                    activeTask.StartTask();
                }
            }
            else
            {
                if (activeTask != null)
                {
                    selectedControl.RemoveTask(activeTask);
                    activeTask = null;
                }
            }
            
        }
    }
}
