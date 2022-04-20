using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Test_Loopguy
{
    public partial class PlaceKeys : Form
    {
        public PlaceKeys()
        {
            InitializeComponent();
        }

        private void createButton_Click(object sender, EventArgs e)
        {
            LevelEditor.SetKeyParams((int)doorID.Value, permanentCheck.Checked);
            LevelEditor.selectedObject = ObjectSelection.KeycardRed;
            LevelEditor.currentSelection = Selection.Object;
            this.Close();
        }
    }
}
