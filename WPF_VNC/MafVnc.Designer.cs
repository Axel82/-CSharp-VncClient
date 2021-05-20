namespace WPF_VNC
{
    partial class MafVnc
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.remoteDesktop = new VncSharp.RemoteDesktop();
            this.SuspendLayout();
            // 
            // remoteDesktop
            // 
            this.remoteDesktop.AutoScroll = true;
            this.remoteDesktop.AutoScrollMinSize = new System.Drawing.Size(608, 427);
            this.remoteDesktop.Location = new System.Drawing.Point(0, 0);
            this.remoteDesktop.Name = "remoteDesktop";
            this.remoteDesktop.Size = new System.Drawing.Size(800, 480);
            this.remoteDesktop.TabIndex = 0;
            // 
            // MafVnc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.remoteDesktop);
            this.Name = "MafVnc";
            this.Size = new System.Drawing.Size(805, 485);
            this.ResumeLayout(false);

        }

        #endregion

        private VncSharp.RemoteDesktop remoteDesktop;
    }
}
