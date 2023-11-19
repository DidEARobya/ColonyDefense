using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;
using Pathfinding;
using Pathfinding.Legacy;
using System.Runtime.CompilerServices;
using UnityEngine.TextCore.Text;
using System.Net.Http.Headers;

public enum CharacterStates
{
    SELECTED,
    WORKING,
    INCOMBAT
}

public class CharacterControl : MonoBehaviour, ISelectable, ICharacter
{
    private WorldGrid worldGrid;

    public CharacterObject characterObject;
    private GameObject selectedAnim;
    private GameObject isSelectedObject;

    public CharacterStates state;
    private CharacterStates lastState;

    private Seeker seeker;
    public AIPathCustom aiPath;
    private CharacterPathDisplay pathDisplay;

    public WorkAI workAi;
    private BehaviourTreeBase behaviourTree;

    private Vector3 colliderOffset;
    private Vector2 lastPos;
    private Vector2 idlePos;

    private float idleDelay;
    private float idleTime;

    public ResourceControl interactingObject;
    public CarryableObject heldObject;

    private bool isSpawned;
    public bool isSelected;

    public void Init(CharacterObject character, Vector2 spawnpos)
    {
        if(character == null)
        {
            return;
        }

        worldGrid = GameHandler.GetWorldGrid_Static();
        characterObject = character;

        this.transform.position = spawnpos;
        this.name = characterObject.characterName;

        Object temp = Resources.Load("Animations/CharacterSelected");
        selectedAnim = temp as GameObject;

        colliderOffset = this.GetComponent<BoxCollider2D>().offset;

        seeker = this.AddComponent<Seeker>();
        aiPath = this.AddComponent<AIPathCustom>();
        pathDisplay = this.AddComponent<CharacterPathDisplay>();
        workAi = this.AddComponent<WorkAI>();
        behaviourTree = this.AddComponent<BehaviourTreeBase>();

        if(aiPath != null)
        {
            InitAIPath();
        }

        if (behaviourTree != null)
        {
            behaviourTree.Init(characterObject.GetPriorities());
        }

        if (workAi != null)
        {
            workAi.Init(this);
        }

        idleDelay = 2.0f;
        state = CharacterStates.WORKING;
        isSpawned = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpawned == false)
        {
            return;
        }

        idleTime += Time.deltaTime;

        if (aiPath.velocity == Vector3.zero)
        {
            return;
        }

        RotateCharacter();
    }

    private void LateUpdate()
    {
        if(isSpawned == false)
        {
            return;
        }

        ManageAI();
    }
    public void Select()
    {
        isSelected = true;

        lastState = state;
        state = CharacterStates.SELECTED;

        if(isSelectedObject == null)
        {
            isSelectedObject = Instantiate(selectedAnim);
            isSelectedObject.transform.position = this.transform.position + (colliderOffset * 2);
            isSelectedObject.transform.SetParent(this.transform);
        }
    }
    public void DeSelect()
    {
        isSelected = false;

        state = lastState;

        if(isSelectedObject != null)
        {
            Destroy(isSelectedObject);
        }
    }
    public Task GetCurrentTask()
    {
        if(workAi.tasks.Count <= 0)
        {
            return null;
        }

        return workAi.tasks[0];
    }
    public void AddTask(Task task)
    {
        if(workAi.tasks.Contains(task))
        {
            return;
        }

        workAi.tasks.Add(task);
    }
    public void RemoveTask(Task task)
    {
        if(workAi.tasks.Contains(task))
        {
            task.StopTask();
            workAi.tasks.Remove(task);
        }
    }
    public void Interact(CharacterControl character)
    {
       
    }
    public void Cancel(GameObject newObj)
    {

    }
    public void IdleCharacter(Vector2 pos)
    {
        if(idlePos != pos)
        {
            idlePos = pos;
        }

        int rand = Random.Range(0, 9);
        Vector2 newPos = Vector2.zero;

        switch(rand)
        {
            case 0:
                newPos = idlePos + new Vector2(1, 0);
                break;
            case 1:
                newPos = idlePos + new Vector2(1, 1);
                break;
            case 2:
                newPos = idlePos + new Vector2(0, 1);
                break;
            case 3:
                newPos = idlePos + new Vector2(-1, 1);
                break;
            case 4:
                newPos = idlePos + new Vector2(-1, 0);
                break;
            case 5:
                newPos = idlePos + new Vector2(-1, -1);
                break;
            case 6:
                newPos = idlePos + new Vector2(0, -1);
                break;
            case 7:
                newPos = idlePos + new Vector2(1, -1);
                break;
            case 8:
                newPos = idlePos;
                break;
        }

        if(idleTime >= idleDelay)
        {
            this.MoveCharacterTo(newPos);
            idleTime = 0;
        }
    }
    private void RotateCharacter()
    {
        if (aiPath.destination.x - this.transform.position.x < 0)
        {
            Quaternion temp = this.transform.rotation;
            temp.y = -180;
            this.transform.rotation = temp;
        }
        else if (aiPath.destination.x - this.transform.position.x > 0)
        {
            Quaternion temp = this.transform.rotation;
            temp.y = 0;
            this.transform.rotation = temp;
        }
    }
    private void InitAIPath()
    {
        aiPath.orientation = OrientationMode.YAxisForward;
        aiPath.maxSpeed = characterObject.moveSpeed;
        aiPath.maxAcceleration = -1 / 0.4f;
        aiPath.slowdownDistance = 0.4f;
        aiPath.enableRotation = false;
        aiPath.pickNextWaypointDist = 2;
        aiPath.gravity = new Vector3(0, 0, 0);
        aiPath.endReachedDistance = 0.03f;
        aiPath.whenCloseToDestination = CloseToDestinationMode.Stop;
    }
    private void ManageAI()
    {
        switch(state)
        {
            case CharacterStates.SELECTED:

                break;

            case CharacterStates.WORKING:

                workAi.AIUpdate();
                break;

            case CharacterStates.INCOMBAT:

                break;
        }
    }
    public void MoveCharacterTo(Vector2 destination)
    {
        aiPath.destination = worldGrid.GetCellCentre(worldGrid.GetWorldToCell(destination));
    }
    public bool CheckIfStopped(float remainingDistance)
    {
        if (aiPath.remainingDistance <= remainingDistance)
        {
            return true;
        }

        return false;
    }
}
