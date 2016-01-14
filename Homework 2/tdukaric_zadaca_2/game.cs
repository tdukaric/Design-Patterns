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
            int intervalSeconds;
            int controlInterval;
            int limit = 2;
            CareTakerTeams takerTeam = new CareTakerTeams();
            CareTakerResults takerResults = new CareTakerResults();
            CareTakerTeams takerTeamDifferences = new CareTakerTeams();
            
            if(!Int32.TryParse(args[1], out intervalSeconds))
            {
                Console.WriteLine("Can't parse second parameter.");
                return;
            }

            if(!Int32.TryParse(args[2], out controlInterval))
            {
                Console.WriteLine("Can't parse third parameter.");
                return;
            }

            if(!Int32.TryParse(args[3], out limit))
            {
                Console.WriteLine("Can't parse fourth parameter.");
                return;
            }

            load temp = new load(fileName, limit);
            teams t = new teams(temp.getTeams());
            match m = new match(t.ranking, takerTeam, takerTeamDifferences, takerResults);
            m.play(controlInterval, intervalSeconds, limit);
            //t.update();
            t.sort();
            Console.WriteLine();
            Console.WriteLine();

        }
    }
}
