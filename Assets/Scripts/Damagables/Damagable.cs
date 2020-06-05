using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Damagable : MonoBehaviour
{
    public float health = 100;

    public void applyDamage(float damage)
    {
        this.health -= damage;
        this.onDamage();

        if (health <= 0)
        {
            this.onDeath();
        }
    }

    protected abstract void onDamage();
    protected abstract void onDeath();
}
