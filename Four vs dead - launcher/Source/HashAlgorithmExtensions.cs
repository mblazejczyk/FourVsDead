using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Linq;

// https://stackoverflow.com/questions/3625658/creating-hash-for-folder

namespace RedlabsUpdateUtility
{
	public static class HashAlgorithmExtensions
	{

		/// <summary>
		/// Generates an MD5 hash for a specified file/folder structure. Includes all subfolders and files.
		/// If you want a different or more secure hash, replace the contents of the function with your
		/// hash algorithm. MD5 should be secure enough for most purposes.
		/// </summary>
		/// <param name="path"></param>
		/// <returns>A string hash for the files</returns>
		public static string CreateMd5ForFolder(string path)
		{
			// assuming you want to include nested folders
			var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories)
								 .OrderBy(p => p).ToList();

			MD5 md5 = MD5.Create();

			for(int i = 0; i < files.Count; i++)
			{
				string file = files[i];

				// hash path
				string relativePath = file.Substring(path.Length + 1);
				byte[] pathBytes = Encoding.UTF8.GetBytes(relativePath.ToLower());
				md5.TransformBlock(pathBytes, 0, pathBytes.Length, pathBytes, 0);

				// hash contents
				byte[] contentBytes = File.ReadAllBytes(file);
				if(i == files.Count - 1)
					md5.TransformFinalBlock(contentBytes, 0, contentBytes.Length);
				else
					md5.TransformBlock(contentBytes, 0, contentBytes.Length, contentBytes, 0);
			}

			return BitConverter.ToString(md5.Hash).Replace("-", "").ToLower();
		}
	}
}