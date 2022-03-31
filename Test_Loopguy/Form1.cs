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
            LevelEditor.currentSelection = Selection.Object;
        }

        private void potSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.selectedObject = ObjectSelection.Pot;
            LevelEditor.currentSelection = Selection.Object;
        }

        private void shrubSmallSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.selectedObject = ObjectSelection.ShrubSmall;
            LevelEditor.currentSelection = Selection.Object;
        }

        private void treeSmallSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.selectedObject = ObjectSelection.TreeSmall;
            LevelEditor.currentSelection = Selection.Object;
        }

        private void grassSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.selectedTile = TileSelection.Grass;
            LevelEditor.currentSelection = Selection.Tile;
        }

        private void dirtSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.selectedTile = TileSelection.Dirt;
            LevelEditor.currentSelection = Selection.Tile;
        }

        private void boxSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.selectedObject = ObjectSelection.Box;
            LevelEditor.currentSelection = Selection.Object;
        }

        private void boxOpenSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.selectedObject = ObjectSelection.BoxOpen;
            LevelEditor.currentSelection = Selection.Object;
        }

        private void shrubBigSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.selectedObject = ObjectSelection.ShrubBig;
            LevelEditor.currentSelection = Selection.Object;
        }

        private void treeBigSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.selectedObject = ObjectSelection.TreeBig;
            LevelEditor.currentSelection = Selection.Object;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            int id = Int32.Parse(mapIdBox.Text);
            //LevelManager.ExportObjectList(id);
            //for each object in list, write a line bla bla bla
            //for each tile in array, write a char bla bla bla
            //save to maps/levelID/ bla bla bla
            LevelEditor.SaveLevelToFile(id, LevelManager.ExportObjectList(id), LevelManager.ExportTileList(id));
        }

        private void saveMapSize_Click(object sender, EventArgs e)
        {
            int x = Int32.Parse(mapWidth.Text);
            int y = Int32.Parse(mapHeight.Text);
            LevelManager.SetMapSize(x, y);
        }
    }
}
