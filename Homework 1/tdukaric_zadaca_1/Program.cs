using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tdukaric_zadaca_1
{

    class Program
    {
        /// <summary>
        /// Switch-case za izvršavanje funkcija ovisno o naredbi - glavno sučelje aplikacije
        /// </summary>
        /// <param name="DatotecniSustav">Objekt tipa "FS" s inicijaliziranim datotečnim sustavom</param>
        static void naredbe(IFS DatotecniSustav)
        {
            string x = null;
            x = Console.ReadLine();
            x = x.Trim();

            int i;
            int j;
            string[] Naredbe = null;

            Naredbe = x.Split(' ');

            switch (Naredbe[0].ToLower())
            {
                case "ls":
                case "dir":
                    if (Naredbe.Length == 1)
                        Console.WriteLine(DatotecniSustav.Glavni.prikazi(0));
                    else
                        if (Int32.TryParse(Naredbe[1], out i))
                        {
                            IComponent temp = DatotecniSustav.Glavni.pronadiKomponentu(i);
                            if (temp == null)
                            {
                                Console.WriteLine("Objekt ne postoji!");
                                naredbe(DatotecniSustav);
                            }
                            Console.WriteLine(temp.prikazi(0));
                        }
                        else
                            Console.WriteLine("Krivo unesena naredba, pokrenite \"pomoc\" za više informacija");

                    break;

                case "copy":
                case "cp":
                    if (Int32.TryParse(Naredbe[1], out i) && Int32.TryParse(Naredbe[2], out j))
                        if (Naredbe.Length == 4)
                            DatotecniSustav.kopirajKomponentu(i, j, Naredbe[3]);
                        else
                            DatotecniSustav.kopirajKomponentu(i, j);
                    else
                        Console.WriteLine("Krivo unesena naredba, pokrenite \"pomoc\" za više informacija");
                    break;

                case "move":
                case "mv":
                    if (Int32.TryParse(Naredbe[1], out i) && Int32.TryParse(Naredbe[2], out j))
                        DatotecniSustav.premjestiKomponentu(i, j);
                    else
                        Console.WriteLine("Krivo unesena naredba, pokrenite \"pomoc\" za više informacija");
                    break;

                case "rm":
                    if (Int32.TryParse(Naredbe[1], out i))
                        DatotecniSustav.makniKomponentu(i);
                    else
                        Console.WriteLine("Krivo unesena naredba, pokrenite \"pomoc\" za više informacija");
                    break;

                case "rid":
                    if (Int32.TryParse(Naredbe[1], out i))
                        Console.WriteLine(DatotecniSustav.prikaziUnatrag(i));
                    else
                        Console.WriteLine("Krivo unesena naredba, pokrenite \"pomoc\" za više informacija");
                    break;

                case "mkdir":
                    if (Int32.TryParse(Naredbe[1], out i))
                        DatotecniSustav.kreirajKomponentuFS(i, Naredbe[2], true);
                    else
                        Console.WriteLine("Krivo unesena naredba, pokrenite \"pomoc\" za više informacija");
                    break;

                case "touch":
                    if (Int32.TryParse(Naredbe[1], out i))
                        DatotecniSustav.kreirajKomponentuFS(i, Naredbe[2], false);
                    else
                        Console.WriteLine("Krivo unesena naredba, pokrenite \"pomoc\" za više informacija");
                    break;

                case "help":
                case "pomoc":
                    StringBuilder Pomoc = new StringBuilder();
                    Pomoc.AppendLine("DIR [od kud]");
                    Pomoc.AppendLine("CP sto kamo [naziv]");
                    Pomoc.AppendLine("MV sto kamo [naziv]");
                    Pomoc.AppendLine("RM sto");
                    Pomoc.AppendLine("RID sto");
                    Pomoc.AppendLine("MKDIR gdje naziv");
                    Pomoc.AppendLine("TOUCH gdje naziv");
                    Pomoc.AppendLine("OTVORI sto");
                    Console.WriteLine(Pomoc.ToString());
                    break;

                case "open":
                case "otvori":
                    if (Int32.TryParse(Naredbe[1], out i))
                        DatotecniSustav.otvori(i);
                    else
                        Console.WriteLine("Krivo unesena naredba, pokrenite \"pomoc\" za više informacija");
                    break;

                case "quit":
                    System.Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Krivo unesena naredba, pokrenite \"pomoc\" za više informacija");
                    break;

            }
            Console.WriteLine();
            naredbe(DatotecniSustav);
        }

        static void Main(string[] args)
        {
            string DS_TIP = Environment.GetEnvironmentVariable("DS_TIP");
            if (DS_TIP == null)
            {
                Console.WriteLine("Varijabla DS_TIP nije definirana.");
                return;
            }

            if (!(DS_TIP == "NTFS" || DS_TIP == "exFAT"))
            {
                Console.WriteLine("Datotečni sustav nije podržan!");
                return;
            }

            IFS FileSystem;

            if (DS_TIP == "NTFS")
            {
                FileSystem = NTFS.GetInstance(args[0], DS_TIP);
            }
            else
            {
                FileSystem = exFAT.GetInstance(args[0], DS_TIP);
            }
            Console.WriteLine(DS_TIP);

            //FS FileSystem = FS.GetInstance(args[0], DS_TIP);
            FileSystem.ispisiFS(FileSystem.Glavni);
            Console.WriteLine("DS_TIP : " + FileSystem.DS_TIP);
            naredbe(FileSystem);
        }
    }
}
