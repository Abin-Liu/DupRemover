using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DupRemover
{
	public partial class FormMain : Form
	{
		public FormMain()
		{
			InitializeComponent();
		}

		private void FormMain_Load(object sender, EventArgs e)
		{			
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			string folder = txtFolder.Text.Trim();
			if (string.IsNullOrEmpty(folder))
			{
				MessageBox.Show(this, "Directory is required.", ProductName);
				txtFolder.Focus();
				txtFolder.SelectAll();
				return;
			}

			if (!Directory.Exists(folder))
			{
				MessageBox.Show(this, "Directory is not valid.", ProductName);
				txtFolder.Focus();
				txtFolder.SelectAll();
				return;
			}

			FormWorker form = new FormWorker(folder);
			form.ShowDialog(this);

			string text = string.Format("Total files: {0}\nDuplicated files: {1}", form.TotalFiles, form.DupFiles);
			MessageBox.Show(this, text, ProductName);
		}

		private void btnExit_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
