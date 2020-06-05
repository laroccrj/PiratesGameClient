using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : BoatEntity, Interactable, ProjectileLauncher
{
    /**
    public override Mountables MountableType => Mountables.CANNON;
    public SpriteRenderer sprite;

    public override void OnDismount()
    {
    }

    public override void OnMount()
    {
    }

    public void Update()
    {
        this.sprite.color = new Color(1, health / maxHealth, health / maxHealth, 1);
    }
    **/
    public Transform launcher;
    public Transform body;
    public override BoatEntityType BoatEntityType => BoatEntityType.CANNON;

    public BoatEntity GetBoatEntity()
    {
        return this;
    }

    public Transform GetLauncher()
    {
        return this.launcher;
    }

    public InteractionType[] GetPossibleInteractionTypes()
    {
        return new InteractionType[]{
            InteractionType.REPAIR,
            InteractionType.USE
        };
    }

    public override void ReadDataFromPacket(Packet packet)
    {
        this.body.localRotation = packet.ReadQuaternion();
    }
}
