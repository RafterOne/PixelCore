using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace PixelMEDIA.PixelCore.Helpers
{
    /// <summary>
    /// System utilities.
    /// </summary>
	public static class SystemHelper
	{
		/// <summary>
		/// Executes a command and returns the standard output.
		/// </summary>
		/// <param name="executable"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public static string ExecuteCommand(string executable, params string[] args)
		{
			Process p = new Process();
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.RedirectStandardOutput = true;
			p.StartInfo.FileName = executable;
			p.StartInfo.Arguments = String.Join(" ", args);
			p.Start();
			var output = p.StandardOutput.ReadToEnd();
			p.WaitForExit();
			return output;
		}

		/// <summary>
		/// Gets the full path for another file with the same name but different extension as the original file.
		/// </summary>
		/// <param name="originalFilePath"></param>
		/// <param name="buddyExtension"></param>
		/// <returns></returns>
		public static string GetDifferentlyExtensionedBuddyFilePath(string originalFilePath, string buddyExtension)
		{
			var targetDirectory = Path.GetDirectoryName(originalFilePath);
			return GetDifferentlyExtensionedBuddyFilePath(originalFilePath, buddyExtension, targetDirectory);
		}

		/// <summary>
		/// Gets the full path for another file with the same name but different extension and directory as the original file.
		/// </summary>
		/// <param name="originalFilePath"></param>
		/// <param name="buddyExtension"></param>
		/// <param name="targetDirectory"></param>
		/// <returns></returns>
		public static string GetDifferentlyExtensionedBuddyFilePath(string originalFilePath, string buddyExtension, string targetDirectory)
		{
			var fileStub = Path.GetFileNameWithoutExtension(originalFilePath);
			return Path.Combine(targetDirectory, fileStub + "." + buddyExtension);
		}

        /// <summary>
        /// Get the file name minus extension from the given path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
		public static string GetFileNameStub(string path)
		{
			return Path.GetFileNameWithoutExtension(path);
		}

		/// <summary>
		/// Returns a full path of a s
		/// </summary>
		/// <param name="originalFilePath"></param>
		/// <param name="neighborFileStub"></param>
		/// <returns></returns>
		public static string GetSimilarlyExtensionedNeighborFilePath(string originalFilePath, string neighborFileStub)
		{
			var targetDirectory = Path.GetDirectoryName(originalFilePath);
			var extension = Path.GetExtension(originalFilePath);
			return Path.Combine(targetDirectory, neighborFileStub + "." + extension);
		}

		/// <summary>
		/// Returns the contents of a text file.
		/// </summary>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static string GetTextFromFile(string filePath)
		{
			using (var reader = new StreamReader(filePath))
			{
				return reader.ReadToEnd();
			}
		}

		/// <summary>
		/// Gets a list of the full paths of all files in a directory.
		/// </summary>
		/// <param name="directoryPath"></param>
		/// <returns></returns>
		public static IEnumerable<string> GetFilePathsFromDirectory(string directoryPath)
		{
			DirectoryInfo di = new DirectoryInfo(directoryPath);
			var files = di.GetFiles();
			return from file in files select file.FullName;
		}

		/// <summary>
		/// Gets a list of the file names of all files in a directory.
		/// </summary>
		/// <param name="directoryPath"></param>
		/// <returns></returns>
		public static IEnumerable<string> GetFileNamesFromDirectory(string directoryPath)
		{
			DirectoryInfo di = new DirectoryInfo(directoryPath);
			var files = di.GetFiles();
			return from file in files select file.Name;
		}

		/// <summary>
		/// Moves a file from one path to another.
		/// </summary>
		/// <param name="fromPath"></param>
		/// <param name="toPath"></param>
		/// <returns></returns>
		public static Status MoveFile(string fromPath, string toPath)
		{
			FileInfo fi = new FileInfo(fromPath);
			try
			{
				fi.MoveTo(toPath);
				return Status.Succeed();
			}
			catch (Exception ex)
			{
				return Status.Fail(ex);
			}
		}

		/// <summary>
		/// Moves/Replaces a file from one path to another.
		/// </summary>
		/// <param name="fromPath"></param>
		/// <param name="toPath"></param>
		/// <returns></returns>
		public static Status ReplaceFile(string fromPath, string toPath)
		{
			FileInfo fi = new FileInfo(fromPath);
			try
			{
				fi.CopyTo(toPath, true);
				fi.Delete();
				return Status.Succeed();
			}
			catch (Exception ex)
			{
				return Status.Fail(ex);
			}
		}

		/// <summary>
		/// Moves a file from one directory to another.
		/// </summary>
		/// <param name="fileName"></param>
		/// <param name="fromDirectory"></param>
		/// <param name="toDirectory"></param>
		/// <returns></returns>
		public static Status MoveFileToDirectory(string fileName, string fromDirectory, string toDirectory)
		{
			FileInfo fi = new FileInfo(Path.Combine(fromDirectory, fileName));
			try
			{
				var destination = Path.Combine(toDirectory, fileName);
				fi.CopyTo(destination, true);
				fi.Delete();
				return Status.Succeed(destination);
			}
			catch (Exception ex)
			{
				return Status.Fail(ex);
			}
		}

		/// <summary>
		/// Saves a byte array to disk at the specified path.
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public static Status SaveFile(string filePath, byte[] bytes)
		{
			try
			{
				File.WriteAllBytes(filePath, bytes);
				return Status.Succeed(filePath);
			}
			catch (Exception ex)
			{
				return Status.Fail(ex);
			}
		}

	}
}
