using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriverScript : MonoBehaviour
{
    Vector2 pointA = new Vector2(5.5f, 0.7f);
    Vector2 pointB = new Vector2(13.2f, 7.5f);

    Vector3[] cube;

    
    Matrix4x4 megaMatrix;
    int screenwidth;
    int screenHeight;

    // Start is called before the first frame update
    void Start()
    {
        
        cube = new Vector3[8];

        cube[0] = new Vector3(1, 1, 1);
        cube[1] = new Vector3(-1, 1, 1);
        cube[2] = new Vector3(-1, -1, 1);
        cube[3] = new Vector3(1, -1, 1);
        cube[4] = new Vector3(1, 1, -1);
        cube[5] = new Vector3(-1, 1, -1);
        cube[6] = new Vector3(-1, -1, -1);
        cube[7] = new Vector3(1, -1, -1);



        //Outcode outcodeA = new Outcode(pointA);
        //Outcode outcodeB = new Outcode(pointB);
        //Outcode inViewPort = new Outcode();
        //List<Vector2Int> vector2Ints = new List<Vector2Int>();

        screenwidth = Screen.width;
        screenHeight = Screen.height;

        //outcodeA.printOutcode();
        //outcodeB.printOutcode();

        //LineClip.lineClip(ref pointA, ref pointB);
        //print(pointA + " " + pointB);

        //Vector2Int pAI = new Vector2Int((int)pointA.x, (int)pointA.y);
        //Vector2Int pBI = new Vector2Int((int)pointB.x, (int)pointB.y);

        //vector2Ints = LineClip.Breshenhams(pAI, pBI);

        //foreach (Vector2Int v in vector2Ints)
        //    print(v);

        //print(pointA + pointB);

        //if ((outcodeA + outcodeB) == inViewPort)
        //{
        //    Debug.Log("Trival Acceptance");
        //}
        //else if ((outcodeA * outcodeB) == inViewPort)
        //{
        //    print("Trivial Rejection");
        //}
    }

    private Matrix4x4 getRotationMatrix()
    {
        Vector3 startingAxis = new Vector3(8, 1, 1);
        startingAxis.Normalize();
        Quaternion rotation = Quaternion.AngleAxis(17, startingAxis);
        return Matrix4x4.TRS(new Vector3(0, 0, 0),rotation, Vector3.one);
    }
    private Matrix4x4 getScalingMatrix()
    {
        return Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(8, 3, 1));
    }
    private Matrix4x4 getTranslationMatrix()
    {
        return Matrix4x4.TRS(new Vector3(2, 1, 2), Quaternion.identity, Vector3.one);
    }
    private Matrix4x4 getViewingMatrix()
    {
        Vector3 pos = -new Vector3(10, 4, 51);
        Vector3 direction = (new Vector3(1, 8, 1) - new Vector3(10, 4, 51));
        Vector3 cameraUp = new Vector3(2, 1, 8);
        Quaternion rot = Quaternion.LookRotation(direction.normalized, cameraUp.normalized);
        return Matrix4x4.TRS(pos, rot, Vector3.one);
    }
    private Matrix4x4 getPerspectiveMatrix()
    {
        return  Matrix4x4.Perspective(45, 1.2f, 1, 1000);
    }
    private Matrix4x4 getSuperMatrix()
    {
        return getTranslationMatrix() * getScalingMatrix() * getRotationMatrix();
    }
    private Matrix4x4 getMegaMatrix()
    {
        return megaMatrix = getPerspectiveMatrix() * getViewingMatrix() * getSuperMatrix();
    }
    private Vector3[] MatrixTransform(Vector3[] meshVertices, Matrix4x4 transformMatrix)
    {
        Vector3[] output = new Vector3[meshVertices.Length];
        for (int i = 0; i < meshVertices.Length; i++)
            output[i] = transformMatrix *
                new Vector4(
                meshVertices[i].x,
                meshVertices[i].y,
                meshVertices[i].z,
                    1);

        return output;
    }

    private Vector2[] divideByZ()
    {
        Vector3[] image = getImageTransform();
        List<Vector2> output = new List<Vector2>();
        foreach (Vector3 v in image)
        {
            output.Add(new Vector2(v.x/v.z,v.y/v.z));
        }
        return output.ToArray();
    }
    private Vector3[] getImageTransform()
    {
        return MatrixTransform(cube, getMegaMatrix());
    }
    private Vector2Int convertToScreen(Vector2 v)
    {
        int x = (int)Math.Round((v.x + 1 / 2) * (screenwidth - 1));
        int y = (int)Math.Round((1 + v.y / 2) * (screenHeight - 1));
        return new Vector2Int(x, y);
    }
    private Vector2Int[] screenImage()
    {
        Vector2[] input = divideByZ();
        List<Vector2Int> output = new List<Vector2Int>();
        foreach (Vector2 v in input)
        {
            output.Add(convertToScreen(v));
        }
        return output.ToArray();

    }

    private Vector2[] clippedImage()
    {
        Vector2[] input = divideByZ();
        List<Vector2Int> output = new List<Vector2Int>();

        LineClip.lineClip(ref input[0],ref input[1]);
        LineClip.lineClip(ref input[0], ref input[1]);
        

        return input;
    }

    // Update is called once per frame
    void Update()
    {

        Vector2[] image = divideByZ();

        if (LineClip.lineClip(ref image[0],ref image[1]))
        {
            LineClip.Breshenhams(convertToScreen(image[0]), convertToScreen(image[1]));
        }
        if (LineClip.lineClip(ref image[0], ref image[4]))
        {
            LineClip.Breshenhams(convertToScreen(image[0]), convertToScreen(image[1]));
        }

        if (LineClip.lineClip(ref image[1], ref image[2]))
        {
            LineClip.Breshenhams(convertToScreen(image[0]), convertToScreen(image[1]));
        }
        if (LineClip.lineClip(ref image[1], ref image[5]))
        {
            LineClip.Breshenhams(convertToScreen(image[0]), convertToScreen(image[1]));
        }
        if (LineClip.lineClip(ref image[5], ref image[4]))
        {
            LineClip.Breshenhams(convertToScreen(image[0]), convertToScreen(image[1]));
        }
        if (LineClip.lineClip(ref image[5], ref image[6]))
        {
            LineClip.Breshenhams(convertToScreen(image[0]), convertToScreen(image[1]));
        }
        if (LineClip.lineClip(ref image[2], ref image[6]))
        {
            LineClip.Breshenhams(convertToScreen(image[0]), convertToScreen(image[1]));
        }
        if (LineClip.lineClip(ref image[2], ref image[3]))
        {
            LineClip.Breshenhams(convertToScreen(image[0]), convertToScreen(image[1]));
        }
        if (LineClip.lineClip(ref image[7], ref image[4]))
        {
            LineClip.Breshenhams(convertToScreen(image[0]), convertToScreen(image[1]));
        }
        if (LineClip.lineClip(ref image[7], ref image[3]))
        {
            LineClip.Breshenhams(convertToScreen(image[0]), convertToScreen(image[1]));
        }
        if (LineClip.lineClip(ref image[2], ref image[3]))
        {
            LineClip.Breshenhams(convertToScreen(image[0]), convertToScreen(image[1]));
        }
    }
}
