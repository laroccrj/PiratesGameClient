using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ProjectileType
{
    CANNON_BALL = 1
}

public abstract class Projectile : Entity
{
    public int id;
    public bool movingTowardsHit = false;
    public SpriteRenderer sprite;

    public abstract ProjectileType ProjectileType
    {
        get;
    }

    new public void Update()
    {
        base.Update();

        if (movingTowardsHit && Vector3.Distance(this.transform.position, TargetPosition) <= .1)
        {
            this.OnHitDestinationReached();
            this.movingTowardsHit = false;
        }
    }

    public void OnProjectileHit(Vector3 point)
    {
        this.TargetPosition = point;
        this.movingTowardsHit = true;
        this.OnHit(point);
    }

    protected abstract void OnHit(Vector3 point);
    protected abstract void OnHitDestinationReached();
}
