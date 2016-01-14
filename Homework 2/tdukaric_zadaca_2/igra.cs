// ***********************************************************************
// Assembly         : tdukaric_zadaca_2
// Author           : Tomislav
// Created          : 11-25-2013
//
// Last Modified By : Tomislav
// Last Modified On : 11-28-2013
// ***********************************************************************
// <copyright file="igra.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// The tdukaric_zadaca_2 namespace.
/// </summary>
namespace tdukaric_zadaca_2
{
    /// <summary>
    /// Class game.
    /// </summary>
    static class game
    {
        /// <summary>
        /// Plays the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static public void play(string[] args)
        {
            string fileName = args[0];
            int intervalSekunde;
            int kontrolniInterval;
            int prag = 2;
            CareTakerTeams takerTim = new CareTakerTeams();
            CareTakerResults takerRez = new CareTakerResults();
            CareTakerTeams takerTimDiff = new CareTakerTeams();
            
            if(!Int32.TryParse(args[1], out intervalSekunde))
            {
                Console.WriteLine("Can't parse second parameter.");
                return;
            }

            if(!Int32.TryParse(args[2], out kontrolniInterval))
            {
                Console.WriteLine("Can't parse third parameter.");
                return;
            }

            if(!Int32.TryParse(args[3], out prag))
            {
                Console.WriteLine("Can't parse fourth parameter.");
                return;
            }

            load temp = new load(fileName, prag);
            teams t = new teams(temp.getTeams());
            match m = new match(t.ranking, takerTim, takerTimDiff, takerRez);
            m.play(kontrolniInterval, intervalSekunde, prag);
            //t.update();
            t.sort();
            Console.WriteLine();
            Console.WriteLine();

        }
    }
}
