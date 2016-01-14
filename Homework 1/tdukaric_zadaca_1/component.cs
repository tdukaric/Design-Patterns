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
        void dodajKomponentu(IComponent com);
        void makniKomponentu(int i);
        void makniKomponentu(IComponent i);
        IComponent pronadiKomponentu(int i);
        IComponent pronadiKomponentu(string Ime);
        IComponent pronadiKomponentuUDirektoriju(string Ime);
        string prikazi(int i);
        void izracunajVelicinu();

        int Oznaka { get; set; }
        string Putanja { get; set; }
        string Naziv { get; set; }
        bool Direktorij { get; set; }
        IComponent Korijen { get; set; }
        long Velicina { get; set; }
        bool DozvoliPisanje { get; set; }
        bool Poveznica { get; set; }
    }
}
