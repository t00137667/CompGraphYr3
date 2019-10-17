using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverScript : MonoBehaviour
{
    Vector2 pointA = new Vector2(0.5f, 0.7f);
    Vector2 pointB = new Vector2(-0.2f, -0.5f);

    // Start is called before the first frame update
    void Start()
    {
        Outcode outcodeA = new Outcode(pointA);
        Outcode outcodeB = new Outcode(pointB);
        Outcode inViewPort = new Outcode();

        outcodeA.printOutcode();
        outcodeB.printOutcode();

        lineClip(ref pointA, ref pointB);

        if ((outcodeA + outcodeB) == inViewPort)
        {
            Debug.Log("Trival Acceptance");
        }
        else if ((outcodeA * outcodeB) == inViewPort)
        {
            print("Trivial Rejection");
        }
    }

    public bool lineClip(ref Vector2 v, ref Vector2 u)
    {
        Outcode outcodeV = new Outcode(v);
        Outcode outcodeU = new Outcode(u);
        Outcode inViewPort = new Outcode();

        if ((outcodeV + outcodeU) == inViewPort) return true;
        if ((outcodeV * outcodeU) == inViewPort) return false;

        if (outcodeV == inViewPort)
            lineClip(ref u, ref v);



        if (outcodeV.up)  // Above viewport
        {
           Vector2 newV =  intercept(v, u, 0);

            Outcode newVOutcode = new Outcode(newV);
            if (newVOutcode == inViewPort)
            { v = newV;
                return lineClip(ref v, ref u);
        }

        return false;
    }

    private Vector2 intercept(Vector2 v, Vector2 u, int edgeId)
    {
        float slope = (u.y - v.y) / (u.x - v.x);

        switch (edgeId)
        {
            case 0: //Top
                return new Vector2( v.x + (1/slope)  * ( 1 - v.y) ,  1);

            case 1: //Bottom
                return new Vector2(v.x + (1 / slope) * (-1 - v.y), -1);

            case 2: //Left
                return new Vector2(-1, v.y + slope * (-1 - v.x));

            default:  // Right

            return new Vector2(1, v.y + slope * (1 - v.x));

        }
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
