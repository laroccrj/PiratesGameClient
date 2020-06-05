using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : Projectile
{
    public override ProjectileType ProjectileType => ProjectileType.CANNON_BALL;
    public GameObject explosion;

    protected override void OnHit(Vector3 point)
    {
        GameObject.Instantiate(explosion, point, Quaternion.identity);
    }

    protected override void OnHitDestinationReached()
    {
        Destroy(this.gameObject);
    }
}
