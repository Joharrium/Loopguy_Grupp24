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
        Form1 editorForm;

        public Form1()
        {
            InitializeComponent();
            editorForm = this;
        }

        private void barrelSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.ShelfArchivingSmall);
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
            LevelEditor.SaveLevelToFile(id, LevelManager.ExportObjectList(id), LevelManager.ExportTileList(id), LevelManager.ExportEnemyList());
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

        private void barrelDestructibleSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.BarrelDestructible);
        }

        private void carpetSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectTile(TileSelection.CarpetWorn);
        }

        private void metalTileSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectTile(TileSelection.TileMetal);
        }

        private void wornWallSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectTile(TileSelection.DrywallWorn);
        }

        private void wallMetalSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectTile(TileSelection.WallMetal);
        }

        private void meleeTestSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectEnemy(EnemySelection.MeleeTest);
        }

        private void rangedTestSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectEnemy(EnemySelection.RangedTest);
        }

        private void healSmallSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.HealingSmall);
        }

        private void ammoSmallSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.AmmoSmall);
        }

        private void countDownBox_CheckedChanged(object sender, EventArgs e)
        {
            LevelManager.countTime = !countDownBox.Checked;
        }

        private void BillBoardSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.Billboard);
        }

        private void cabinetSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.Cabinet);
        }

        private void cardboardBoxSmallStackSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.CardboardStackSmall);
        }

        private void counterSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.Counter);
        }

        private void chairOfficeBwSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.ChairOfficeBw);
        }

        private void chairOfficeFwSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.ChairOfficeFw);
        }

        private void deskOfficeSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.DeskOffice);
        }

        private void monitorWallSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.MonitorWall);
        }

        private void serverSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.Server);
        }

        private void shelfArchivingSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.ShelfArchiving);
        }

        private void waterSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectTile(TileSelection.Water);
        }

        private void bigRobotSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectEnemy(EnemySelection.RobotBig);
        }

        private void deskBackwardSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.DeskBackward);
        }

        private void deskForwardSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.DeskForward);
        }

        private void pottedPlantSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.PottedPlant);
        }

        private void bigSinkSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.BigSink);
        }

        private void crateStackSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.CrateStack);
        }

        private void operationEquipmentSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.OperationEquipment);
        }

        private void carryingThingSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.CarryingThing);
        }

        private void niceBookshelfSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.NiceBookshelf);
        }

        private void shootingRangeBenchSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.ShootingRangeBench);
        }

        private void sinkSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.Sink);
        }

        private void sofaSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.Sofa);
        }

        private void cameraSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.Camera);
        }

        private void morgueSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.Morgue);
        }

        private void lockerSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.Locker);
        }

        private void shelfWeirdSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.ShelfWeird);
        }

        private void whiteBoardSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.Whiteboard);
        }

        private void tileWarningSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectTile(TileSelection.Warning);
        }

        private void kitchenCounterSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.KitchenCounter);
        }

        private void canteenTableSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.CanteenTable);
        }

        private void canteenFoodThingSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.CanteenFoodThing);
        }

        private void sofaRightSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.SofaRight);
        }

        private void sofaLeftSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.SofaLeft);
        }

        private void canteenChairRightSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.CanteenChairRight);
        }

        private void canteenChairLeftSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.CanteenChairLeft);
        }

        private void benchSelect_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.Bench);
        }

        private void humanVialsFilled_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.HumanVialsFilled);
        }

        private void humanVialsNoBody_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.HumanVialsNoBody);
        }

        private void humanVialsEmpty_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.HumanVialsEmpty);
        }

        private void carsLeft_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.CarsLeft);
        }

        private void carsRight_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.CarsRight);
        }

        private void smallCarLeft_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.SmallCarLeft);
        }

        private void smallCarRight_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.SmallCarRight);
        }

        private void trashCan_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.TrashCan);
        }

        private void copCarLeft_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.CopCarLeft);
        }

        private void copCarRight_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.CopCarRight);
        }

        private void waterStain_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.WaterStain);
        }

        private void radioactiveStain_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.RadioactiveStain);
        }

        private void bigScreenTele_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.BigScreenTele);
        }

        private void normalScreenTele_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.NormalScreenTele);
        }

        private void bigMonitor_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.BigMonitor);
        }

        private void workstation_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.Workstation);
        }

        private void computerFront_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.ComputerFront);
        }

        private void computerBack_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.ComputerBack);
        }

        private void chest_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.Chest);
        }

        private void chairBack_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.ChairBack);
        }

        private void smallLocker_Click(object sender, EventArgs e)
        {
            LevelEditor.SelectObject(ObjectSelection.SmallLocker);
        }
    }
}
