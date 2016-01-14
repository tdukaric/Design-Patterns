using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tdukaric_zadaca_1
{
    /// <summary>
    /// Composite
    /// </summary>
    class dir : IComponent
    {
        public bool folder { get; set; }
        public int id { get; set; }
        public string path { get; set; }
        public string name { get; set; }
        private List<IComponent> childrens = new List<IComponent>();
        public IComponent root { get; set; }
        public long size { get; set; }
        public bool permitWriting { get; set; }
        public bool link { get; set; }

        public void AddComponent(IComponent Komponenta)
        {
            childrens.Add(Komponenta);
            CalculateSize();
        }

        public void RemoveComponent(IComponent i)
        {
            childrens.Remove(i);
            CalculateSize();
        }

        public void RemoveComponent(int i)
        {
            IComponent Component = FindComponent(i);
            if (Component != null)
                childrens.Remove(Component);
            else
                Console.WriteLine("Can't delete component");
        }

        public IComponent getComponent(int i)
        {
            return childrens[i];
        }

        public IComponent FindComponent(int i)
        {
            if (id == i)
                return this;
            IComponent x = null;
            foreach (IComponent c in childrens)
            {
                x = c.FindComponent(i);
                if (x != null)
                    return x;
            }
            return x;
        }

        public IComponent FindComponent(string name)
        {
            if (this.name.ToLower() == name.ToLower())
                return this;
            IComponent x = null;
            foreach (IComponent c in childrens)
            {
                x = c.FindComponent(name);
                if (x != null)
                    return x;
            }
            return x;
        }

        public IComponent FindComponentInFolder(string name)
        {
            if (this.name.ToLower() == name.ToLower())
                return this;
            foreach (IComponent c in childrens)
            {
                if (c.name.ToLower() == name.ToLower())
                    return this;
            }
            return null;
        }

        public string Show(int depth)
        {
            CalculateSize();
            StringBuilder result = new StringBuilder();
            if (this.root == null)
                result.Append(String.Format("[{0,3}][{1}] {2, -30}     velicina: {3}\n", this.id, (this.link ? "L": "D"), new String(' ', depth) + this.path, this.size));
            else
                result.Append(String.Format("[{0,3}][{1}] {2, -30}     velicina: {3}\n", this.id, (this.link ? "L" : "D"), new String(' ', depth) + this.name, this.size));

            foreach (IComponent component in childrens)
            {
                result.Append(component.Show(depth + 2));
            }
            return result.ToString();
        }

        public void CalculateSize()
        {
            this.size = 0;
            foreach (IComponent child in this.childrens)
            {
                if (!child.link)
                {
                    child.CalculateSize();
                    size += child.size;
                }
            }
        }

    }
}
