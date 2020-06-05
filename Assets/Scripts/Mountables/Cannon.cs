using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : Mountable
{
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
}
