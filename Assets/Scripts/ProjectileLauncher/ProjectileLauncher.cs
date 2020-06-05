using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public Projectile projectile;
    public float power = 10;
    public float reloadspeed = 1;
    public bool loaded = true;

    private float lastFireTime = 0;

    private Rigidbody2D rigid;

    private void Awake()
    {
        this.rigid = gameObject.GetComponentInParent<Rigidbody2D>();
    }

    private void Update()
    {
        if (Time.time > lastFireTime + reloadspeed)
        {
            this.loaded = true;
        }
    }

    public void fire()
    {
        if (this.loaded)
        {
            Projectile newProjectile = GameObject.Instantiate<Projectile>(this.projectile, this.transform.position, this.transform.rotation);
            this.ignoreObjectAndChildren(newProjectile.GetComponent<Collider2D>(), this.rigid.transform);

            Vector3 direction = newProjectile.transform.TransformDirection(Vector3.up);
            newProjectile.GetComponent<Rigidbody2D>().velocity = this.rigid.velocity;
            newProjectile.GetComponent<Rigidbody2D>().AddForce(direction * power);
            this.lastFireTime = Time.time;
            this.loaded = false;
        }
    }

    private void ignoreObjectAndChildren(Collider2D collider, Transform ignore)
    {
        if (ignore.GetComponent<Collider2D>() != null)
            Physics2D.IgnoreCollision(collider, ignore.GetComponent<Collider2D>());

        foreach (Transform child in ignore)
        {
            this.ignoreObjectAndChildren(collider, child);
        }
    }
}
