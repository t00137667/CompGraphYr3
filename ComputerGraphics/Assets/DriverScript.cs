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

    int wall = -1;

    Renderer ourRenderer;
    Matrix4x4 megaMatrix;
    int screenwidth;
    int screenHeight;
    Texture2D ourScreen;
    private float angle;
    private int movement;
    Color targetColor;
    List<Edge> edgeTable;

    // Start is called before the first frame update
    void Start()
    {
        ourRenderer = GetComponent<Renderer>();

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

        ourScreen = new Texture2D(screenwidth, screenHeight);
        ourRenderer.material.mainTexture = ourScreen;

        targetColor = ourScreen.GetPixel(1, 1);

        edgeTable = new List<Edge>();

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

    private Matrix4x4 getRotationMatrix(float angle, Vector3 axis)
    {

        Quaternion rotation = Quaternion.AngleAxis(angle, axis.normalized);
        return Matrix4x4.TRS(new Vector3(0, 0, 0), rotation, Vector3.one);
    }
    private Matrix4x4 getScalingMatrix()
    {
        return Matrix4x4.TRS(new Vector3(0, 0, 0), Quaternion.identity, new Vector3(8, 3, 1));
    }
    private Matrix4x4 getTranslationMatrix(Vector3 v)
    {
        return Matrix4x4.TRS(v, Quaternion.identity, Vector3.one);
    }
    private Matrix4x4 getViewingMatrix(Vector3 pos, Vector3 target, Vector3 up)
    {

        Vector3 direction = target - pos;

        Quaternion rot = Quaternion.LookRotation(direction.normalized, up.normalized);
        return Matrix4x4.TRS(-pos, rot, Vector3.one);
    }
    private Matrix4x4 getPerspectiveMatrix()
    {
        return Matrix4x4.Perspective(45, 1.2f, 1, 1000);
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

    private Vector2[] divideByZ(Vector3[] image)
    {

        List<Vector2> output = new List<Vector2>();
        foreach (Vector3 v in image)
        {
            output.Add(new Vector2(v.x / v.z, v.y / v.z));
        }
        return output.ToArray();
    }

    private Vector2Int convertToScreen(Vector2 v)
    {
        int x = (int)Math.Round((v.x + 1.0f) * (screenwidth - 1.0f) / 2.0f);
        int y = (int)Math.Round((1.0f - v.y) * (screenHeight - 1.0f) / 2.0f);
        return new Vector2Int(x, y);
    }
    //private Vector2Int[] screenImage()
    //{
    //    Vector2[] input = divideByZ();
    //    List<Vector2Int> output = new List<Vector2Int>();
    //    foreach (Vector2 v in input)
    //    {
    //        output.Add(convertToScreen(v));
    //    }
    //    return output.ToArray();

    //}

    //private Vector2[] clippedImage()
    //{
    //    Vector2[] input = divideByZ();
    //    List<Vector2Int> output = new List<Vector2Int>();

    //    LineClip.lineClip(ref input[0],ref input[1]);
    //    LineClip.lineClip(ref input[0], ref input[1]);


    //    return input;
    //}

    // Update is called once per frame
    void Update()
    {
        angle++;
        //print(angle);
        Destroy(ourScreen);

        ourScreen = new Texture2D(screenwidth, screenHeight);
        ourRenderer.material.mainTexture = ourScreen;

        targetColor = ourScreen.GetPixel(1, 1);


        Matrix4x4 world = getRotationMatrix(angle, new Vector3(1, 1, 1)); // * getTranslationMatrix(new Vector3(8, 0, 2))  ;
        Matrix4x4 view = getViewingMatrix(new Vector3(0, 0, 20), new Vector3(1, 0, 0), Vector3.up);

        Vector3[] imageafterProjection = MatrixTransform(cube, getPerspectiveMatrix() * view * world);

        Vector2[] image = divideByZ(imageafterProjection);

        //Draw(image);
        DrawPoly(image);
        //Vector2Int pixelPoint = convertToScreen(image[2]);
        //int pos_x = pixelPoint.x, pos_y = pixelPoint.y;
        //Fill(pos_x, pos_y, targetColor, Color.black);


        ourScreen.Apply();
    }

    private void Draw(Vector2[] image)


    {
        drawLine(image[0], image[1]);
        drawLine(image[1], image[2]);
        drawLine(image[2], image[3]);
        drawLine(image[3], image[0]);
        drawLine(image[4], image[5]);
        drawLine(image[5], image[6]);
        drawLine(image[6], image[7]);
        drawLine(image[7], image[4]);
        drawLine(image[0], image[4]);
        drawLine(image[1], image[5]);
        drawLine(image[2], image[6]);
        drawLine(image[3], image[7]);




    }

    private void DrawPoly(Vector2[] image)
    {
        Vector2[] polyTop = { image[5], image[1], image[0], image[4], image[5] };
        Vector2[] polyFront = { image[1], image[2], image[3], image[0], image[1] };
        Vector2[] polyLeft = { image[0], image[3], image[7], image[4], image[0] };
        Vector2[] polyBack = { image[4], image[7], image[6], image[5], image[4] };
        Vector2[] polyRight = { image[5], image[6], image[2], image[1], image[5] };
        Vector2[] polyBottom = { image[2], image[6], image[7], image[3], image[2] };

        List<Vector2[]> polygons = new List<Vector2[]>();
        polygons.Add(polyTop);
        polygons.Add(polyFront);
        polygons.Add(polyLeft);
        polygons.Add(polyBack);
        polygons.Add(polyRight);
        polygons.Add(polyBottom);

        foreach (Vector2[] v in polygons)
        {
            if (isFront(v[0], v[1], v[2]))
            {
                drawLine(v[0], v[1]);
                drawLine(v[1], v[2]);
                drawLine(v[2], v[3]);
                drawLine(v[3], v[4]);

            }

            if (isFront(v[0], v[1], v[2]))
            {
                Vector2Int fill = fillPoint(v[0], v[1], v[2]);
                if (!onScreen(fill)) print("Fillpoint Offscreen");
                Fill(fill.x, fill.y, Color.blue, Color.cyan);
            }

        }

    }

    private bool isFront(Vector2 v, Vector2 u, Vector2 w)
    {
        Vector2 v1 = u - v;
        Vector2 u1 = w - u;
        double cross = (v1.x * u1.y) - (v1.y * u1.x);
        return cross > 0 ? true : false;
    }

    private Vector2Int fillPoint(Vector2 v, Vector2 u, Vector2 w)
    {
        float x = 0f;
        float y = 0f;
        float z = 0f;

        x = (v.x + u.x + w.x) / 3;
        y = (v.y + u.y + w.y) / 3;

        return convertToScreen(new Vector2(x, y));
    }

    private void drawLine(Vector2 Start, Vector2 Finish)
    {
        Vector2 start = Start, finish = Finish;

        if (LineClip.lineClip(ref start, ref finish))
        {
            Draw(LineClip.Breshenhams(convertToScreen(start), convertToScreen(finish)));
        }

    }

    private void Draw(List<Vector2Int> list)
    {
        foreach (Vector2Int v in list)
            ourScreen.SetPixel(v.x, v.y, Color.blue);
    }

    bool onScreen(int x, int y)
    {
        return (x >= 0) && (y >= 0) && (x < screenwidth) && (y < screenHeight);
    }

    bool onScreen(Vector2Int v)
    {
        return (v.x >= 0) && (v.y >= 0) && (v.x < screenwidth) && (v.y < screenHeight);
    }
    private void Fill(int pos_x, int pos_y, Color wall_colour, Color fill_colour)
    {
        //print(pos_x + ", " + pos_y);

        if (!onScreen(pos_x, pos_y)) return;

        if (ourScreen.GetPixel(pos_x, pos_y) == fill_colour)
        {
            return;// if there is no wall or if i haven't been there
        }

        if (ourScreen.GetPixel(pos_x, pos_y) == wall_colour) // if it's not color go back
            return;

        ourScreen.SetPixel(pos_x, pos_y, fill_colour); // mark the point so that I know if I passed through it. 

        Fill(pos_x + 1, pos_y, wall_colour, fill_colour);  // then i can either go south
        Fill(pos_x - 1, pos_y, wall_colour, fill_colour);  // or north
        Fill(pos_x, pos_y + 1, wall_colour, fill_colour);  // or east
        Fill(pos_x, pos_y - 1, wall_colour, fill_colour);  // or west

        return;
    }


    bool EqualColour(Color one, Color two)
    {
        Vector3 diff = new Vector3(one.r - two.r, one.g - two.g, one.b - two.b);
        return diff.magnitude < 0.3f;
    }
    private void FillCube()
    {

    }

    private void CreateEdges(Vector2[] polygon)
    {

        for (int i = 0; i < polygon.Length - 1; i++)
        {
            Vector2 firstPoint, secondPoint;

            if (polygon[i].x < polygon[i + 1].x)
            {
                firstPoint = polygon[i];
                secondPoint = polygon[i + 1];
            }
            else
            {
                firstPoint = polygon[i + 1];
                secondPoint = polygon[i];
            }

            int dX = (int)(secondPoint.x - firstPoint.x);
            int dY = (int)(secondPoint.y - firstPoint.y);
            int sign = dY > 0 ? 1 : -1;
            if (dX != 0)
            {
                Edge edge = new Edge((int)Math.Max(firstPoint.y, secondPoint.y),
                (int)Math.Min(firstPoint.y, secondPoint.y),
                (int)Math.Min(firstPoint.y, secondPoint.y),
                sign,
                dX,
                dY
                );
                edgeTable.Add(edge);
            }

        }


    }

//    Create ET
//    1. Process the vertices list in pairs, start with[numOfVertices - 1] and[0].
//    2. For each vertex pair, create an edge bucket
//2. Sort ET by yMin
//3. Process the ET
//    1. Start on the scan line equal to the yMin of the first edge in the ET
//    2. While the ET contains edges
//        1. Check if any edges in the AL need to be removes(when yMax == current scan line)
//            1. If an edge is removed from the AL, remove the associated the Edge Bucket from the Edge Table.
//        2. If any edges have a yMin == current scan line, add them to the AL
//        3. Sort the edges in AL by X
//        4. Fill in the scan line between pairs of edges in AL
//        5. Increment current scan line
//        6. Increment all the X's in the AL edges based on their slope
//            1. If the edge's slope is vertical, the bucket's x member is NOT incremented.

    private void processEdgeTable(List<Edge> edgeTable)
    {
        List<Edge> activeEdge = new List<Edge>();
        while (edgeTable.Count != 0)
        {
            if (activeEdge.Count != 0)
            {
                foreach (Edge e in activeEdge)
                {
                    if (e.yMax == currentScanline)
                    {
                        edgeTable.Remove(e);
                        activeEdge.Remove(e);
                    }
                }                
            }
            for ()
            {

            }
            for ()
            {

            }
        }
    }


private class Edge
    {
        public int yMax;
        public int yMin;
        public int x;
        public int sign;
        public int dX;
        public int dY;
        public double sum = 0;

        public Edge(int yMax, int yMin)
        {
            this.yMax = yMax;
            this.yMin = yMin;
            x = yMin;

        }

        public Edge(int yMax, int yMin, int x, int sign, int dX, int dY) : this(yMax, yMin)
        {
            this.x = x;
            this.sign = sign;
            this.dX = dX;
            this.dY = dY;
        }
    }
}
