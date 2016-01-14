using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tdukaric_zadaca_1
{
    class NTFS : FS, IFS
    {
        private static NTFS instance;

        public static NTFS GetInstance(string path, string DS_Type)
        {
            if (instance == null)
            {
                instance = new NTFS(path, DS_Type);
            }
            return instance;
        }


        private NTFS(string path, string DS_Type)
        {
            this.DS_Type = DS_Type;
            this.id = 0;
            this.main = new dir { path = path, id = 0, name = path, root = null, folder = true, link = false };
            loadComposites(path, main);
        }

        private void loadLeafs(string path, IComponent root)
        {

            string[] files = Directory.GetFiles(path);

            foreach (string file in files)
            {
                this.id++;
                FileInfo fileInfo = new FileInfo(file);

                file dat = new file { path = file, name = fileInfo.Name, size = fileInfo.Length, id = this.id, folder = false, root = root, permitWriting = !fileInfo.IsReadOnly, link = System.IO.File.GetAttributes(file).HasFlag(FileAttributes.ReparsePoint) };
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
                Console.WriteLine("Folder doesn't exist!");
                return;
            }

            try
            {
                string[] folders = Directory.GetDirectories(path);
                foreach (string folderPath in folders)
                {
                    this.id++;
                    dir folder = null;

                    folder = new dir { path = folderPath, id = this.id, name = new DirectoryInfo(folderPath).Name, folder = true, root = root, link = System.IO.File.GetAttributes(folderPath).HasFlag(FileAttributes.ReparsePoint) };

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
            dir directory = null;

            directory = new dir { path = _dir.FullName, id = this.id, name = _dir.Name, folder = true, root = root, link = System.IO.File.GetAttributes(_dir.FullName).HasFlag(FileAttributes.ReparsePoint) };

            root.AddComponent(directory);

            foreach (DirectoryInfo subdir in dirs)
            {
                string tempPath = Path.Combine(where, subdir.Name);
                copyFolder(subdir.FullName, tempPath, directory);
            }
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                this.id++;
                string temppath = Path.Combine(where, file.Name);
                FileInfo temp = file.CopyTo(temppath, false);

                file dat = new file { path = temp.FullName, name = temp.Name, size = temp.Length, id = this.id, folder = false, root = directory, permitWriting = !temp.IsReadOnly, link = System.IO.File.GetAttributes(temp.FullName).HasFlag(FileAttributes.ReparsePoint) };
                directory.AddComponent(dat);

            }
        }

    }
}
