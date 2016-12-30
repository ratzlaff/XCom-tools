using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace DSShared.FileSystems
{
	/// <summary>
	/// </summary>
	public interface IFileBackupManager
	{
		/// <summary>
		/// Backups a file
		/// </summary>
		void Backup(string filePath);
	}

	/// <summary>
	/// </summary>
	public class FileBackupManager : IFileBackupManager
	{
		/// <summary>
		/// Backups a file
		/// </summary>
		public void Backup(string filePath)
		{
			var dir = Path.GetDirectoryName(filePath);
			dir = Path.Combine(dir, "Backups");
			if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
			var name = Path.GetFileName(filePath);

			var finalPath = Path.Combine(dir, name + Path.GetRandomFileName() );
			while (File.Exists(finalPath))
			{
				  finalPath = Path.Combine(dir, name + Path.GetRandomFileName());
			}
			File.Copy(filePath, finalPath);
		}
	}
}
