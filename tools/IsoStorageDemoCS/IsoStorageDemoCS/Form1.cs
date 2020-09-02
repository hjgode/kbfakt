using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace IsoStorageDemoCS
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		internal System.Windows.Forms.TextBox txtDemo;
		internal System.Windows.Forms.ComboBox cmbDemo;
		internal System.Windows.Forms.CheckBox chkDemo;
		internal System.Windows.Forms.Label Label1;

		private IsolatedStorage.ConfigurationManager config;

		public Form1()
		{
			InitializeComponent();
			config = IsolatedStorage.ConfigurationManager.GetConfigurationManager(Application.ProductName);
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing)
		{
			if( disposing)
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.txtDemo = new System.Windows.Forms.TextBox();
			this.cmbDemo = new System.Windows.Forms.ComboBox();
			this.chkDemo = new System.Windows.Forms.CheckBox();
			this.Label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// txtDemo
			// 
			this.txtDemo.Location = new System.Drawing.Point(72, 12);
			this.txtDemo.Name = "txtDemo";
			this.txtDemo.Size = new System.Drawing.Size(112, 20);
			this.txtDemo.TabIndex = 7;
			this.txtDemo.Text = "";
			// 
			// cmbDemo
			// 
			this.cmbDemo.Items.AddRange(new object[] {
														 "One",
														 "Two",
														 "Three",
														 "Four",
														 "Rock & Roll!"});
			this.cmbDemo.Location = new System.Drawing.Point(12, 80);
			this.cmbDemo.Name = "cmbDemo";
			this.cmbDemo.Size = new System.Drawing.Size(176, 21);
			this.cmbDemo.TabIndex = 10;
			// 
			// chkDemo
			// 
			this.chkDemo.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.chkDemo.Location = new System.Drawing.Point(12, 48);
			this.chkDemo.Name = "chkDemo";
			this.chkDemo.Size = new System.Drawing.Size(172, 24);
			this.chkDemo.TabIndex = 9;
			this.chkDemo.Text = "Yes/No";
			// 
			// Label1
			// 
			this.Label1.Location = new System.Drawing.Point(8, 12);
			this.Label1.Name = "Label1";
			this.Label1.Size = new System.Drawing.Size(60, 23);
			this.Label1.TabIndex = 8;
			this.Label1.Text = "Some text:";
			this.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 266);
			this.Controls.Add(this.txtDemo);
			this.Controls.Add(this.cmbDemo);
			this.Controls.Add(this.chkDemo);
			this.Controls.Add(this.Label1);
			this.Name = "Form1";
			this.Text = "IsoStorageDemoCS";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			config.ReadFormSettings(this);
			txtDemo.Text = config.Read("DemoText");
			chkDemo.Checked = config.ReadBoolean("DemoChecked", false);
			cmbDemo.SelectedIndex = config.ReadInteger("DemoCombo", 0);
		}

		private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			config.Write("DemoText", txtDemo.Text);
			config.Write("DemoChecked", chkDemo.Checked.ToString());
			config.Write("DemoCombo", cmbDemo.SelectedIndex.ToString());
			config.WriteFormSettings(this);
			config.Persist();
		}
	}
}
