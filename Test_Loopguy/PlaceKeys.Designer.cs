
namespace Test_Loopguy
{
    partial class PlaceKeys
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
            this.permanentCheck = new System.Windows.Forms.CheckBox();
            this.keyId = new System.Windows.Forms.Label();
            this.createButton = new System.Windows.Forms.Button();
            this.doorID = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.doorID)).BeginInit();
            this.SuspendLayout();
            // 
            // permanentCheck
            // 
            this.permanentCheck.AutoSize = true;
            this.permanentCheck.Location = new System.Drawing.Point(22, 63);
            this.permanentCheck.Name = "permanentCheck";
            this.permanentCheck.Size = new System.Drawing.Size(89, 19);
            this.permanentCheck.TabIndex = 11;
            this.permanentCheck.Text = "Permanent?";
            this.permanentCheck.UseVisualStyleBackColor = true;
            // 
            // keyId
            // 
            this.keyId.AutoSize = true;
            this.keyId.Location = new System.Drawing.Point(9, 7);
            this.keyId.Name = "keyId";
            this.keyId.Size = new System.Drawing.Size(40, 15);
            this.keyId.TabIndex = 10;
            this.keyId.Text = "Key ID";
            // 
            // createButton
            // 
            this.createButton.Location = new System.Drawing.Point(56, 88);
            this.createButton.Name = "createButton";
            this.createButton.Size = new System.Drawing.Size(75, 23);
            this.createButton.TabIndex = 9;
            this.createButton.Text = "Create";
            this.createButton.UseVisualStyleBackColor = true;
            this.createButton.Click += new System.EventHandler(this.createButton_Click);
            // 
            // doorID
            // 
            this.doorID.Location = new System.Drawing.Point(9, 34);
            this.doorID.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.doorID.Name = "doorID";
            this.doorID.Size = new System.Drawing.Size(120, 23);
            this.doorID.TabIndex = 8;
            this.doorID.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // PlaceKeys
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(136, 120);
            this.Controls.Add(this.permanentCheck);
            this.Controls.Add(this.keyId);
            this.Controls.Add(this.createButton);
            this.Controls.Add(this.doorID);
            this.Name = "PlaceKeys";
            this.Text = "PlaceKeys";
            ((System.ComponentModel.ISupportInitialize)(this.doorID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox permanentCheck;
        private System.Windows.Forms.Label keyId;
        private System.Windows.Forms.Button createButton;
        private System.Windows.Forms.NumericUpDown doorID;
    }
}