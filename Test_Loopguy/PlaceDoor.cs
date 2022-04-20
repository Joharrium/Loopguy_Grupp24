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
        ObjectSelection type;
        public PlaceDoor(ObjectSelection type)
        {
            InitializeComponent();
            this.type = type;
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            LevelEditor.SetDoorParams((int)doorID.Value);
            LevelEditor.selectedObject = type;
            LevelEditor.currentSelection = Selection.Object;
            this.Close();
        }

        private void requiredKey_Click(object sender, EventArgs e)
        {

        }

        private void doorID_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
