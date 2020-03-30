using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Comparer.Core
{
    public class Core
    {
        DirectoryInfo source;
        DirectoryInfo destination;


        List<FileInfo> sourceSet = new List<FileInfo>();
        List<FileInfo> destinationSet = new List<FileInfo>();
        FileInfo[] results;

        public bool ShouldStop = false;
        private DateTime lapStart;
        DateTime start;

        int counter = 0;

        public Dictionary<string, string> Similarities = new Dictionary<string, string>();
        private readonly Action<string, int> onStatusUpdate;

        public Core(Action<string, int> onStatusUpdate, string srcPath, string dstPath)
        {
            this.onStatusUpdate = onStatusUpdate;
            Console.CancelKeyPress += (sender, args) => ShouldStop = true;
            source = new DirectoryInfo(srcPath);
            destination = new DirectoryInfo(dstPath);
        }



        public void Execute()
        {
            this.lapStart = start = DateTime.UtcNow;
            ListDir(source, ref sourceSet, "listing");
            onStatusUpdate("listing", counter);
            counter = 0;
            ListDir(destination, ref destinationSet, "comparing");
            onStatusUpdate("comparing", counter);
            Compute();

            Console.WriteLine("Took: " + (DateTime.UtcNow - start));
        }

        void Compute()
        {
            var comparer = new SizeAndNameComparer(Similarities, onStatusUpdate);
            results = sourceSet.Except(destinationSet, comparer).ToArray();
            onStatusUpdate("computing", comparer.counter);
        }


        public FileInfo[] GetResults()
        {
            return results;
        }


        void Show()
        {
            foreach (FileInfo f in results)
                Console.WriteLine(f.FullName);
        }

        void LogStatus(DirectoryInfo folder, string action)
        {
            var now = DateTime.UtcNow;
            if (lapStart < now)
            {
                lapStart = now.AddSeconds(2);
                Console.WriteLine(action + " " + folder.FullName);
            }
        }


        void ListDir(DirectoryInfo folder, ref List<FileInfo> list, string action)
        {
            if (ShouldStop)
                return;
            LogStatus(folder, action);
            try
            {
                DirectoryInfo[] dirs = folder.GetDirectories();

                foreach (DirectoryInfo d in dirs)
                {
                    ListDir(d, ref list, action);
                }
                FileInfo[] files = folder.GetFiles();
                counter += files.Length;
                if (counter % 50 == 0)
                    onStatusUpdate(action, counter);
                list.AddRange(files);
            }
            catch (System.UnauthorizedAccessException)
            {
            }
        }

    }



}
