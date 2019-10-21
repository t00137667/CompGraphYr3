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
    }
}
