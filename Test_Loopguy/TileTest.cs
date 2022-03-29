﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
    public static class TileManager
    {
        public static List<Tile> tiles;
        public static void Initialization() 
        {
            tiles = new List<Tile>();
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    tiles.Add(new Tile(new Vector2(i*36, j*36)));
                }
            }


            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    tiles.Add(new Tile(new Vector2(i * 36 + 256, j * 36 + 0)));
                }
            }

        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach(Tile t in tiles)
            {
                t.Draw(spriteBatch);
            }
            
        }

        public static bool CheckCollision(Vector2 position)
        {
            foreach(Tile t in tiles)
            {
                if(t.hitBox.Contains(position))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
