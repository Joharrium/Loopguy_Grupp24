using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Test_Loopguy
{
    public partial class PlaceDoor : Form
    {
        public PlaceDoor()
        {
            InitializeComponent();
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            LevelEditor.SetDoorParams((int)doorID.Value);
            LevelEditor.selectedObject = ObjectSelection.DoorWood;
            LevelEditor.currentSelection = Selection.Object;
            this.Close();
        }
    }
}
