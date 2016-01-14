using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tdukaric_zadaca_1
{
    abstract class FS : IFS
    {
        public IComponent main { get; set; }
        public string DS_Type { get; set; }
        public int id { get; set; }

        public void MoveComponent(int what, int where)
        {
            if (CopyComponent(what, where))
            {
                RemoveComponent(what);
            }
            else
                Console.WriteLine("Error, can't move.");
        }

        public bool CopyComponent(int what, int where)
        {
            IComponent _what = main.FindComponent(what);
            IComponent _where = main.FindComponent(where);

            if (_what == null || _where == null)
            {
                Console.WriteLine("Can't find an object!");
                return false;
            }
            if (_where.folder)
                if (_where.FindComponentInFolder(_what.name) == null)
                {

                    IComponent temp;
                    this.id++;
                    if (_what.folder)
                    {
                        DirectoryInfo dir = Directory.CreateDirectory(_where.path + '\\' + _what.name);
                        CopyDirectory(_what.path, dir.FullName, _where);

                    }
                    else
                    {
                        string destination = System.IO.Path.Combine(_where.path, _what.name);
                        System.IO.File.Copy(_what.path, destination, true);

                        FileInfo fileInfo = new FileInfo(destination);

                        temp = new file { id = this.id, name = _what.name, root = _where, path = destination, size = _what.size, permitWriting = !fileInfo.IsReadOnly };
                        _where.AddComponent(temp);
                    }

                    _where.CalculateSize();

                    return true;
                }
                else
                    Console.WriteLine("Error, check the name.");
            else
                Console.WriteLine("Error, can't copy the object.");
            return false;
        }

        private void CopyDirectory(string what, string where, IComponent root)
        {

            DirectoryInfo dir = new DirectoryInfo(what);
            DirectoryInfo[] dirs = dir.GetDirectories();

            DirectoryInfo _dir = Directory.CreateDirectory(where);

            this.id++;
            dir directory = null;
            if (DS_Type == "NTFS")
            {
                directory = new dir { path = _dir.FullName, id = this.id, name = _dir.Name, folder = true, root = root, link = System.IO.File.GetAttributes(_dir.FullName).HasFlag(FileAttributes.ReparsePoint) };
            }
            else if (DS_Type == "exFAT")
            {
                if (!System.IO.File.GetAttributes(_dir.FullName).HasFlag(FileAttributes.ReparsePoint))
                    directory = new dir { path = _dir.FullName, id = this.id, name = new DirectoryInfo(_dir.FullName).Name, folder = true, root = root, link = false };
            }

            root.AddComponent(directory);

            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(where, subdir.Name);
                CopyDirectory(subdir.FullName, temppath, directory);
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

        public bool CopyComponent(int what, int where, string name)
        {
            IComponent _what = main.FindComponent(what);
            string temp = _what.name;
            _what.name = name;
            if (CopyComponent(what, where))
                _what.name = temp;
            else
            {
                _what.name = name;
                return false;
            }
            return true;
        }

        public void RemoveComponent(int what)
        {
            IComponent _what = main.FindComponent(what);
            if (_what == null)
            {
                Console.WriteLine("Can't find object.");
                return;
            }
            if (_what.folder)
            {
                System.IO.DirectoryInfo directoryInfo = new System.IO.DirectoryInfo(_what.path);
                try
                {
                    directoryInfo.Delete(true);
                }
                catch
                {
                    Console.WriteLine("Can't delete the component.");
                    return;
                }

            }
            else
            {
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(_what.path);
                try
                {
                    fileInfo.Delete();
                }
                catch
                {
                    Console.WriteLine("Can't delete the component.");
                    return;
                }
            }
            _what.root.RemoveComponent(_what);
        }

        public void PrintFS(IComponent root)
        {
            Console.WriteLine(root.Show(0));
        }

        public bool CreateComponent(int where, string name, bool isDirectory)
        {
            IComponent _where = main.FindComponent(where);
            if (!_where.folder)
            {
                Console.WriteLine("Destination has to be a directory!");
                return false;
            }
            else
            {
                this.id++;
                IComponent root = null;
                if (isDirectory)
                {
                    root = new dir { path = _where.path + '\\' + name, id = this.id, name = name, folder = true, root = _where, link = false };
                }
                else
                {
                    root = new file { path = _where.path + '\\' + name, id = this.id, name = name, folder = false, root = _where, link = false, permitWriting = true };
                }
                if (root == null)
                    return false;
                else
                {
                    _where.AddComponent(root);
                    _where.CalculateSize();
                }
            }
            return true;
        }

        public bool CreateComponentOnFS(int where, string name, bool isDirectory)
        {
            IComponent _temp = main.FindComponent(where);
            if ((_temp != null) && (main.FindComponent(where).FindComponentInFolder(name.ToLower()) == null))
            {
                string path = _temp.path + '\\' + name;
                if (isDirectory)
                {
                    Directory.CreateDirectory(path);
                    return CreateComponent(where, name, isDirectory);
                }
                else
                {
                    File.Create(path);
                    return CreateComponent(where, name, isDirectory);
                }
            }
            else
            {
                Console.WriteLine("Can't delete the component!");
                return false;
            }
        }

        public String ShowReverse(int what)
        {
            StringBuilder result = new StringBuilder();
            IComponent _what = main.FindComponent(what);
            while (_what != null)
            {
                result.Append(_what.name);
                result.Append('\\');
                _what = _what.root;
            }
            result = result.Remove(result.Length - 1, 1);
            return result.ToString();
        }

        public void Open(int what)
        {
            IComponent _what = main.FindComponent(what);
            if (_what != null)
                System.Diagnostics.Process.Start(_what.path);
        }
    }
}
