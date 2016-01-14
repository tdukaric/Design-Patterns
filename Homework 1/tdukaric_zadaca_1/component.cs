using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tdukaric_zadaca_1
{
    /// <summary>
    /// Component
    /// </summary>
    public interface IComponent
    {
        void AddComponent(IComponent com);
        void RemoveComponent(int i);
        void RemoveComponent(IComponent i);
        IComponent FindComponent(int i);
        IComponent FindComponent(string Ime);
        IComponent FindComponentInFolder(string Ime);
        string Show(int i);
        void CalculateSize();

        int id { get; set; }
        string path { get; set; }
        string name { get; set; }
        bool folder { get; set; }
        IComponent root { get; set; }
        long size { get; set; }
        bool permitWriting { get; set; }
        bool link { get; set; }
    }
}
