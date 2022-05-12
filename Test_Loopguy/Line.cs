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

        public Vector2 IntersectionPoint;

        public Line(Vector2 p1, Vector2 p2)
        {
            P1 = p1;
            P2 = p2;
        }

        public Line LineFromIntersect(Rectangle rect)
        {
            Line rleft = new Line(new Vector2(rect.Left, rect.Top), new Vector2(rect.Left, rect.Bottom));
            Line rright = new Line(new Vector2(rect.Right, rect.Top), new Vector2(rect.Right, rect.Bottom));
            Line rtop = new Line(new Vector2(rect.Left, rect.Top), new Vector2(rect.Right, rect.Top));
            Line rbottom = new Line(new Vector2(rect.Left, rect.Bottom), new Vector2(rect.Right, rect.Bottom));

            Line cLeft = this;
            Line cRight = this;
            Line cTop = this;
            Line cBottom = this;

            Line smallLine = this;
            //double smallestline = smallLine.Length();

            bool left = LineIntersects(rleft);
            if (left)
            {
                cLeft = new Line(P1, new Vector2(IntersectionPoint.X, IntersectionPoint.Y));
                if (cLeft.Length() < smallLine.Length())
                {
                    smallLine = cLeft;
                    //smallestline = smallLine.Length();
                }
            }
            {
                /*
                            if(IntersectionPoint != null && IntersectionPoint != Vector2.Zero)
                            {
                                cLeft = new Line(P1, IntersectionPoint);
                                if(cLeft.Length() < smallLine.Length())
                                {
                                    smallLine = cLeft;
                                }

                            }
                            */
            }



            bool right = LineIntersects(rright);
            if (right)
            {
                cRight = new Line(P1, new Vector2(IntersectionPoint.X, IntersectionPoint.Y));
                if (cRight.Length() < smallLine.Length())
                {
                    smallLine = cRight;
                    //smallestline = smallLine.Length();
                }
            }

            {
                /*
                 if (IntersectionPoint != null && IntersectionPoint != cLeft.P2)
                    {
                        cRight = new Line(P1, IntersectionPoint);
                        if(cRight.Length() < smallLine.Length())
                        {
                            smallLine = cRight;
                        }
                    }
                 */
            }


            bool top = LineIntersects(rtop);
            if (top)
            {
                cTop = new Line(P1, new Vector2(IntersectionPoint.X, IntersectionPoint.Y));
                if (cTop.Length() < smallLine.Length())
                {
                    smallLine = cTop;
                    //smallestline = smallLine.Length();
                }
            }

            bool bottom = LineIntersects(rbottom);
            if (bottom)
            {
                cBottom = new Line(P1, new Vector2(IntersectionPoint.X, IntersectionPoint.Y));
                if (cBottom.Length() < smallLine.Length())
                {
                    smallLine = cBottom;
                    //smallestline = smallLine.Length();
                }
            }

            P2 = smallLine.P2;
            IntersectionPoint = smallLine.P2;


            //If any of the above are true, the line intersects with rectangle

                return smallLine;

        }

        public bool RectangleIntersects(Rectangle rect)
        {
            //Does this line hit any of the rectangles sides? Uses LineIntersects method for each side
            Line rleft = new Line(new Vector2(rect.Left, rect.Top), new Vector2(rect.Left, rect.Bottom));
            Line rright = new Line(new Vector2(rect.Right, rect.Top), new Vector2(rect.Right, rect.Bottom));
            Line rtop = new Line(new Vector2(rect.Left, rect.Top), new Vector2(rect.Right, rect.Top));
            Line rbottom = new Line(new Vector2(rect.Left, rect.Bottom), new Vector2(rect.Right, rect.Bottom));

            Line cLeft = this;
            Line cRight = this;
            Line cTop = this;
            Line cBottom = this;

            Line smallLine = this;
            //double smallestline = smallLine.Length();

            bool left = LineIntersects(rleft);
            if(left)
            {
                cLeft = new Line(P1, new Vector2(IntersectionPoint.X, IntersectionPoint.Y));
                if (cLeft.Length() < smallLine.Length())
                {
                    smallLine = cLeft;
                    //smallestline = smallLine.Length();
                }
            }
            {
                /*
                            if(IntersectionPoint != null && IntersectionPoint != Vector2.Zero)
                            {
                                cLeft = new Line(P1, IntersectionPoint);
                                if(cLeft.Length() < smallLine.Length())
                                {
                                    smallLine = cLeft;
                                }

                            }
                            */
            }



            bool right = LineIntersects(rright);
            if(right)
            {
                cRight = new Line(P1, new Vector2(IntersectionPoint.X, IntersectionPoint.Y));
                if (cRight.Length() < smallLine.Length())
                {
                    smallLine = cRight;
                    //smallestline = smallLine.Length();
                }
            }

            {
                /*
                 if (IntersectionPoint != null && IntersectionPoint != cLeft.P2)
                    {
                        cRight = new Line(P1, IntersectionPoint);
                        if(cRight.Length() < smallLine.Length())
                        {
                            smallLine = cRight;
                        }
                    }
                 */
            }

            
            bool top = LineIntersects(rtop);
            if(top)
            {
                cTop = new Line(P1, new Vector2(IntersectionPoint.X, IntersectionPoint.Y));
                if (cTop.Length() < smallLine.Length())
                {
                    smallLine = cTop;
                    //smallestline = smallLine.Length();
                }
            }

            bool bottom = LineIntersects(rbottom);
            if(bottom)
            {
                cBottom = new Line(P1, new Vector2(IntersectionPoint.X, IntersectionPoint.Y));
                if (cBottom.Length() < smallLine.Length())
                {
                    smallLine = cBottom;
                    //smallestline = smallLine.Length();
                }
            }

            P2 = smallLine.P2;
            IntersectionPoint = smallLine.P2;


            //If any of the above are true, the line intersects with rectangle
            if (left || right || top || bottom)
            {
                return true;
            }

            return false;
        }

        public double Length()
        {
            return Math.Abs(Math.Sqrt(Math.Pow(P1.X - P2.X, 2) + Math.Pow(P1.Y - P2.Y, 2)));
        }



        public bool LineIntersects(Line o)
        {
            //Calculates the direction of the lines
            float uA = ((o.P2.X - o.P1.X) * (P1.Y - o.P1.Y) - (o.P2.Y - o.P1.Y) * (P1.X - o.P1.X)) / ((o.P2.Y - o.P1.Y) * (P2.X - P1.X) - (o.P2.X - o.P1.X) * (P2.Y - P1.Y));
            float uB = ((P2.X - P1.X) * (P1.Y - o.P1.Y) - (P2.Y - P1.Y) * (P1.X - o.P1.X)) / ((o.P2.Y - o.P1.Y) * (P2.X - P1.X) - (o.P2.X - o.P1.X) * (P2.Y - P1.Y));

            //If uA and uB are between 0-1, lines are colliding
            if (uA >= 0 && uA <= 1 && uB >= 0 && uB <= 1)
            {
                float intersectionX = P1.X + (uA * (P2.X - P1.X)) - 2;
                float intersectionY = P1.Y + (uA * (P2.Y - P1.Y));
                IntersectionPoint = new Vector2(intersectionX, intersectionY);

                return true;
            }

            return false;
        }

     }
}

