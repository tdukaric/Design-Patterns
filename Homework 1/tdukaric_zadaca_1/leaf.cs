using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tdukaric_zadaca_1
{
    /// <summary>
    /// Leaf
    /// </summary>
    class file : IComponent
    {
        public bool folder { get; set; }
        public string path { get; set; }
        public string name { get; set; }
        public long size { get; set; }
        public int id { get; set; }
        public IComponent root { get; set; }
        public bool permitWriting { get; set; }
        public bool link { get; set; }
        

        public void AddComponent(IComponent c)
        {
            Console.WriteLine("Can't add a leaf to the leaf.");
        }

        public void RemoveComponent(int i)
        {
            root.RemoveComponent(i);
        }

        public void RemoveComponent(IComponent i)
        {
            root.RemoveComponent(i);
        }

        public void CalculateSize()
        {

        }

        public IComponent FindComponent(int i)
        {
            if (this.id == i)
                return this;
            else return null;
        }

        public IComponent FindComponent(string name)
        {
            if (this.name.ToLower() == name.ToLower())
                return this;
            else return null;
        }

        public IComponent FindComponentInFolder(string name)
        {
            Console.WriteLine("Not a directory!");
            return null;
        }

        public string Show(int depth)
        {
            return String.Format("[{0,3}][F] {1, -30}[R{2}] size: {3}\n", this.id, new String(' ', depth) + this.name, (this.permitWriting ? "W" : " "), this.size);
        }
    }
}
