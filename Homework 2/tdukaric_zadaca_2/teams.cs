// ***********************************************************************
// Assembly         : tdukaric_zadaca_2
// Author           : Tomislav
// Created          : 11-25-2013
//
// Last Modified By : Tomislav
// Last Modified On : 11-27-2013
// ***********************************************************************
// <copyright file="teams.cs" company="">
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
    /// Class teams.
    /// </summary>
    class teams
    {
        /// <summary>
        /// Gets the teams.
        /// </summary>
        /// <value>The teams.</value>
        public List<team> Teams { get; private set; }
        /// <summary>
        /// Gets the ranking.
        /// </summary>
        /// <value>The ranking.</value>
        public List<team> ranking { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="teams" /> class.
        /// </summary>
        /// <param name="Teams">The teams.</param>
        public teams(List<team> Teams)
        {
            this.Teams = Teams;
            this.ranking = Teams;
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        public void update()
        {
            foreach(team p in ranking)
                p.points += p.round_points;
        }

        /// <summary>
        /// Sorts this instance.
        /// </summary>
        public void sort()
        {
            ranking = ranking.OrderByDescending(p => p.points).ToList();
        }

        
    }
}
