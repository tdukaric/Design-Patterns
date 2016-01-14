using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tdukaric_zadaca_1
{
    class exFAT : FS, IFS
    {
        private static exFAT instance;

        public static exFAT GetInstance(string Putanja, string DS_TIP)
        {
            if (instance == null)
            {
                instance = new exFAT(Putanja, DS_TIP);
            }
            return instance;
        }


        private exFAT(string Putanja, string DS_TIP)
        {
            this.DS_TIP = DS_TIP;
            this.Oznaka = 0;
            this.Glavni = new dir { Putanja = Putanja, Oznaka = 0, Naziv = Putanja, Korijen = null, Direktorij = true, Poveznica = false };
            ucitajKompozite(Putanja, Glavni);
        }

        private void ucitajListove(string Putanja, IComponent Korijen)
        {

            string[] Datoteke = Directory.GetFiles(Putanja);

            foreach (string Datoteka in Datoteke)
            {
                this.Oznaka++;
                FileInfo temp = new FileInfo(Datoteka);
                file dat;
                if (System.IO.File.GetAttributes(Datoteka).HasFlag(FileAttributes.ReparsePoint))
                {
                    continue;
                }
                else
                {
                    dat = new file { Putanja = Datoteka, Naziv = temp.Name, Velicina = temp.Length, Oznaka = this.Oznaka, Direktorij = false, Korijen = Korijen, DozvoliPisanje = !temp.IsReadOnly };
                }
                Korijen.dodajKomponentu(dat);
            }
        }

        private void ucitajKompozite(string Putanja, IComponent Korijen)
        {
            try
            {
                string[] Datoteke = Directory.GetFiles(Putanja, ".");
            }
            catch
            {
                Console.WriteLine("Direktorij ne postoji!");
                return;
            }

            try
            {
                string[] Direktoriji = Directory.GetDirectories(Putanja);
                foreach (string PutanjaDirektorija in Direktoriji)
                {
                    this.Oznaka++;
                    dir Direktorij = null;

                    if (System.IO.File.GetAttributes(PutanjaDirektorija).HasFlag(FileAttributes.ReparsePoint))
                        continue;
                    else
                        Direktorij = new dir { Putanja = PutanjaDirektorija, Oznaka = this.Oznaka, Naziv = new DirectoryInfo(PutanjaDirektorija).Name, Direktorij = true, Korijen = Korijen, Poveznica = false };

                    Korijen.dodajKomponentu(Direktorij);
                    ucitajKompozite(PutanjaDirektorija, Direktorij);
                }
                ucitajListove(Putanja, Korijen);
                Korijen.izracunajVelicinu();
            }
            catch
            {
                Console.WriteLine("Error");
            }
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

    }
}
