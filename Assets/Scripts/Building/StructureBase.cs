using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum StructureMaterials
{
    WOODEN,
    STONE,
    STEEL
}
public class StructureBase : MonoBehaviour
{
    [HideInInspector]
    public string structureName;
    [HideInInspector]
    public int durability;

    public int woodDurability;
    public int stoneDurability;
    public int steelDurability;

    public Sprite woodSprite;
    public Sprite stoneSprite;
    public Sprite steelSprite;

    [HideInInspector]
    public BoxCollider2D sCollider;

    protected new SpriteRenderer renderer;
    protected Rigidbody2D rigidBody;

    protected bool isReady;
    public virtual void Init(StructureMaterials material) { }

    protected void InitComponents()
    {
        sCollider = this.AddComponent<BoxCollider2D>();
        sCollider.size = new Vector2(0.49f, 0.49f);
        rigidBody = this.AddComponent<Rigidbody2D>();
        rigidBody.gravityScale = 0;
        rigidBody.mass = 100000;
        rigidBody.isKinematic = false;
        rigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        rigidBody.useFullKinematicContacts = true;

        renderer = this.AddComponent<SpriteRenderer>();
    }
    public void IsReady()
    {
        isReady = true;
    }
}
