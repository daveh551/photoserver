using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoServer.DataAccessLayer.Storage
{
	public interface IStorageProvider
	{
		bool FileExists(string path);
		Stream GetStream(string path);
		void WriteFile(string path, byte[] imageArray);
		void DeleteFile(string path);
		IEnumerable<string> GetFiles(string directoryPath);
	}
}
