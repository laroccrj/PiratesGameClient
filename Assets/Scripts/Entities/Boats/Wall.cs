using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour, Interactable
{
    public int id;
    public Boat boat;
    public float health;
    public float maxHealth;
    public SpriteRenderer sprite;

    public int GetId()
    {
        return this.id;
    }

    public InteractableType GetInteractableType()
    {
        return InteractableType.WALL;
    }

    public void Update()
    {
        this.sprite.color = new Color(1, health / maxHealth, health / maxHealth, 1); 

        bool alive = health > 0;
        this.sprite.enabled = alive;
    }

    public InteractionType[] GetPossibleInteractionTypes()
    {
        return new InteractionType[]{
            InteractionType.REPAIR,
        };
    }
}
