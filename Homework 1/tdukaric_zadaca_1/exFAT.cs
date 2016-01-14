using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tdukaric_zadaca_1
{
    class exFAT : FS, IFS
    {
        private static exFAT instance;

        public static exFAT GetInstance(string path, string DS_Type)
        {
            if (instance == null)
            {
                instance = new exFAT(path, DS_Type);
            }
            return instance;
        }


        private exFAT(string path, string DS_Type)
        {
            this.DS_Type = DS_Type;
            this.id = 0;
            this.main = new dir { path = path, id = 0, name = path, root = null, folder = true, link = false };
            loadComposites(path, main);
        }

        private void loadLeafs(string path, IComponent root)
        {

            string[] folders = Directory.GetFiles(path);

            foreach (string file in folders)
            {
                this.id++;
                FileInfo temp = new FileInfo(file);
                file dat;
                if (System.IO.File.GetAttributes(file).HasFlag(FileAttributes.ReparsePoint))
                {
                    continue;
                }
                else
                {
                    dat = new file { path = file, name = temp.Name, size = temp.Length, id = this.id, folder = false, root = root, permitWriting = !temp.IsReadOnly };
                }
                root.AddComponent(dat);
            }
        }

        private void loadComposites(string path, IComponent root)
        {
            try
            {
                string[] files = Directory.GetFiles(path, ".");
            }
            catch
            {
                Console.WriteLine("Directory doesn't exists!");
                return;
            }

            try
            {
                string[] folders = Directory.GetDirectories(path);
                foreach (string folderPath in folders)
                {
                    this.id++;
                    dir folder = null;

                    if (System.IO.File.GetAttributes(folderPath).HasFlag(FileAttributes.ReparsePoint))
                        continue;
                    else
                        folder = new dir { path = folderPath, id = this.id, name = new DirectoryInfo(folderPath).Name, folder = true, root = root, link = false };

                    root.AddComponent(folder);
                    loadComposites(folderPath, folder);
                }
                loadLeafs(path, root);
                root.CalculateSize();
            }
            catch
            {
                Console.WriteLine("Error");
            }
        }

        private void copyFolder(string what, string where, IComponent root)
        {

            DirectoryInfo dir = new DirectoryInfo(what);
            DirectoryInfo[] dirs = dir.GetDirectories();

            DirectoryInfo _dir = Directory.CreateDirectory(where);

            this.id++;
            dir folder = null;
            if (DS_Type == "NTFS")
            {
                folder = new dir { path = _dir.FullName, id = this.id, name = _dir.Name, folder = true, root = root, link = System.IO.File.GetAttributes(_dir.FullName).HasFlag(FileAttributes.ReparsePoint) };
            }
            else if (DS_Type == "exFAT")
            {
                if (!System.IO.File.GetAttributes(_dir.FullName).HasFlag(FileAttributes.ReparsePoint))
                    folder = new dir { path = _dir.FullName, id = this.id, name = new DirectoryInfo(_dir.FullName).Name, folder = true, root = root, link = false };
            }

            root.AddComponent(folder);

            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(where, subdir.Name);
                copyFolder(subdir.FullName, temppath, folder);
            }
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                this.id++;
                string temppath = Path.Combine(where, file.Name);
                FileInfo temp = file.CopyTo(temppath, false);

                file dat = new file { path = temp.FullName, name = temp.Name, size = temp.Length, id = this.id, folder = false, root = folder, permitWriting = !temp.IsReadOnly, link = System.IO.File.GetAttributes(temp.FullName).HasFlag(FileAttributes.ReparsePoint) };
                folder.AddComponent(dat);

            }
        }

    }
}
