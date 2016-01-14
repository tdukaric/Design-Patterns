using System;
namespace tdukaric_zadaca_1
{
    public interface IFS
    {
        int id { get; set; }
        string DS_Type { get; set; }
        IComponent main { get; set; }

        void PrintFS(IComponent what);
        bool CopyComponent(int what, int where);
        bool CopyComponent(int what, int where, string name);
        bool CreateComponent(int where, string name, bool isDirectory);
        bool CreateComponentOnFS(int where, string name, bool isDirectory);
        void RemoveComponent(int what);
        void Open(int what);
        void MoveComponent(int what, int where);
        string ShowReverse(int what);
    }

}
