using System;
using System.Collections.Generic;
using System.IO;

namespace Comparer.Core
{
    public class SizeAndNameComparer : IEqualityComparer<FileInfo>
    {
        public int counter = 0;
        private int minicounter = 0;

        public Dictionary<string, string> Similarities;
        private readonly Action<string, int> onStatusUpdate;

        public SizeAndNameComparer(Dictionary<string, string> similarities, Action<string, int> onStatusUpdate)
        {
            this.Similarities = similarities;
            this.onStatusUpdate = onStatusUpdate;
        }


        public bool Equals(FileInfo x, FileInfo y)
        {
            counter++;
            minicounter++;
            if (minicounter == 100000)
            {
                minicounter = 0;
                onStatusUpdate("computing", counter);
            }
            if (x.Name == y.Name)
            {
                if (x.Length == y.Length)
                    return true;
                try
                {
                    Similarities.Add(x.FullName, y.FullName);
                }
                catch (Exception) { }
                try
                {
                    Similarities.Add(y.FullName, x.FullName);
                }
                catch (Exception) { }
            }
            return false;

        }

        public int GetHashCode(FileInfo obj)
        {
            return 0;
        }
    }
}
