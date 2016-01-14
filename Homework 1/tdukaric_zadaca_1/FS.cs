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
        public IComponent Glavni { get; set; }
        public string DS_TIP { get; set; }
        public int Oznaka { get; set; }

        public void premjestiKomponentu(int Sto, int Gdje)
        {
            if (kopirajKomponentu(Sto, Gdje))
            {
                makniKomponentu(Sto);
            }
            else
                Console.WriteLine("Greška, nemogu premjestiti.");
        }

        public bool kopirajKomponentu(int _Sto, int _Gdje)
        {
            IComponent Sto = Glavni.pronadiKomponentu(_Sto);
            IComponent Gdje = Glavni.pronadiKomponentu(_Gdje);

            if (Sto == null || Gdje == null)
            {
                Console.WriteLine("Nemogu pronaći objekt!");
                return false;
            }
            if (Gdje.Direktorij)
                if (Gdje.pronadiKomponentuUDirektoriju(Sto.Naziv) == null)
                {

                    IComponent temp;
                    this.Oznaka++;
                    if (Sto.Direktorij)
                    {
                        DirectoryInfo dir = Directory.CreateDirectory(Gdje.Putanja + '\\' + Sto.Naziv);
                        kopirajDirektorij(Sto.Putanja, dir.FullName, Gdje);

                    }
                    else
                    {
                        string Destinacija = System.IO.Path.Combine(Gdje.Putanja, Sto.Naziv);
                        System.IO.File.Copy(Sto.Putanja, Destinacija, true);

                        FileInfo DatotekaInfo = new FileInfo(Destinacija);

                        temp = new file { Oznaka = this.Oznaka, Naziv = Sto.Naziv, Korijen = Gdje, Putanja = Destinacija, Velicina = Sto.Velicina, DozvoliPisanje = !DatotekaInfo.IsReadOnly };
                        Gdje.dodajKomponentu(temp);
                    }

                    Gdje.izracunajVelicinu();

                    return true;
                }
                else
                    Console.WriteLine("Greška, provjerite ime.");
            else
                Console.WriteLine("Greška, ne mogu kopirati objekt.");
            return false;
        }

        private void kopirajDirektorij(string Sto, string Gdje, IComponent Korijen)
        {

            DirectoryInfo dir = new DirectoryInfo(Sto);
            DirectoryInfo[] dirs = dir.GetDirectories();

            DirectoryInfo _dir = Directory.CreateDirectory(Gdje);

            this.Oznaka++;
            dir Direktorij = null;
            if (DS_TIP == "NTFS")
            {
                Direktorij = new dir { Putanja = _dir.FullName, Oznaka = this.Oznaka, Naziv = _dir.Name, Direktorij = true, Korijen = Korijen, Poveznica = System.IO.File.GetAttributes(_dir.FullName).HasFlag(FileAttributes.ReparsePoint) };
            }
            else if (DS_TIP == "exFAT")
            {
                if (!System.IO.File.GetAttributes(_dir.FullName).HasFlag(FileAttributes.ReparsePoint))
                    Direktorij = new dir { Putanja = _dir.FullName, Oznaka = this.Oznaka, Naziv = new DirectoryInfo(_dir.FullName).Name, Direktorij = true, Korijen = Korijen, Poveznica = false };
            }

            Korijen.dodajKomponentu(Direktorij);

            foreach (DirectoryInfo subdir in dirs)
            {
                string temppath = Path.Combine(Gdje, subdir.Name);
                kopirajDirektorij(subdir.FullName, temppath, Direktorij);
            }
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                this.Oznaka++;
                string temppath = Path.Combine(Gdje, file.Name);
                FileInfo temp = file.CopyTo(temppath, false);

                file dat = new file { Putanja = temp.FullName, Naziv = temp.Name, Velicina = temp.Length, Oznaka = this.Oznaka, Direktorij = false, Korijen = Direktorij, DozvoliPisanje = !temp.IsReadOnly, Poveznica = System.IO.File.GetAttributes(temp.FullName).HasFlag(FileAttributes.ReparsePoint) };
                Direktorij.dodajKomponentu(dat);

            }
        }

        public bool kopirajKomponentu(int Sto, int Gdje, string Naziv)
        {
            IComponent _Sto = Glavni.pronadiKomponentu(Sto);
            string temp = _Sto.Naziv;
            _Sto.Naziv = Naziv;
            if (kopirajKomponentu(Sto, Gdje))
                _Sto.Naziv = temp;
            else
            {
                _Sto.Naziv = Naziv;
                return false;
            }
            return true;
        }

        public void makniKomponentu(int Sto)
        {
            IComponent _Sto = Glavni.pronadiKomponentu(Sto);
            if (_Sto == null)
            {
                Console.WriteLine("Can't find object.");
                return;
            }
            if (_Sto.Direktorij)
            {
                System.IO.DirectoryInfo DirektorijInfo = new System.IO.DirectoryInfo(_Sto.Putanja);
                try
                {
                    DirektorijInfo.Delete(true);
                }
                catch
                {
                    Console.WriteLine("Greška pri brisanju komponente.");
                }

            }
            else
            {
                System.IO.FileInfo DatotekaInfo = new System.IO.FileInfo(_Sto.Putanja);
                try
                {
                    DatotekaInfo.Delete();
                }
                catch
                {
                    Console.WriteLine("Greška pri brisanju komponente.");
                }
            }
            _Sto.Korijen.makniKomponentu(_Sto);
        }

        public void ispisiFS(IComponent root)
        {
            Console.WriteLine(root.prikazi(0));
        }

        public bool kreirajKomponentu(int Gdje, string Naziv, bool JeDirektorij)
        {
            IComponent _Gdje = Glavni.pronadiKomponentu(Gdje);
            if (!_Gdje.Direktorij)
            {
                Console.WriteLine("Destinacija mora biti direktorij!");
                return false;
            }
            else
            {
                this.Oznaka++;
                IComponent Korijen = null;
                if (JeDirektorij)
                {
                    Korijen = new dir { Putanja = _Gdje.Putanja + '\\' + Naziv, Oznaka = this.Oznaka, Naziv = Naziv, Direktorij = true, Korijen = _Gdje, Poveznica = false };
                }
                else
                {
                    Korijen = new file { Putanja = _Gdje.Putanja + '\\' + Naziv, Oznaka = this.Oznaka, Naziv = Naziv, Direktorij = false, Korijen = _Gdje, Poveznica = false, DozvoliPisanje = true };
                }
                if (Korijen == null)
                    return false;
                else
                {
                    _Gdje.dodajKomponentu(Korijen);
                    _Gdje.izracunajVelicinu();
                }
            }
            return true;
        }

        public bool kreirajKomponentuFS(int Gdje, string Naziv, bool JeDirektorij)
        {
            IComponent _temp = Glavni.pronadiKomponentu(Gdje);
            if ((_temp != null) && (Glavni.pronadiKomponentu(Gdje).pronadiKomponentuUDirektoriju(Naziv.ToLower()) == null))
            {
                string Putanja = _temp.Putanja + '\\' + Naziv;
                if (JeDirektorij)
                {
                    Directory.CreateDirectory(Putanja);
                    return kreirajKomponentu(Gdje, Naziv, JeDirektorij);
                }
                else
                {
                    File.Create(Putanja);
                    return kreirajKomponentu(Gdje, Naziv, JeDirektorij);
                }
            }
            else
            {
                Console.WriteLine("Greška kod kreiranja komponente!");
                return false;
            }
        }

        public String prikaziUnatrag(int Sto)
        {
            StringBuilder Rezultat = new StringBuilder();
            IComponent _temp = Glavni.pronadiKomponentu(Sto);
            while (_temp != null)
            {
                Rezultat.Append(_temp.Naziv);
                Rezultat.Append('\\');
                _temp = _temp.Korijen;
            }
            Rezultat = Rezultat.Remove(Rezultat.Length - 1, 1);
            return Rezultat.ToString();
        }

        public void otvori(int Sto)
        {
            IComponent _Sto = Glavni.pronadiKomponentu(Sto);
            if (_Sto != null)
                System.Diagnostics.Process.Start(_Sto.Putanja);
        }
    }
}
