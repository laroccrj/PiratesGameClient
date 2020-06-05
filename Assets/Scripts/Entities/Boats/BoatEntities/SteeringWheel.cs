using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringWheel : BoatEntity, Interactable
{
    /**
    public override Mountables MountableType => Mountables.STEERING_WHEEL;

    public void Start()
    {
        this.cameraPosition = this.boat.transform;
    }

    public override void OnDismount()
    {
        CameraController.main.rotateTarget = PlayerManager.player.transform;
    }

    public override void OnMount()
    {
        CameraController.main.rotateTarget = null;
    }
    **/
    public override BoatEntityType BoatEntityType => BoatEntityType.STEERING_WHEEL;
    public Transform Body;

    public BoatEntity GetBoatEntity()
    {
        return this;
    }

    public InteractionType[] GetPossibleInteractionTypes()
    {
        return new InteractionType[]
        {
            InteractionType.USE
        };
    }

    public override void ReadDataFromPacket(Packet packet)
    {
        this.Body.localRotation = packet.ReadQuaternion();
    }
}
