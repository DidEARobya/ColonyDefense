using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageObjectScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
