using System.Configuration;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;
using Monopoly.Properties;
using Monopoly.Model;

namespace Monopoly
{
    partial class optionsForm : INotifyPropertyChanged
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
            this.components = new System.ComponentModel.Container();
            this.resolutionLabel = new System.Windows.Forms.Label();
            this.volumeLabel = new System.Windows.Forms.Label();
            this.templateLabel = new System.Windows.Forms.Label();
            this.volumeTrackBar = new System.Windows.Forms.TrackBar();
            this.resolutionListBox = new System.Windows.Forms.ListBox();
            this.templateListBox = new System.Windows.Forms.ListBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.exitButton = new System.Windows.Forms.Button();
            this.optionsFormBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.optionsFormBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.volumeTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.optionsFormBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.optionsFormBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // resolutionLabel
            // 
            this.resolutionLabel.AutoSize = true;
            this.resolutionLabel.Location = new System.Drawing.Point(28, 68);
            this.resolutionLabel.Name = "resolutionLabel";
            this.resolutionLabel.Size = new System.Drawing.Size(63, 13);
            this.resolutionLabel.TabIndex = 0;
            this.resolutionLabel.Text = "Resolution :";
            // 
            // volumeLabel
            // 
            this.volumeLabel.AutoSize = true;
            this.volumeLabel.Location = new System.Drawing.Point(28, 156);
            this.volumeLabel.Name = "volumeLabel";
            this.volumeLabel.Size = new System.Drawing.Size(48, 13);
            this.volumeLabel.TabIndex = 1;
            this.volumeLabel.Text = "Volume :";
            this.volumeLabel.Click += new System.EventHandler(this.volumeLabel_Click);
            // 
            // templateLabel
            // 
            this.templateLabel.AutoSize = true;
            this.templateLabel.Location = new System.Drawing.Point(28, 248);
            this.templateLabel.Name = "templateLabel";
            this.templateLabel.Size = new System.Drawing.Size(57, 13);
            this.templateLabel.TabIndex = 2;
            this.templateLabel.Text = "Template :";
            // 
            // volumeTrackBar
            // 
            this.volumeTrackBar.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::Monopoly.Properties.Settings.Default, "Volume", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.volumeTrackBar.Location = new System.Drawing.Point(93, 156);
            this.volumeTrackBar.Name = "volumeTrackBar";
            this.volumeTrackBar.Size = new System.Drawing.Size(215, 45);
            this.volumeTrackBar.TabIndex = 3;
            this.volumeTrackBar.Value = global::Monopoly.Properties.Settings.Default.Volume;
            // 
            // resolutionListBox
            // 
            this.resolutionListBox.FormattingEnabled = true;
            this.resolutionListBox.Items.AddRange(Tools.ConvertCollectionToList(Settings.Default.Resolution).ToArray());
            this.resolutionListBox.SelectedIndex = Settings.Default.ResolutionSelected;
            this.resolutionListBox.Location = new System.Drawing.Point(93, 64);
            this.resolutionListBox.Name = "resolutionListBox";
            this.resolutionListBox.Size = new System.Drawing.Size(215, 30);
            this.resolutionListBox.TabIndex = 4;
            this.resolutionListBox.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // templateListBox
            // 
            this.templateListBox.FormattingEnabled = true;
            this.templateListBox.Items.AddRange(new object[] {
            "HELLO",
            "WORLD"});
            this.templateListBox.Location = new System.Drawing.Point(93, 244);
            this.templateListBox.Name = "templateListBox";
            this.templateListBox.Size = new System.Drawing.Size(215, 30);
            this.templateListBox.TabIndex = 5;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(31, 326);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 6;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            // 
            // exitButton
            // 
            this.exitButton.Location = new System.Drawing.Point(233, 326);
            this.exitButton.Name = "exitButton";
            this.exitButton.Size = new System.Drawing.Size(75, 23);
            this.exitButton.TabIndex = 7;
            this.exitButton.Text = "Exit";
            this.exitButton.UseVisualStyleBackColor = true;
            this.exitButton.Click += new System.EventHandler(this.exitButton_Click);
            // 
            // optionsFormBindingSource
            // 
            this.optionsFormBindingSource.DataSource = typeof(Monopoly.optionsForm);
            // 
            // optionsFormBindingSource1
            // 
            this.optionsFormBindingSource1.DataSource = typeof(Monopoly.optionsForm);
            // 
            // optionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 416);
            this.Controls.Add(this.exitButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.templateListBox);
            this.Controls.Add(this.resolutionListBox);
            this.Controls.Add(this.volumeTrackBar);
            this.Controls.Add(this.templateLabel);
            this.Controls.Add(this.volumeLabel);
            this.Controls.Add(this.resolutionLabel);
            this.Name = "optionsForm";
            this.Text = "Options";
            this.Load += new System.EventHandler(this.optionsForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.volumeTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.optionsFormBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.optionsFormBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label resolutionLabel;
        private System.Windows.Forms.Label volumeLabel;
        private System.Windows.Forms.Label templateLabel;
        private System.Windows.Forms.TrackBar volumeTrackBar;
        private System.Windows.Forms.ListBox resolutionListBox;
        private System.Windows.Forms.ListBox templateListBox;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button exitButton;
        public event PropertyChangedEventHandler PropertyChanged;

        private int _volumeValue;
        public int volumeValue
        {
            get { return _volumeValue;  }
            set
            {
                if(_volumeValue != value)
                {
                    _volumeValue = value;
                    OnPropertyChanged();
                }
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        private System.Windows.Forms.BindingSource optionsFormBindingSource;
        private System.Windows.Forms.BindingSource optionsFormBindingSource1;


        

    }
}