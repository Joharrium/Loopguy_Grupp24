using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Test_Loopguy.Content;

namespace Test_Loopguy
{
    public class Line
    {
        public Vector2 P1, P2;

        public Vector2 intersectionPoint;

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

            Vector2 intersectLeft = LineIntersects(rleft);
            Vector2 intersectRight = LineIntersects(rright);
            Vector2 intersectTop = LineIntersects(rtop);
            Vector2 intersectBottom = LineIntersects(rbottom);

            float distanceLeft = Vector2.Distance(P1, intersectLeft);
            float distanceRight = Vector2.Distance(P1, intersectRight);
            float distanceTop = Vector2.Distance(P1, intersectTop);
            float distanceBottom = Vector2.Distance(P1, intersectBottom);

            if(distanceLeft < distanceRight && distanceLeft < distanceTop && distanceLeft < distanceBottom)
            {
                intersectionPoint = intersectLeft;
                return true;
            }
            else if (distanceRight < distanceLeft && distanceRight < distanceTop && distanceTop < distanceBottom)
            {
                intersectionPoint = intersectRight;
                return true;
            }
            else if (distanceTop < distanceLeft && distanceTop < distanceRight && distanceTop < distanceBottom)
            {
                intersectionPoint = intersectTop;
                return true;
            }
            else if (distanceBottom < distanceLeft && distanceBottom < distanceRight && distanceBottom < distanceTop)
            {
                intersectionPoint = intersectBottom;
                return true;
            }
            else
            {
                return false;
            }
        }

        public Vector2 LineIntersects(Line o)
        {
            //Calculates the direction of the lines
            float uA = ((o.P2.X - o.P1.X) * (P1.Y - o.P1.Y) - (o.P2.Y - o.P1.Y) * (P1.X - o.P1.X)) / ((o.P2.Y - o.P1.Y) * (P2.X - P1.X) - (o.P2.X - o.P1.X) * (P2.Y - P1.Y));
            float uB = ((P2.X - P1.X) * (P1.Y - o.P1.Y) - (P2.Y - P1.Y) * (P1.X - o.P1.X)) / ((o.P2.Y - o.P1.Y) * (P2.X - P1.X) - (o.P2.X - o.P1.X) * (P2.Y - P1.Y));

            //If uA and uB are between 0-1, lines are colliding
            if (uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1)
            {
                float intersectionX = P1.X + (uA * (P2.X - P1.X));
                float intersectionY = P1.Y + (uA * (P2.Y - P1.Y));
                Vector2 intersection = new Vector2(intersectionX, intersectionY);

                return intersection;
            }

            return Vector2.Zero;
        }

     }
}

