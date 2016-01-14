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
        public bool Direktorij { get; set; }
        public string Putanja { get; set; }
        public string Naziv { get; set; }
        public long Velicina { get; set; }
        public int Oznaka { get; set; }
        public IComponent Korijen { get; set; }
        public bool DozvoliPisanje { get; set; }
        public bool Poveznica { get; set; }
        

        public void dodajKomponentu(IComponent c)
        {
            Console.WriteLine("Datoteka se nemože dodati na datoteku");
        }

        public void makniKomponentu(int i)
        {
            Korijen.makniKomponentu(i);
        }

        public void makniKomponentu(IComponent i)
        {
            Korijen.makniKomponentu(i);
        }

        public void izracunajVelicinu()
        {

        }

        public IComponent pronadiKomponentu(int i)
        {
            if (this.Oznaka == i)
                return this;
            else return null;
        }

        public IComponent pronadiKomponentu(string Naziv)
        {
            if (this.Naziv.ToLower() == Naziv.ToLower())
                return this;
            else return null;
        }

        public IComponent pronadiKomponentuUDirektoriju(string Naziv)
        {
            Console.WriteLine("Not a directory!");
            return null;
        }

        public string prikazi(int Dubina)
        {
            return String.Format("[{0,3}][F] {1, -30}[R{2}] velicina: {3}\n", this.Oznaka, new String(' ', Dubina) + this.Naziv, (this.DozvoliPisanje ? "W" : " "), this.Velicina);
        }
    }
}
