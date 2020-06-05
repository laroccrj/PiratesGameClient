using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : Damagable
{
    protected override void onDamage()
    {
        return;
    }

    protected override void onDeath()
    {
        Destroy(this.gameObject);
    }
}
