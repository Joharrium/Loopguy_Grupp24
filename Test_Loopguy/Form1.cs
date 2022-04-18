using System;
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

        private void grayBrickSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.selectedTile = TileSelection.GrayBrick;
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

        private void tilesCheckeredGraySelect_Click(object sender, EventArgs e)
        {
            LevelEditor.currentSelection = Selection.Tile;
            LevelEditor.selectedTile = TileSelection.TilesCheckeredGray;
        }

        private void tilesCheckeredBrownSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.currentSelection = Selection.Tile;
            LevelEditor.selectedTile = TileSelection.TilesCheckeredBrown;
        }

        private void tileBigLightSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.currentSelection = Selection.Tile;
            LevelEditor.selectedTile = TileSelection.TilesBigLight;
        }

        private void tileBigDarkSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.currentSelection = Selection.Tile;
            LevelEditor.selectedTile = TileSelection.TilesBigDark;
        }

        private void setCameraSize_Click(object sender, EventArgs e)
        {
            LevelManager.SetBounds(Int32.Parse(cameraX.Text), Int32.Parse(cameraY.Text));
        }

        private void goToLevelButton_Click(object sender, EventArgs e)
        {
            LevelManager.StartLevelTransition(Int32.Parse(goToLevelWithID.Text), EntityManager.player, new Microsoft.Xna.Framework.Vector2(64, 64));
        }

        private void doorSelect_Click(object sender, EventArgs e)
        {
            PlaceDoor placeDoorDialog = new PlaceDoor(ObjectSelection.DoorWood);
            placeDoorDialog.ShowDialog();
        }

        private void keycardRedSelect_Click(object sender, EventArgs e)
        {
            PlaceKeys placeKeyDialod = new PlaceKeys();
            placeKeyDialod.ShowDialog();
        }

        private void doorSlidingSelect_Click(object sender, EventArgs e)
        {
            PlaceDoor placeDoorDialog = new PlaceDoor(ObjectSelection.DoorSliding);
            placeDoorDialog.ShowDialog();
        }
    }
}
