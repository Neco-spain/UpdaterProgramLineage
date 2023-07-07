namespace Updater
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnAtualizar = new System.Windows.Forms.Panel();
            this.logo = new System.Windows.Forms.PictureBox();
            this.btnStart = new System.Windows.Forms.Panel();
            this.header = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.porcentagem02 = new System.Windows.Forms.Label();
            this.velocidade = new System.Windows.Forms.Label();
            this.status = new System.Windows.Forms.Label();
            this.porcentagem01 = new System.Windows.Forms.Label();
            this.bar02 = new System.Windows.Forms.PictureBox();
            this.bar01 = new System.Windows.Forms.PictureBox();
            this.labaelextrando = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.logo)).BeginInit();
            this.header.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bar02)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bar01)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAtualizar
            // 
            this.btnAtualizar.BackColor = System.Drawing.Color.Transparent;
            this.btnAtualizar.BackgroundImage = global::Updater.Properties.Resources.reload3;
            this.btnAtualizar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAtualizar.Location = new System.Drawing.Point(486, 522);
            this.btnAtualizar.Name = "btnAtualizar";
            this.btnAtualizar.Size = new System.Drawing.Size(50, 50);
            this.btnAtualizar.TabIndex = 2;
            this.btnAtualizar.Click += new System.EventHandler(this.VereficandoSeExisteAtualizacaoClick);
            // 
            // logo
            // 
            this.logo.BackColor = System.Drawing.Color.Transparent;
            this.logo.BackgroundImage = global::Updater.Properties.Resources.logo1;
            this.logo.Location = new System.Drawing.Point(310, 61);
            this.logo.Name = "logo";
            this.logo.Size = new System.Drawing.Size(387, 182);
            this.logo.TabIndex = 2;
            this.logo.TabStop = false;
            this.logo.Click += new System.EventHandler(this.AbrirSite);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.Transparent;
            this.btnStart.BackgroundImage = global::Updater.Properties.Resources.playgame2;
            this.btnStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnStart.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStart.Location = new System.Drawing.Point(538, 438);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(150, 150);
            this.btnStart.TabIndex = 1;
            this.btnStart.Click += new System.EventHandler(this.AbrirGame);
            // 
            // header
            // 
            this.header.BackColor = System.Drawing.Color.Transparent;
            this.header.BackgroundImage = global::Updater.Properties.Resources.navbar;
            this.header.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.header.Controls.Add(this.panel1);
            this.header.Controls.Add(this.btnClose);
            this.header.ImeMode = System.Windows.Forms.ImeMode.Alpha;
            this.header.Location = new System.Drawing.Point(0, 0);
            this.header.Name = "header";
            this.header.Size = new System.Drawing.Size(700, 60);
            this.header.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BackgroundImage = global::Updater.Properties.Resources.aside_block_discord;
            this.panel1.Location = new System.Drawing.Point(5, 7);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(45, 45);
            this.panel1.TabIndex = 3;
            this.panel1.Click += new System.EventHandler(this.AbrirDiscord);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.BackgroundImage = global::Updater.Properties.Resources.close;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.Location = new System.Drawing.Point(644, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(50, 50);
            this.btnClose.TabIndex = 2;
            this.btnClose.Click += new System.EventHandler(this.Minimizar);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.BackgroundImage = global::Updater.Properties.Resources.fotterprcess1;
            this.panel2.Controls.Add(this.labaelextrando);
            this.panel2.Controls.Add(this.porcentagem02);
            this.panel2.Controls.Add(this.velocidade);
            this.panel2.Controls.Add(this.status);
            this.panel2.Controls.Add(this.porcentagem01);
            this.panel2.Controls.Add(this.bar02);
            this.panel2.Controls.Add(this.bar01);
            this.panel2.Location = new System.Drawing.Point(0, 381);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(700, 220);
            this.panel2.TabIndex = 3;
            // 
            // porcentagem02
            // 
            this.porcentagem02.AutoSize = true;
            this.porcentagem02.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.porcentagem02.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.porcentagem02.ForeColor = System.Drawing.Color.White;
            this.porcentagem02.Location = new System.Drawing.Point(411, 191);
            this.porcentagem02.Name = "porcentagem02";
            this.porcentagem02.Size = new System.Drawing.Size(0, 13);
            this.porcentagem02.TabIndex = 6;
            // 
            // velocidade
            // 
            this.velocidade.AutoSize = true;
            this.velocidade.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.velocidade.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.velocidade.ForeColor = System.Drawing.Color.White;
            this.velocidade.Location = new System.Drawing.Point(333, 130);
            this.velocidade.Name = "velocidade";
            this.velocidade.Size = new System.Drawing.Size(0, 13);
            this.velocidade.TabIndex = 6;
            // 
            // status
            // 
            this.status.AutoSize = true;
            this.status.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.status.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.status.ForeColor = System.Drawing.Color.White;
            this.status.Location = new System.Drawing.Point(39, 130);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(0, 13);
            this.status.TabIndex = 6;
            // 
            // porcentagem01
            // 
            this.porcentagem01.AutoSize = true;
            this.porcentagem01.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.porcentagem01.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.porcentagem01.ForeColor = System.Drawing.Color.White;
            this.porcentagem01.Location = new System.Drawing.Point(412, 150);
            this.porcentagem01.Name = "porcentagem01";
            this.porcentagem01.Size = new System.Drawing.Size(0, 13);
            this.porcentagem01.TabIndex = 6;
            // 
            // bar02
            // 
            this.bar02.BackgroundImage = global::Updater.Properties.Resources.barraprocessocomplete;
            this.bar02.Location = new System.Drawing.Point(24, 192);
            this.bar02.Name = "bar02";
            this.bar02.Size = new System.Drawing.Size(369, 12);
            this.bar02.TabIndex = 5;
            this.bar02.TabStop = false;
            // 
            // bar01
            // 
            this.bar01.BackgroundImage = global::Updater.Properties.Resources.barraprocesso;
            this.bar01.Location = new System.Drawing.Point(27, 150);
            this.bar01.Name = "bar01";
            this.bar01.Size = new System.Drawing.Size(369, 12);
            this.bar01.TabIndex = 4;
            this.bar01.TabStop = false;
            // 
            // labaelextrando
            // 
            this.labaelextrando.AutoSize = true;
            this.labaelextrando.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.labaelextrando.Location = new System.Drawing.Point(36, 173);
            this.labaelextrando.Name = "labaelextrando";
            this.labaelextrando.Size = new System.Drawing.Size(0, 13);
            this.labaelextrando.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::Updater.Properties.Resources.bg;
            this.ClientSize = new System.Drawing.Size(700, 600);
            this.Controls.Add(this.btnAtualizar);
            this.Controls.Add(this.logo);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.header);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.logo)).EndInit();
            this.header.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bar02)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bar01)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel header;
        private System.Windows.Forms.Panel btnStart;
        private System.Windows.Forms.PictureBox logo;
        private System.Windows.Forms.Panel btnAtualizar;
        private System.Windows.Forms.Panel btnClose;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox bar01;
        private System.Windows.Forms.PictureBox bar02;
        private System.Windows.Forms.Label porcentagem01;
        private System.Windows.Forms.Label porcentagem02;
        private System.Windows.Forms.Label velocidade;
        private System.Windows.Forms.Label status;
        private System.Windows.Forms.Label labaelextrando;
    }
}

