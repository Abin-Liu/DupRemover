using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;
using MFGLib;

namespace DupRemover
{
	class DupRemoverThread : MessageThread
	{
		public int DupedFiles { get; private set; }
		string m_folder;

		public int SetWorkingFolder(string folder)
		{
			m_folder = folder;
			DirectoryInfo di = new DirectoryInfo(folder);
			return di.GetFiles().Length;
		}

		protected override void ThreadProc()
		{
			string backupFolder = m_folder + "\\_Duplicated";
			bool backupFolderExists = Directory.Exists(backupFolder);
			Dictionary<string, bool> map = new Dictionary<string, bool>();

			DirectoryInfo di = new DirectoryInfo(m_folder);
			FileInfo[] files = di.GetFiles();

			int dupFiles = 0;
			HashAlgorithm hash = HashAlgorithm.Create();
			foreach (FileInfo fi in files)
			{
				string key;
				using (FileStream fs = new FileStream(fi.FullName, FileMode.Open))
				{
					byte[] bytes = hash.ComputeHash(fs);
					key = BitConverter.ToString(bytes);
				}

				if (map.ContainsKey(key))
				{
					if (!backupFolderExists)
					{
						Directory.CreateDirectory(backupFolder);
						backupFolderExists = true;
					}

					File.Move(fi.FullName, backupFolder + '\\' + fi.Name);
					dupFiles++;
				}
				else
				{
					map.Add(key, true);
				}

				PostMessage(1);
			}

			DupedFiles = dupFiles;
		}
	}
}
