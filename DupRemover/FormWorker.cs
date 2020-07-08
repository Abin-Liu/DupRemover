using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MFGLib;

namespace DupRemover
{
	public partial class FormWorker : MessageThreadForm
	{
		public int TotalFiles { get; private set; }
		public int DupFiles { get; private set; }

		DupRemoverThread m_thread = new DupRemoverThread();
		string m_folder;

		public FormWorker(string folder)
		{
			InitializeComponent();
			m_folder = folder;
		}

		private void FormWorker_Load(object sender, EventArgs e)
		{
			SetThread(m_thread);
			TotalFiles = m_thread.SetWorkingFolder(m_folder);

			progressBar1.Minimum = 0;
			progressBar1.Maximum = TotalFiles;
			progressBar1.Value = 0;

			m_thread.Start();
		}

		protected override void OnThreadMessage(int wParam, int lParam)
		{
			base.OnThreadMessage(wParam, lParam);
			if (wParam == 1)
			{
				progressBar1.Value++;
			}
		}

		protected override void OnThreadStop()
		{
			base.OnThreadStop();
			DupFiles = m_thread.DupedFiles;
			Close();
		}
	}
}
