﻿using System;
using System.IO;

namespace Frapid.Framework
{
    public static class FileHelper
    {
        /// <summary>
        /// Depth-first recursive delete, with handling for descendant 
        /// directories open in Windows Explorer.
        /// </summary>
        public static void DeleteDirectoryRecursively(string path)
        {
            foreach (string directory in Directory.GetDirectories(path))
            {
                DeleteDirectoryRecursively(directory);
            }

            try
            {
                Directory.Delete(path, true);
            }
            catch (IOException)
            {
                Directory.Delete(path, true);
            }
            catch (UnauthorizedAccessException)
            {
                Directory.Delete(path, true);
            }
        }

        public static void CopyFilesRecursively(string source, string target, bool createTarget)
        {
            if (!Directory.Exists(source)) return;

            if (!Directory.Exists(target))
            {
                if (createTarget)
                {
                    Directory.CreateDirectory(target);
                }
                else
                {
                    return;
                }
            }

            CopyFilesRecursively(new DirectoryInfo(source), new DirectoryInfo(target));
        }

        public static void CopyFilesRecursively(DirectoryInfo source, DirectoryInfo target, bool overwrite = true)
        {
            foreach (var dir in source.GetDirectories())
                CopyFilesRecursively(dir, target.CreateSubdirectory(dir.Name), overwrite);
            foreach (var file in source.GetFiles())
                file.CopyTo(Path.Combine(target.FullName, file.Name), overwrite);
        }

        public static void CopyDirectory(string source, string target)
        {
            var origin = new DirectoryInfo(source);
            var destination = new DirectoryInfo(target);

            CopyFilesRecursively(origin, destination);
        }
    }
}