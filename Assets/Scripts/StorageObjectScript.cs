using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageObjectScript : MonoBehaviour, IInteractable
{
    public CharacterControl GetCharacterControl()
    {
        return null;
    }
    public void Interact(CharacterControl control)
    {
        control.MoveCharacterTo(transform.position);
    }
    public List<Task> Task(CharacterControl control)
    {
        return null;
    }
    public Task Task(CharacterControl control, bool playerOverride)
    {
        return null;
    }
    public void Cancel(GameObject gameObject)
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<CharacterControl>() != null)
        {
            if(collision.gameObject.GetComponent<CharacterControl>().heldObject != null)
            {
                collision.gameObject.GetComponent<CharacterControl>().heldObject.Store();
            }
        }
    }
}
