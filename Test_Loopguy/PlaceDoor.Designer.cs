
namespace Test_Loopguy
{
    partial class PlaceDoor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.doorID = new System.Windows.Forms.NumericUpDown();
            this.createButton = new System.Windows.Forms.Button();
            this.requiredKey = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.doorID)).BeginInit();
            this.SuspendLayout();
            // 
            // doorID
            // 
            this.doorID.Location = new System.Drawing.Point(12, 36);
            this.doorID.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.doorID.Name = "doorID";
            this.doorID.Size = new System.Drawing.Size(120, 23);
            this.doorID.TabIndex = 0;
            this.doorID.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // createButton
            // 
            this.createButton.Location = new System.Drawing.Point(60, 65);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(75, 23);
            this.createButton.TabIndex = 2;
            this.createButton.Text = "Create";
            this.createButton.UseVisualStyleBackColor = true;
            this.createButton.Click += new System.EventHandler(this.createButton_Click);
            // 
            // requiredKey
            // 
            this.requiredKey.AutoSize = true;
            this.requiredKey.Location = new System.Drawing.Point(12, 9);
            this.requiredKey.Name = "requiredKey";
            this.requiredKey.Size = new System.Drawing.Size(76, 15);
            this.requiredKey.TabIndex = 3;
            this.requiredKey.Text = "Required Key";
            // 
            // PlaceDoor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(147, 96);
            this.Controls.Add(this.requiredKey);
            this.Controls.Add(this.createButton);
            this.Controls.Add(this.doorID);
            this.Name = "PlaceDoor";
            this.Text = "Form2";
            ((System.ComponentModel.ISupportInitialize)(this.doorID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown doorID;
        private System.Windows.Forms.Button createButton;
        private System.Windows.Forms.Label requiredKey;
    }
}