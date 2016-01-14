// ***********************************************************************
// Assembly         : tdukaric_zadaca_2
// Author           : Tomislav
// Created          : 11-25-2013
//
// Last Modified By : Tomislav
// Last Modified On : 11-28-2013
// ***********************************************************************
// <copyright file="team.cs" company="">
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
    /// Class team.
    /// </summary>
    class team
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public String name {get; private set;}
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int ID { get; private set; }
        /// <summary>
        /// The points
        /// </summary>
        public int points;
        /// <summary>
        /// The rank
        /// </summary>
        public int rank;
        /// <summary>
        /// The round_points
        /// </summary>
        public int round_points;
        /// <summary>
        /// The participated
        /// </summary>
        public int participated;
        /// <summary>
        /// The efficiency
        /// </summary>
        public double efficiency;
        /// <summary>
        /// The state
        /// </summary>
        public Context State;

        /// <summary>
        /// The prag
        /// </summary>
        int prag;

        /// <summary>
        /// Initializes a new instance of the <see cref="team" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="prag">The prag.</param>
        public team(int id, String name, int prag)
        {
            this.name = name;
            this.ID = id;
            this.points = 0;
            this.rank = 1;
            this.State = new Context(new normal(prag));
            this.efficiency = 0;
            this.prag = prag;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="team" /> class.
        /// </summary>
        /// <param name="t">The team</param>
        public team(team t)
        {
            this.name = t.name;
            this.ID = t.ID;
            this.points = t.points;
            this.rank = t.rank;
            this.round_points = t.round_points;
            this.participated = t.participated;
            this.efficiency = t.efficiency;
            this.State = new Context(t.State.state);
            this.prag = t.prag;
        }

        /// <summary>
        /// Calculates the efficiency.
        /// </summary>
        public void calculateEfficiency()
        {
            try
            {
                this.efficiency = (double)this.points / (double)this.participated;
            }
            catch
            {
                this.efficiency = 0;
            }
        }
    }
}
