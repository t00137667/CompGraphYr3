using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Outcode
{
   public bool up, down, left, right;

    public Outcode()
    {

    }

    public Outcode(Vector2 v)
    {
        up = v.y > 1;
        down = v.y < -1;
        left = v.x < -1;
        right = v.x > 1;
    }

    public Outcode (bool Up, bool Down, bool Left, bool Right)
    {
        up = Up;
        down = Down;
        left = Left;
        right = Right;
    }

    public static bool operator == (Outcode a, Outcode b)
    {
       return (a.up == b.up) && (a.down == b.down) && (a.left == b.left) && (a.right == b.right);
    }

    public static bool operator != (Outcode a, Outcode b)
    {
        return !(a == b);
    }

    public static Outcode operator + (Outcode a, Outcode b)
    {
        return new Outcode(a.up || b.up, a.down || b.down, a.left || b.left, a.right || b.right);
    }

    public static Outcode operator *(Outcode a, Outcode b)
    {
        return new Outcode(a.up && b.up, a.down && b.down, a.left && b.left, a.right && b.right);
    }

    public void printOutcode()
    {
        string upP, downP, leftP, rightP;
        if (up)
            upP = "1";
        else
            upP = "0";

        if (down)
           downP = "1";
        else
            downP = "0";

        if (left)
            leftP = "1";
        else
            leftP = "0";

        if (right)
            rightP = "1";
        else
            rightP = "0";

        Debug.Log("Outcode " + upP + ", " + downP + ", " + leftP + ", " + rightP);
    }
        
}
