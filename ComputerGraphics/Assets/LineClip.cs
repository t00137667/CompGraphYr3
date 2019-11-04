using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public class LineClip
    {
        public static bool lineClip(ref Vector2 v, ref Vector2 u)
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
                Vector2 newV = intercept(v, u, 0);

                Outcode newVOutcode = new Outcode(newV);
                if (newVOutcode == inViewPort)
                {
                    v = newV;
                    return lineClip(ref v, ref u);
                }
            }

            if (outcodeV.down)  // Below viewport
            {
                Vector2 newV = intercept(v, u, 1);

                Outcode newVOutcode = new Outcode(newV);
                if (newVOutcode == inViewPort)
                {
                    v = newV;
                    return lineClip(ref v, ref u);
                }
            }

            if (outcodeV.left)  // Left of viewport
            {
                Vector2 newV = intercept(v, u, 2);

                Outcode newVOutcode = new Outcode(newV);
                if (newVOutcode == inViewPort)
                {
                    v = newV;
                    return lineClip(ref v, ref u);
                }
            }

            if (outcodeV.right)  // Right of viewport
            {
                Vector2 newV = intercept(v, u, 3);

                Outcode newVOutcode = new Outcode(newV);
                if (newVOutcode == inViewPort)
                {
                    v = newV;
                    return lineClip(ref v, ref u);
                }
            }

            return false;
        }

        private static Vector2 intercept(Vector2 v, Vector2 u, int edgeId)
        {
            float slope = (u.y - v.y) / (u.x - v.x);

            switch (edgeId)
            {
                case 0: //Top
                    return new Vector2(v.x + (1 / slope) * (1 - v.y), 1);

                case 1: //Bottom
                    return new Vector2(v.x + (1 / slope) * (-1 - v.y), -1);

                case 2: //Left
                    return new Vector2(-1, v.y + slope * (-1 - v.x));

                default:  // Right

                    return new Vector2(1, v.y + slope * (1 - v.x));

            }
        }

        public static List<Vector2Int> Breshenhams(Vector2Int v, Vector2Int u)
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
        private static Vector2Int swapXY(Vector2Int v)
        {
            return new Vector2Int(v.y, v.x);
        }

        private static List<Vector2Int> swapXY(List<Vector2Int> list)
        {
            List<Vector2Int> output_list = new List<Vector2Int>();

            foreach (Vector2Int v in list)
                output_list.Add(swapXY(v));


            return output_list;
        }


        private static List<Vector2Int> negateY(List<Vector2Int> list)
        {
            List<Vector2Int> output_list = new List<Vector2Int>();

            foreach (Vector2Int v in list)
                output_list.Add(negateY(v));


            return output_list;
        }

        private static Vector2Int negateY(Vector2Int v)
        {
            return new Vector2Int(v.x, -v.y);
        }
    }
}
