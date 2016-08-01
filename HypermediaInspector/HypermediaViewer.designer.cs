namespace HypermediaInspector
{
    partial class HypermediaViewer
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.JsonTreeView = new System.Windows.Forms.TreeView();
            this.SuspendLayout();
            // 
            // JsonTreeView
            // 
            this.JsonTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.JsonTreeView.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.JsonTreeView.Location = new System.Drawing.Point(0, 0);
            this.JsonTreeView.Name = "JsonTreeView";
            this.JsonTreeView.Size = new System.Drawing.Size(563, 336);
            this.JsonTreeView.TabIndex = 0;
            this.JsonTreeView.DrawNode += new System.Windows.Forms.DrawTreeNodeEventHandler(this.TreeView_DrawNode);
            this.JsonTreeView.DoubleClick += new System.EventHandler(this.JsonTreeView_DoubleClick);
            // 
            // HypermediaViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.JsonTreeView);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "HypermediaViewer";
            this.Size = new System.Drawing.Size(563, 336);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView JsonTreeView;
    }
}
