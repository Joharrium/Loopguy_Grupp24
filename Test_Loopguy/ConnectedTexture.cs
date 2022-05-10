using System;
using System.Collections.Generic;
using System.Text;

namespace Test_Loopguy
{
    public static class ConnectedTextureCalc
    {
        public static int GetConnections(Tile tile)
        {
            bool[,] dir = LevelManager.GetEdges(tile);

            int variation = 10;
            bool up = !dir[1, 0];
            bool left = !dir[0, 1];
            bool down = !dir[1, 2];
            bool right = !dir[2, 1];

            if(right && !left && !up && !down)
            {
                variation = 0;
            }

            if(right && left && !up && !down)
            {
                variation = 1;
            }

            if(!right && left && !up && !down)
            {
                variation = 2;
            }

            if(!right && !left && up && !down)
            {
                variation = 3;
            }
            if(!right && !left && up && down)
            {
                variation = 4;
            }
            if(!right && !left && !up && down)
            {
                variation = 5;
            }
            if(right && !left && !up && down)
            {
                variation = 6;
            }
            if (!right && left && !up && down)
            {
                variation = 7;
            }
            if(right && !left && up && !down)
            {
                variation = 8;
            }
            if(!right && left && up && !down)
            {
                variation = 9;
            }
            if(right && left && up && !down)
            {
                variation = 11;
            }
            if(right && left && !up && down)
            {
                variation = 12;
            }
            if(!right && left && up && down)
            {
                variation = 14;
            }
            if(right && !left && up && down)
            {
               variation = 13;
            }


            return variation;
        }
    }
}
