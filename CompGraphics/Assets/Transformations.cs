using UnityEngine;
using System.Collections;
using System;

public class Transformations : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Vector3[] cube = new Vector3[8];
        cube[0] = new Vector3(1, 1, 1);
        cube[1] = new Vector3(-1, 1, 1);
        cube[2] = new Vector3(-1, -1, 1);
        cube[3] = new Vector3(1, -1, 1);
        cube[4] = new Vector3(1, 1, -1);
        cube[5] = new Vector3(-1, 1, -1);
        cube[6] = new Vector3(-1, -1, -1);
        cube[7] = new Vector3(1, -1, -1);

        Vector3 startingAxis = new Vector3(8, 1, 1);
        startingAxis.Normalize();
        Quaternion rotation = Quaternion.AngleAxis(17, startingAxis);


        // Calculate Rotation Matrix
        Matrix4x4 rotationMatrix =
            Matrix4x4.TRS(new Vector3(0,0,0),
                            rotation,
                            Vector3.one);
        printMatrix(rotationMatrix);

        Vector3[] imageAfterRotation =
            MatrixTransform(cube, rotationMatrix);
        printVerts(imageAfterRotation);

        // Save to text file
        PrintMatrixToFile(rotationMatrix, "Rotation Matrix");
        PrintVertsToFile(imageAfterRotation, "\nImage after Rotation");


        // Calculate Scaling Matrix
        Matrix4x4 scalingMatrix =
            Matrix4x4.TRS(new Vector3(0, 0, 0),
                            Quaternion.identity,
                            new Vector3(8,3,1));
        printMatrix(scalingMatrix);

        Vector3[] imageAfterScaling =
            MatrixTransform(imageAfterRotation, scalingMatrix);
        printVerts(imageAfterScaling);

        // Save to text file
        PrintMatrixToFile(scalingMatrix, "\nScaling Matrix");
        PrintVertsToFile(imageAfterScaling, "\nImage after Scaling");


        // Calculate Translation Matrix
        Matrix4x4 translationMatrix =
            Matrix4x4.TRS(new Vector3(2, 1, 2),
                            Quaternion.identity,
                            Vector3.one);
        printMatrix(translationMatrix);

        Vector3[] imageAfterTranslation =
            MatrixTransform(imageAfterScaling, translationMatrix);
        printVerts(imageAfterTranslation);

        // Save to text file
        PrintMatrixToFile(translationMatrix, "\nTranslation Matrix");
        PrintVertsToFile(imageAfterTranslation, "\nImage after Translation");

        // Reverse above order for Super Matrix
        Matrix4x4 superMatrix = translationMatrix * scalingMatrix * rotationMatrix;

        Vector3[] imageAfterSuperMatrix = MatrixTransform(cube, superMatrix);

        PrintMatrixToFile(superMatrix, "\nSuper Matrix");
        PrintVertsToFile(imageAfterSuperMatrix, "\nImage after Super Matrix");

        // Viewing Matrix
        Vector3 pos = -new Vector3(10, 4, 51);
        Vector3 direction = (new Vector3(1, 8, 1) - new Vector3(10, 4, 51));
        Vector3 cameraUp = new Vector3(2, 1, 8);
        Quaternion rot = Quaternion.LookRotation(direction.normalized,cameraUp.normalized);
        Matrix4x4 viewingMatrix = Matrix4x4.TRS(pos,rot,Vector3.one);

        Vector3[] imageAfterViewing =
            MatrixTransform(imageAfterTranslation, viewingMatrix);

        // Save to file
        PrintMatrixToFile(viewingMatrix, "\nViewing Matrix");
        PrintVertsToFile(imageAfterViewing, "\nImage after Viewing Matrix");

        Matrix4x4 perspectiveMatrix = Matrix4x4.Perspective(45, 1.2f, 1, 1000);

        Vector3[] imageAfterPerspective =
            MatrixTransform(imageAfterViewing, perspectiveMatrix);

        // Save to file
        PrintMatrixToFile(perspectiveMatrix, "\nPespective Matrix");
        PrintVertsToFile(imageAfterPerspective, "\nImage after Perspective Matrix");

        Matrix4x4 megaMatrix = perspectiveMatrix * viewingMatrix * superMatrix;

        Vector3[] imageAfterMegaMatrix = MatrixTransform(cube, megaMatrix);

        PrintMatrixToFile(megaMatrix, "\nMega Matrix");
        PrintVertsToFile(imageAfterMegaMatrix, "\nImage after Mega Matrix");

    }

    private void printVerts(Vector3[] newImage)
    {
        for (int i = 0; i < newImage.Length; i++)
            print(newImage[i].x + " , " +
                newImage[i].y + " , " +
                newImage[i].z);

    }

    private Vector3[] MatrixTransform(
        Vector3[] meshVertices, 
        Matrix4x4 transformMatrix)
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

    private void printMatrix(Matrix4x4 matrix)
    {
        for (int i = 0; i < 4; i++)
            print(matrix.GetRow(i).ToString());
    }

    private void PrintMatrixToFile(Matrix4x4 matrix, string text)
    {
        using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"I:\Year 3\Semester 1\Computer Graphics\CompGraphics\Assets\Matrix.txt", true))
        {
            file.WriteLine(text);
            
            file.WriteLine(matrix.m00 + ", " + matrix.m01 + ", " + matrix.m02 + ", " + matrix.m03);
            file.WriteLine(matrix.m10 + ", " + matrix.m11 + ", " + matrix.m12 + ", " + matrix.m13);
            file.WriteLine(matrix.m20 + ", " + matrix.m21 + ", " + matrix.m22 + ", " + matrix.m23);
            file.WriteLine(matrix.m30 + ", " + matrix.m31 + ", " + matrix.m32 + ", " + matrix.m33);

        }
    }

    private void PrintVertsToFile(Vector3[] newImage, string text)
    {
        using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"I:\Year 3\Semester 1\Computer Graphics\CompGraphics\Assets\Matrix.txt", true))
        {
            file.WriteLine(text);
            for (int i = 0; i < newImage.Length; i++)
                file.WriteLine(newImage[i].x + " , " +
                    newImage[i].y + " , " +
                    newImage[i].z);
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
