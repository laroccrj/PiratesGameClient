using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : BoatEntity, Interactable
{
    public Boat boat;
    public float health = 100;
    public float maxHealth = 100;
    public SpriteRenderer sprite;

    public override BoatEntityType BoatEntityType => BoatEntityType.WALL;

    public BoatEntity GetBoatEntity()
    {
        return this;
    }

    public override void ReadDataFromPacket(Packet packet)
    {
        this.maxHealth = packet.ReadFloat();
        this.health = packet.ReadFloat();
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
