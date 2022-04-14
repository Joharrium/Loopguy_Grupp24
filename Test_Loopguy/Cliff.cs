﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Test_Loopguy
{
    public class Cliff : Tile
    {
        protected Edges edges = new Edges();

        public Cliff(Vector2 position) : base(position)
        {
            this.position = position;
        }

        public void RefreshEdges(Tile[,] tiles)
        {
            sourceRectangle = edges.RefreshEdges(tiles, this);
        }
    }
    public class CliffGray : Cliff
    {
        public CliffGray(Vector2 position) : base(position)
        {
            this.position = position;

            texture = TexMGR.cliffGray;

            //make cliffatlas
            
            sourceRectangle = new Rectangle(16, 32, 16, 16);
        }

        
    }
}
