using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringWheel : Mountable
{
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
}
