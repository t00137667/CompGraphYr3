using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverScript : MonoBehaviour
{
    Vector2 pointA = new Vector2(5.5f, 0.7f);
    Vector2 pointB = new Vector2(13.2f, 7.5f);

    // Start is called before the first frame update
    void Start()
    {
        Outcode outcodeA = new Outcode(pointA);
        Outcode outcodeB = new Outcode(pointB);
        Outcode inViewPort = new Outcode();
        List<Vector2Int> vector2Ints = new List<Vector2Int>();

        int screenwidth = Screen.width;
        int screenHeight = Screen.height;

        outcodeA.printOutcode();
        outcodeB.printOutcode();

        LineClip.lineClip(ref pointA, ref pointB);
        print(pointA + " " + pointB);

        Vector2Int pAI = new Vector2Int((int)pointA.x, (int)pointA.y);
        Vector2Int pBI = new Vector2Int((int)pointB.x, (int)pointB.y);

        vector2Ints = Breshenhams(pAI, pBI);


        foreach (Vector2Int v in vector2Ints)
            print(v);

        print(pointA + pointB);

        if ((outcodeA + outcodeB) == inViewPort)
        {
            Debug.Log("Trival Acceptance");
        }
        else if ((outcodeA * outcodeB) == inViewPort)
        {
            print("Trivial Rejection");
        }
    }

    private List<Vector2Int> Breshenhams(Vector2Int v, Vector2Int u)
    {
        int diffX = (int)(u.x - v.x);
        if (diffX < 0) return Breshenhams(u, v);

        int diffY = (int)(u.y - v.y);
        if (diffY < 0) negateY(Breshenhams(negateY(v), negateY(u)));

        if (diffY > diffX) swapXY(Breshenhams(swapXY(v), swapXY(u)));

        int twoDY = diffY * 2;
        int TwoDY2DX = twoDY - (diffX * 2);
        int p = twoDY - diffY;
        List<Vector2Int> points = new List<Vector2Int>();
        points.Add(v);



        for (int i = (int)v.x; i < u.x; i++)
        {
            
            if (p > 0)
            {
                v.x++;
                v.y++;
                p += TwoDY2DX;
            }
            else
            {
                v.x++;
                p += twoDY;
            }

            points.Add(v);
        }

        return points;
    }

    private Vector2Int swapXY(Vector2Int v)
    {
        return new Vector2Int(v.y, v.x);
    }

    private List<Vector2Int> swapXY(List<Vector2Int> list)
    {
        List<Vector2Int> output_list = new List<Vector2Int>();

        foreach (Vector2Int v in list)
            output_list.Add(swapXY(v));


        return output_list;
    }


    private List<Vector2Int> negateY(List<Vector2Int> list)
    {
        List<Vector2Int> output_list = new List<Vector2Int>();

        foreach (Vector2Int v in list)
            output_list.Add(negateY(v));


        return output_list;
    }

    private Vector2Int negateY(Vector2Int v)
    {
        return new Vector2Int(v.x, -v.y);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
