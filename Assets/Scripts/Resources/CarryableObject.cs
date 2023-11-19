using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarryableObject : MonoBehaviour
{
    private CharacterControl heldBy;
    public CharacterControl targetedBy;
    public bool isTargeted;

    public CharacterControl GetCarrier()
    {
        return heldBy;
    }
    protected void Carry(CharacterControl characterControl)
    {
        heldBy = characterControl;
        heldBy.heldObject = this;
    }
    public void Drop()
    {
        if(heldBy != null)
        {
            heldBy.heldObject = null;
            heldBy = null;
        }

        targetedBy = null;
        isTargeted = false;
    }

    public virtual void Store() { }
}

