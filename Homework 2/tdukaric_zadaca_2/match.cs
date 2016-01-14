// ***********************************************************************
// Assembly         : tdukaric_zadaca_2
// Author           : Tomislav
// Created          : 11-25-2013
//
// Last Modified By : Tomislav
// Last Modified On : 11-28-2013
// ***********************************************************************
// <copyright file="match.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

/// <summary>
/// The tdukaric_zadaca_2 namespace.
/// </summary>
namespace tdukaric_zadaca_2
{



    /// <summary>
    /// Class match.
    /// </summary>
    class match
    {
        /// <summary>
        /// The teams
        /// </summary>
        public List<team> teams;
        /// <summary>
        /// The round
        /// </summary>
        public Dictionary<KeyValuePair<team, team>, int> round;
        /// <summary>
        /// The taker team
        /// </summary>
        public CareTakerTeams takerTeam;
        /// <summary>
        /// The taker team difference
        /// </summary>
        public CareTakerTeams takerTeamDiff;
        /// <summary>
        /// The taker rez
        /// </summary>
        public CareTakerResults takerRez;
        /// <summary>
        /// The timer
        /// </summary>
        private Timer timer;
        /// <summary>
        /// Occurs when [h].
        /// </summary>
        public static event handler h;
        /// <summary>
        /// The n_rounds
        /// </summary>
        int n_rounds;
        /// <summary>
        /// The n
        /// </summary>
        int n = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="match"/> class.
        /// </summary>
        /// <param name="ranking">The ranking.</param>
        /// <param name="takerTeam">The taker team.</param>
        /// <param name="takerTeamDiff">The taker team difference.</param>
        /// <param name="takerRez">The taker rez.</param>
        public match(List<team> ranking, CareTakerTeams takerTeam, CareTakerTeams takerTeamDiff, CareTakerResults takerRez)
        {
            this.teams = new List<team>();
            foreach (team tim in ranking)
            {
                tim.round_points = 0;
                teams.Add(tim);
            }
            this.takerTeam = takerTeam;
            this.takerRez = takerRez;
            this.takerTeamDiff = takerTeamDiff;

            PrintEfListener listen = new PrintEfListener();
            h += new handler(listen.Print);
        }

        /// <summary>
        /// Plays for specified n_rounds.
        /// </summary>
        /// <param name="n_rounds">The n_rounds.</param>
        /// <param name="sec">The seconds.</param>
        /// <param name="prag">The prag.</param>
        public void play(int n_rounds, int sec, int prag)
        {
            this.round = new Dictionary<KeyValuePair<team, team>, int>();
            this.n_rounds = n_rounds;
            Console.WriteLine("Pritisak na bilo koji tipku pokreće simulaciju.");
            Console.WriteLine("Za prekid simulacije pritisnite tipku 'p' ili pričekajte kraj simulacije.");
            Console.ReadKey(true);
            updateMemento();

            timer = new System.Threading.Timer(new TimerCallback(play), null, 0, 1000 * sec);

            while (Console.ReadKey(true).KeyChar != 'p')
            {
                
            }

            timer.Dispose();
            commands();

        }

        /// <summary>
        /// Application commands.
        /// </summary>
        private void commands()
        {
            int id;
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Commands:");
            Console.WriteLine("1 - ispis ukupnog broja arhiviranih redoslijeda klubova");
            Console.WriteLine("2 - ispis određenog redoslijeda klubova prema izabranom rednom broju");
            Console.WriteLine("3 - ispis rezultate određenog kola koje je povezano s izabranim rednim brojem redoslijeda kluba");
            Console.WriteLine("4 - ispis rezultata izabranog kluba");
            Console.WriteLine("q - izlaz");
            
            char c = Console.ReadKey(true).KeyChar;
            switch (c)
            {
                case '1':
                    //ispis ukupnog broja arhiviranih redoslijeda klubova
                    Console.WriteLine("Ukupan broj arhiviranih redoslijeda klubova: " + (takerTeamDiff.Old.Count() - 1));
                    break;
                case '2':
                    //ispis rezultata određenog kola koje je povezano s izabranim rednim brojem redoslijeda klubova
                    Console.WriteLine("Upisite broj od 1 do " + (takerRez.Old.Count() - 1));
                    if (int.TryParse(Console.ReadLine(), out id))
                        printMatch(id + 1);
                    break;
                case '3':
                    //ispis određenog redoslijeda klubova prema izabranom rednom broju
                    Console.WriteLine("Upisite broj od 1 do " + (takerTeamDiff.Old.Count() - 1));
                    if (int.TryParse(Console.ReadLine(), out id))
                        printResultsDiff(id + 1);
                    break;
                case '4':
                    // ispis svih povijesnih rezultata određenog kluba prema izboru
                    foreach (team t in this.teams)
                        Console.WriteLine(t.ID + ". " + t.name);
                    Console.WriteLine("Odaberite tim upisom oznake tima: ");
                    if (int.TryParse(Console.ReadLine(), out id))
                    {
                        printTeamResult(teams.First(t => t.ID == id));
                    }
                    break;
                case 'd':
                    //ispis određenog redoslijeda klubova prema izabranom rednom broju
                    Console.WriteLine("Upisite broj od 1 do " + (takerTeam.Old.Count() - 1));
                    if (int.TryParse(Console.ReadLine(), out id))
                        printResults(id + 1);
                    break;
                case 'q':
                    return;
                default:
                    break;
            }
            commands();
        }

        /// <summary>
        /// Play.
        /// </summary>
        /// <param name="obj">null</param>
        private void play(object obj)
        {
            //Used for Random
            System.Threading.Thread.Sleep(1);

            int n = teams.Count();
            Random rand = new Random();

            List<team> temp = new List<team>();
            Dictionary<team, team> games = new Dictionary<team, team>();

            this.round = new Dictionary<KeyValuePair<team, team>, int>();

            foreach (team tim in teams)
            {
                if (!(tim.State.state is thrown))
                    temp.Add(tim);
            }

            //Stops the timer if there are less than 3 players
            if (temp.Count < 3)
            {
                timer.Dispose();
                Console.WriteLine("Pritisnite 'p' za naredbe.");
                return;
            }

            //Randomizing teams, selecting teams for matches
            while (temp.Count > 1)
            {
                int id1 = rand.Next(0, teams.Count);
                id1 = id1 % temp.Count;
                team player1 = temp[id1];
                temp.Remove(player1);
                int id2 = rand.Next(0, teams.Count);
                id2 = id2 % temp.Count;
                team player2 = temp[id2];
                temp.Remove(player2);
                games.Add(player1, player2);
            }

            //"Playing" the game -> calculating winners using Random, adding points to teams
            Random rand2 = new Random();
            foreach (team player in games.Keys)
            {
                team player2 = games[player];
                player.round_points = 0;
                player2.round_points = 0;
                int rez = rand2.Next(0, 3);
                switch (rez)
                {
                    case 0:
                        player.round_points++;
                        player2.round_points++;
                        break;
                    case 1:
                        player.round_points += 2;
                        break;
                    case 2:
                        player2.round_points += 2;
                        break;
                }
                player.participated++;
                player2.participated++;
                player.points += player.round_points;
                player2.points += player2.round_points;
                round.Add(new KeyValuePair<team, team>(player, player2), rez);
            }

            statistics();

            //Console.Clear();

            List<PrintEfArg> list = UpdateEfficiency();

            Console.WriteLine("=====================================================");

            this.n++;
            if (this.n_rounds == this.n)
            {
                updateStates();
                updateMemento();
                //Console.WriteLine("=====================================================");
                #if DEBUG
                printResults(takerTeam.Old.Count);
                #endif
                this.n = 0;
            }
            else
                updateMemento();

            /*
            if (blah)
            {
                printResults(takerTeam.Old.Count);
                blah = false;
            }

            printResults(takerTeam.Old.Count);
            */

            //Event triggering for every change in efficiency
            foreach (PrintEfArg pr in list)
                h(new object(), pr);

        }

        /// <summary>
        /// Updates the memento.
        /// </summary>
        private void updateMemento()
        {
            OriginatorTeams orgTim = new OriginatorTeams();
            OriginatorResults orgRez = new OriginatorResults();
            OriginatorTeams orgTimDiff = new OriginatorTeams();
            orgRez.round = this.round;
            orgTim.Teams = this.teams.OrderByDescending(o => o.points).ToList();
            orgTimDiff.Teams = this.teams.OrderByDescending(o => o.points).ToList();
            
            this.takerTeam.AddMemento(orgTim.SaveMemento());

            if (takerTeamDiff.Old.Count > 1)
            {
                int id = 0;
                foreach (team t in orgTimDiff.Teams)
                {
                    if (takerTeamDiff.Old[takerTeamDiff.Old.Count - 1].Teams[takerTeamDiff.Old[takerTeamDiff.Old.Count - 1].Teams.FindIndex(p => p.ID == orgTimDiff.Teams[orgTimDiff.Teams.IndexOf(t)].ID)].rank == orgTimDiff.Teams[orgTimDiff.Teams.IndexOf(t)].rank)
                    {
                        id++;
                    }
                }

                if (id != orgTimDiff.Teams.Count)
                {
                    this.takerTeamDiff.AddMemento(orgTim.SaveMemento());
                    this.takerRez.AddMemento(orgRez.SaveMemento());
                }
            }
            else
            {
                this.takerTeamDiff.AddMemento(orgTim.SaveMemento());
                this.takerRez.AddMemento(orgRez.SaveMemento());
            }
        }


        /// <summary>
        /// Statisticses this instance.
        /// </summary>
        private void statistics()
        {
            this.teams = this.teams.OrderByDescending(o => o.points).ToList();
            int points = this.teams[0].points;

            int id = 1;
            int id_last = 1;
            foreach (team t in this.teams)
            {
                if (points == t.points)
                    t.rank = id_last;
                else
                {
                    points = t.points;
                    id_last = id;
                    t.rank = id;
                }
                id++;
            }
        }

        /// <summary>
        /// Updates the efficiency.
        /// </summary>
        /// <returns>List{PrintEfArg}.</returns>
        private List<PrintEfArg> UpdateEfficiency()
        {
            List<PrintEfArg> list = new List<PrintEfArg>();
            foreach (team t in teams)
            {
                if (t.State != null)
                    t.calculateEfficiency();
                double eff = this.takerTeam.Old[this.takerTeam.Old.Count - 1].Teams.Find(o => o.name == t.name).efficiency;

                if (t.efficiency != this.takerTeam.Old[this.takerTeam.Old.Count - 1].Teams.Find(o => o.name == t.name).efficiency)
                {
                    list.Add(new PrintEfArg(t, eff));

                }

            }

            return list;
        }

        /// <summary>
        /// Updates the states.
        /// </summary>
        private void updateStates()
        {
            foreach (team t in teams)
            {
                if (t.State != null)
                    if (this.takerTeam.Old.Count > this.n_rounds + 1)
                    {
                        t.State.Update(t.rank, this.takerTeam.Old[this.takerTeam.Old.Count - this.n_rounds].Teams.Find(o => o.name == t.name).rank);

                    }
                    else
                        t.State.Update(t.rank, this.takerTeam.Old[this.takerTeam.Old.Count - this.n_rounds + 1].Teams.Find(o => o.name == t.name).rank);

            }
        }

        /// <summary>
        /// Prints the match.
        /// </summary>
        /// <param name="idMatch">The identifier match.</param>
        private void printMatch(int idMatch)
        {
            if ((idMatch < 2) || (idMatch > takerRez.Old.Count))
            {
                Console.WriteLine("Nema tog kola!");
                return;
            }
            idMatch--;

            Dictionary<KeyValuePair<team, team>, int> round = new Dictionary<KeyValuePair<team, team>, int>(takerRez.Old[idMatch].round);

            foreach (KeyValuePair<team, team> match in round.Keys)
            {
                Console.WriteLine("U rundi " + idMatch + " igrali su " + match.Key.name + " i " + match.Value.name + ".");
                switch (round[match])
                {
                    case 0:
                        Console.WriteLine("Rezultat je bio: nerješeno.");
                        break;
                    case 1:
                        Console.WriteLine("{0} je pobijedio  {1}", match.Key.name, match.Value.name);
                        break;
                    case 2:
                        Console.WriteLine("{0} je pobijedio  {1}", match.Value.name, match.Key.name);
                        break;
                }
            }
        }

        /// <summary>
        /// Prints the results.
        /// </summary>
        /// <param name="idMatch">The identifier match.</param>
        private void printResults(int idMatch)
        {
            if ((idMatch < 1) || (idMatch > takerTeam.Old.Count))
            {
                Console.WriteLine("Nema tog kola!");
                return;
            }

            idMatch--;

            List<team> Teams = new List<team>(takerTeam.Old[idMatch].Teams);

            Teams = Teams.OrderByDescending(p => p.points).ToList();
            Console.WriteLine(String.Format("┌───────┬──────────────────────┬─────────┬──────────┬────────────┐"));

            Console.WriteLine(String.Format("│ {0, 5} │ {1, -20} │ {2, 7} │ {3, -8} │ {4, -10} │", "rank", "naziv", "bodovi", "status", "efikasnost"));

            Console.WriteLine(String.Format("├───────┼──────────────────────┼─────────┼──────────┼────────────┤"));

            foreach (team p in Teams)
            {
                Console.WriteLine(String.Format("│ {0, 5} │ {1, -20} │ {2, 7} │ {3, -8} │ {4, -10:F7} │", p.rank, p.name, p.points, (p.State.state is thrown ? "izbacen" : p.State.state is weak ? "slab" : "normalan"), p.efficiency));
            }

            Console.WriteLine(String.Format("└───────┴──────────────────────┴─────────┴──────────┴────────────┘"));
        }

        /// <summary>
        /// Prints the results difference.
        /// </summary>
        /// <param name="idMatch">The identifier match.</param>
        private void printResultsDiff(int idMatch)
        {
            if ((idMatch < 1) || (idMatch > takerTeamDiff.Old.Count))
            {
                Console.WriteLine("Nema tog kola!");
                return;
            }

            idMatch--;

            List<team> Teams = new List<team>(takerTeamDiff.Old[idMatch].Teams);

            Teams = Teams.OrderByDescending(p => p.points).ToList();
            Console.WriteLine(String.Format("┌───────┬──────────────────────┬─────────┬──────────┬────────────┐"));

            Console.WriteLine(String.Format("│ {0, 5} │ {1, -20} │ {2, 7} │ {3, -8} │ {4, -10} │", "rank", "naziv", "bodovi", "status", "efikasnost"));

            Console.WriteLine(String.Format("├───────┼──────────────────────┼─────────┼──────────┼────────────┤"));

            foreach (team p in Teams)
            {
                Console.WriteLine(String.Format("│ {0, 5} │ {1, -20} │ {2, 7} │ {3, -8} │ {4, -10:F7} │", p.rank, p.name, p.points, (p.State.state is thrown ? "izbacen" : p.State.state is weak ? "slab" : "normalan"), p.efficiency));
            }

            Console.WriteLine(String.Format("└───────┴──────────────────────┴─────────┴──────────┴────────────┘"));
        }

        /// <summary>
        /// Prints the team result.
        /// </summary>
        /// <param name="t">The t.</param>
        private void printTeamResult(team t)
        {
            foreach (MementoResults rounds in takerRez.Old)
                foreach (KeyValuePair<KeyValuePair<team, team>, int> k in rounds.round)
                {
                    if (k.Key.Value == t || k.Key.Key == t)
                    {
                        switch (k.Value)
                        {
                            case 0:
                                Console.WriteLine("{0,-20} - nerješeno -   {1, -20}", k.Key.Key.name, k.Key.Value.name);
                                break;
                            case 1:
                                Console.WriteLine("{0,-20} je pobijedio    {1, -20}", k.Key.Key.name, k.Key.Value.name);
                                break;
                            case 2:
                                Console.WriteLine("{1,-20} je pobijedio    {0, -20}", k.Key.Key.name, k.Key.Value.name);
                                break;
                        }
                    }
                }
        }
    }
}
