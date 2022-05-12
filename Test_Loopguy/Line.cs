﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Test_Loopguy.Content;

namespace Test_Loopguy
{
    internal class Line
    {
        public Vector2 P1, P2;

        public Vector2 IntersectionPoint;

        public Line(Vector2 p1, Vector2 p2)
        {
            P1 = p1;
            P2 = p2;
        }

        public bool RectangleIntersects(Rectangle rect)
        {
            //Does this line hit any of the rectangles sides? Uses LineIntersects method for each side
            Line rleft = new Line(new Vector2(rect.Left, rect.Top), new Vector2(rect.Left, rect.Bottom));
            Line rright = new Line(new Vector2(rect.Right, rect.Top), new Vector2(rect.Right, rect.Bottom));
            Line rtop = new Line(new Vector2(rect.Left, rect.Top), new Vector2(rect.Right, rect.Top));
            Line rbottom = new Line(new Vector2(rect.Left, rect.Bottom), new Vector2(rect.Right, rect.Bottom));

            bool left = LineIntersects(rleft);
            bool right = LineIntersects(rright);
            bool top = LineIntersects(rtop);
            bool bottom = LineIntersects(rbottom);

            //If any of the above are true, the line intersects with rectangle
            if (left || right || top || bottom)
            {
                return true;
            }

            return false;
        }

        public bool LineIntersects(Line o)
        {
            //Calculates the direction of the lines
            float uA = ((o.P2.X - o.P1.X) * (P1.Y - o.P1.Y) - (o.P2.Y - o.P1.Y) * (P1.X - o.P1.X)) / ((o.P2.Y - o.P1.Y) * (P2.X - P1.X) - (o.P2.X - o.P1.X) * (P2.Y - P1.Y));
            float uB = ((P2.X - P1.X) * (P1.Y - o.P1.Y) - (P2.Y - P1.Y) * (P1.X - o.P1.X)) / ((o.P2.Y - o.P1.Y) * (P2.X - P1.X) - (o.P2.X - o.P1.X) * (P2.Y - P1.Y));

            //If uA and uB are between 0-1, lines are colliding
            if (uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1)
            {
                float intersectionX = P1.X + (uA * (P2.X - P1.X));
                float intersectionY = P1.Y + (uA * (P2.Y - P1.Y));
                IntersectionPoint = new Vector2(intersectionX, intersectionY);

                return true;
            }

            return false;
        }

     }
}
