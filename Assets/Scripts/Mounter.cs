using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mounter : MonoBehaviour
{
    public const int STATE_IDLE     = 0;
    public const int STATE_MOUNTING = 1;
    public const int STATE_MOUNTED  = 2;

    public float mountDistance = 1;
    public Mountable mountable = null;
    public int state = STATE_IDLE;

    public void mount(Mountable mountable)
    {
        this.mountable = mountable;
        this.state = STATE_MOUNTING;
    }

    public void dismount()
    {
        if (this.mountable != null)
        {
            if (this.mountable.mounted && this.state == STATE_MOUNTED)
            {
                this.mountable.dismount();
            }

            this.mountable = null;
            this.state = STATE_IDLE;
        }
        
    }

    public void Update()
    {
        if (
            this.state == STATE_MOUNTING
            && Vector2.Distance(this.transform.position, this.mountable.transform.position) <= this.mountDistance
        ){ 
            this.state = STATE_MOUNTED;
            this.mountable.mount();
        }
        
        if (this.state == STATE_MOUNTED)
        {
            this.transform.position = this.mountable.seat.position;
            //this.transform.rotation = PirateEngine.lookAt(transform, this.mountable.transform.position);
        }
    }
}
