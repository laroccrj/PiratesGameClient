using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMesh : MonoBehaviour
{
    public int acuracy;

    // Start is called before the first frame update
    void Start()
    {
        this.generateNodes();
    }

    // Update is called once per frame
    void generateNodes()
    {
        Vector2 halfScale = this.transform.localScale / 2;

        float minX = -halfScale.x;
        float maxX = halfScale.x;
        float x = minX;
        
        float minY = -halfScale.x;
        float maxY = halfScale.x;
        float y = minY;

        float step = 1f / acuracy;

        while (x < maxX)
        {
            y = minY;

            while (y < maxY)
            {
                Debug.Log(new Vector2(x, y));

                y += step;
            }

            x += step;
        }
        
    }
}
