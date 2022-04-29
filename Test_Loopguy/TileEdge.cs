using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
    
    public enum Direction
    {
        NorthWest, North, NorthEast, East, SouthEast, South, SouthWest, West
    }
    public class TileEdge
    {
        //private bool[,] direction;
        private Edge[,] edges = new Edge[3, 3];
        

        public TileEdge(Texture2D tex, Tile tile)
        {
            for(int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    edges[i, j] = new Edge(tex, tile.position + new Vector2((16*i) - 16, (16*j) - 16));
                }
            } 
        }

        public void UpdateEdges(Tile tile)
        {
            SetEdges(LevelManager.GetEdges(tile));
        }

        public void SetEdges(bool[,] dir)
        {
            //outer corners
            {
                if(dir[0,0] && dir[0,1] && dir[1,0])
                {
                    edges[0, 0].SetVariation(new int[] { 3, 0 });
                }

                if (dir[0, 1] && dir[0, 2] && dir[1, 2])
                {
                    edges[0, 2].SetVariation(new int[] { 3, 1 });
                }

                if (dir[2, 1] && dir[2, 0] && dir[1, 0])
                {
                    edges[2, 0].SetVariation(new int[] { 4, 0 });
                }

                if (dir[2, 1] && dir[2, 2] && dir[1, 2])
                {
                    edges[2, 2].SetVariation(new int[] { 4, 1 });
                }
            }
            
            

            //straight edges
            {
                if (dir[0, 1])
                {
                    edges[0, 1].SetVariation(new int[] { 5, 1 });
                }
                if (dir[2, 1])
                {
                    edges[2, 1].SetVariation(new int[] { 5, 0 });
                }

                if (dir[1, 0])
                {
                    edges[1, 0].SetVariation(new int[] { 1, 1 });
                }
                if (dir[1, 2])
                {
                    edges[1, 2].SetVariation(new int[] { 1, 0 });
                }
            }

            //inner corners
            {
                if (!dir[0, 2] && dir[0, 1])
                {
                    edges[0, 1].SetVariation(new int[] { 2, 1 });
                }
                if (!dir[2, 2] && dir[2, 1])
                {
                    edges[2, 1].SetVariation(new int[] { 0, 1 });
                }
                if (!dir[0, 1] && dir[2, 0] && !dir[1, 2])
                {
                    edges[2, 0].SetVariation(new int[] { 2, 0 });
                }
                if (!dir[0, 2] && dir[1, 2])
                {
                    edges[0, 2].SetVariation(new int[] { 0, 0 });
                }
            }

            int groog = 0;
            foreach(bool d in dir)
            {
                if(!d)
                {
                    groog++;
                }
            }
            if(groog == 8)
            {
                foreach(Edge e in edges)
                {
                    e.active = false;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(Edge e in edges)
            {
                if(e.active)
                {
                    e.Draw(spriteBatch);
                }
            }
        }

    }

    public class Edge
    {
        private Texture2D tex;
        private Rectangle srcRec = new Rectangle(0, 0, 0, 0);
        public bool active = false;
        Vector2 position;

        public Edge(Texture2D tex, Vector2 pos)
        {
            this.tex = tex;
            position = pos;
        }
        public void SetVariation(int[] var)
        {
            active = true;
            srcRec.Size = new Point(16, 16);
            srcRec.X = 16 * var[0];
            srcRec.Y = 16 * var[1];
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, position, srcRec, Color.White);
        }
    }
    
}
