﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Application.Utils
{
    public class PathUtils
    {
        /// <summary>
        /// Makes a filename safe for use within a URL
        /// </summary>
        public static string MakeFileNameSafeForUrls(string fileName)
        {
            var extension = Path.GetExtension(fileName);
            var safeFileName = Path.GetFileNameWithoutExtension(fileName).ToSlug();
            return Path.Combine(Path.GetDirectoryName(fileName), safeFileName + extension);
        }

        /// <summary>
        /// Combines two URL paths
        /// </summary>
        public static string CombinePaths(string path1, string path2)
        {

            if (path2.IsNullOrEmpty())
                return path1;

            if (path1.IsNullOrEmpty())
                return path2;

            if (path2.StartsWith("http://") || path2.StartsWith("https://"))
                return path2;

            var ch = path1[path1.Length - 1];

            if (ch != '/')
                return (path1.TrimEnd('/') + '/' + path2.TrimStart('/'));

            return (path1 + path2);
        }
    }
}