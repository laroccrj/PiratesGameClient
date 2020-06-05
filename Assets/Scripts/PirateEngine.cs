using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PirateEngine : MonoBehaviour
{
    public int speed = 1;
    public float distanceFromDestination = 0.1f;
    public Vector2 destination = new Vector2();

    public GameObject deck;
    private Mounter mounter;

    void Start()
    {
        this.destination = transform.position;
        this.mounter = this.GetComponent<Mounter>();
    }


    void FixedUpdate()
    {
        if (
            Vector2.Distance(this.destination, (Vector2)this.transform.localPosition) > this.distanceFromDestination
            && this.mounter.state != Mounter.STATE_MOUNTED
        ) {
            Vector2 direction = this.destination - (Vector2)this.transform.localPosition;
            transform.Translate(direction.normalized * speed * Time.deltaTime);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePoint, Vector2.zero,10, 1 << this.deck.layer);
            if (hit.collider != null)
            {
                Vector2 target = new Vector2();

                if (hit.collider.GetComponent<Mountable>() != null)
                {
                    target = hit.collider.transform.position;
                    this.mounter.mount(hit.collider.GetComponent<Mountable>());
                }
                else
                {
                    target = hit.point;
                    this.mounter.dismount();
                }

                this.destination = (Vector2)this.deck.transform.InverseTransformPoint(target);
            }
        }
    }

    public static Quaternion lookAt(Transform transform, Vector3 point)
    {
        Vector3 diff = point - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0f, 0f, rot_z - 90);
    }
}
