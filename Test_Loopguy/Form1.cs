﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Test_Loopguy
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void barrelSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.selectedObject = ObjectSelection.Barrel;
        }

        private void potSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.selectedObject = ObjectSelection.Pot;
        }

        private void shrubSmallSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.selectedObject = ObjectSelection.ShrubSmall;
        }

        private void treeSmallSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.selectedObject = ObjectSelection.TreeSmall;
        }

        private void grassSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.selectedTile = TileSelection.Grass;
        }

        private void dirtSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.selectedTile = TileSelection.Dirt;
        }
    }
}
