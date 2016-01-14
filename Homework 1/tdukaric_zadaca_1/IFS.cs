using System;
namespace tdukaric_zadaca_1
{
    public interface IFS
    {
        int Oznaka { get; set; }
        string DS_TIP { get; set; }
        IComponent Glavni { get; set; }

        void ispisiFS(IComponent Sto);
        bool kopirajKomponentu(int _Sto, int _Gdje);
        bool kopirajKomponentu(int Sto, int Gdje, string Naziv);
        bool kreirajKomponentu(int Gdje, string Naziv, bool JeDirektorij);
        bool kreirajKomponentuFS(int Gdje, string Naziv, bool JeDirektorij);
        void makniKomponentu(int Sto);
        void otvori(int Sto);
        void premjestiKomponentu(int Sto, int Gdje);
        string prikaziUnatrag(int Sto);
    }

}
