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
        public bool Direktorij { get; set; }
        public int Oznaka { get; set; }
        public string Putanja { get; set; }
        public string Naziv { get; set; }
        private List<IComponent> Dijeca = new List<IComponent>();
        public IComponent Korijen { get; set; }
        public long Velicina { get; set; }
        public bool DozvoliPisanje { get; set; }
        public bool Poveznica { get; set; }

        public void dodajKomponentu(IComponent Komponenta)
        {
            Dijeca.Add(Komponenta);
            izracunajVelicinu();
        }

        public void makniKomponentu(IComponent i)
        {
            Dijeca.Remove(i);
            izracunajVelicinu();
        }

        public void makniKomponentu(int i)
        {
            IComponent Komponenta = pronadiKomponentu(i);
            if (Komponenta != null)
                Dijeca.Remove(Komponenta);
            else
                Console.WriteLine("Can't delete component");
        }

        public IComponent getComponent(int i)
        {
            return Dijeca[i];
        }

        public IComponent pronadiKomponentu(int i)
        {
            if (Oznaka == i)
                return this;
            IComponent x = null;
            foreach (IComponent c in Dijeca)
            {
                x = c.pronadiKomponentu(i);
                if (x != null)
                    return x;
            }
            return x;
        }

        public IComponent pronadiKomponentu(string Naziv)
        {
            if (this.Naziv.ToLower() == Naziv.ToLower())
                return this;
            IComponent x = null;
            foreach (IComponent c in Dijeca)
            {
                x = c.pronadiKomponentu(Naziv);
                if (x != null)
                    return x;
            }
            return x;
        }

        public IComponent pronadiKomponentuUDirektoriju(string Naziv)
        {
            if (this.Naziv.ToLower() == Naziv.ToLower())
                return this;
            foreach (IComponent c in Dijeca)
            {
                if (c.Naziv.ToLower() == Naziv.ToLower())
                    return this;
            }
            return null;
        }

        public string prikazi(int Dubina)
        {
            izracunajVelicinu();
            StringBuilder Rezultat = new StringBuilder();
            if (this.Korijen == null)
                Rezultat.Append(String.Format("[{0,3}][{1}] {2, -30}     velicina: {3}\n", this.Oznaka, (this.Poveznica ? "L": "D"), new String(' ', Dubina) + this.Putanja, this.Velicina));
            else
                Rezultat.Append(String.Format("[{0,3}][{1}] {2, -30}     velicina: {3}\n", this.Oznaka, (this.Poveznica ? "L" : "D"), new String(' ', Dubina) + this.Naziv, this.Velicina));

            foreach (IComponent component in Dijeca)
            {
                Rezultat.Append(component.prikazi(Dubina + 2));
            }
            return Rezultat.ToString();
        }

        public void izracunajVelicinu()
        {
            this.Velicina = 0;
            foreach (IComponent Dijete in this.Dijeca)
            {
                if (!Dijete.Poveznica)
                {
                    Dijete.izracunajVelicinu();
                    Velicina += Dijete.Velicina;
                }
            }
        }

    }
}
