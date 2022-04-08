using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SingleResPrincipal
{
    public class Journal
    {
        private readonly List<string> entries = new List<string>();

        private static int count = 0;

        public int AddEntry(string text)
        {
            entries.Add($"{++count}:{text}");
            return count;
        }

        public void RemoveEntry(int index)
        {
            entries.RemoveAt(index);
        }

        public override string ToString()
        {
            return string.Join(Environment.NewLine, entries);
        }
    }

    public class Persistence
    {
        public void SaveFile(Journal j, string fileName, bool overwrite = false)
        {
            if (overwrite || !File.Exists(fileName))
            {
                File.WriteAllText(fileName, j.ToString());
            }
        }
    }
   public  class Program
    {
        static void Main(string[] args)
        {
            var j = new Journal();
            j.AddEntry("I Cried Today");
            j.AddEntry("I ate a bug");

            var p = new Persistence();
            var fileName = @"C:\temp\Journal.txt";
            p.SaveFile(j, fileName, true);
            Process.Start(fileName);
           
        }
    }
}
