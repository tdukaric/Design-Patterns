using System;
using System.IO;

namespace tdukaric_zadaca_3
{
    public static class DnevnikRada
    {
        private static string path = "dnevnik.txt";


        public static void add(string what)
        {
            StreamWriter write = new StreamWriter(path, true);
            write.WriteLine(what);
            write.Close();
        }

    }
}
