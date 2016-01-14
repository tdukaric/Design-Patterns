// ***********************************************************************
// Assembly         : tdukaric_zadaca_2
// Author           : Tomislav
// Created          : 11-25-2013
//
// Last Modified By : Tomislav
// Last Modified On : 11-28-2013
// ***********************************************************************
// <copyright file="memento.cs" company="">
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
    /// Class MementoTeams.
    /// </summary>
    class MementoTeams
    {
        /// <summary>
        /// The teams
        /// </summary>
        public List<team> Teams;

        /// <summary>
        /// Initializes a new instance of the <see cref="MementoTeams" /> class.
        /// </summary>
        /// <param name="Teams">The teams.</param>
        public MementoTeams(List<team> Teams)
        {
            this.Teams = new List<team>();
            foreach (team t in Teams)
                this.Teams.Add(new team(t));
        }
    }

    /// <summary>
    /// Class CareTakerTeams.
    /// </summary>
    class CareTakerTeams
    {

        /// <summary>
        /// Gets the old.
        /// </summary>
        /// <value>The old.</value>
        public List<MementoTeams> Old { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="CareTakerTeams" /> class.
        /// </summary>
        public CareTakerTeams()
        {
            Old = new List<MementoTeams>();
        }
        /// <summary>
        /// Adds the memento.
        /// </summary>
        /// <param name="memento">The memento.</param>
        public void AddMemento(MementoTeams memento)
        {
            this.Old.Add(memento);
        }

        /// <summary>
        /// Gets the memento.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>MementoTeams.</returns>
        public MementoTeams GetMemento(int index)
        {
            return this.Old[index];
        }
    }

    /// <summary>
    /// Class OriginatorTeams.
    /// </summary>
    class OriginatorTeams
    {
        /// <summary>
        /// The teams
        /// </summary>
        public List<team> Teams;
        /// <summary>
        /// Restores the memento.
        /// </summary>
        /// <param name="memento">The memento.</param>
        public void RestoreMemento(MementoTeams memento)
        {
            this.Teams = memento.Teams;
        }
        /// <summary>
        /// Saves the memento.
        /// </summary>
        /// <returns>MementoTeams.</returns>
        public MementoTeams SaveMemento()
        {
            return (new MementoTeams(Teams.OrderByDescending(o=>o.points).ToList()));
        }
    }



    /// <summary>
    /// Class MementoResults.
    /// </summary>
    class MementoResults
    {
        /// <summary>
        /// The round
        /// </summary>
        public Dictionary<KeyValuePair<team, team>, int> round;

        /// <summary>
        /// Initializes a new instance of the <see cref="MementoResults" /> class.
        /// </summary>
        /// <param name="round">The round.</param>
        public MementoResults(Dictionary<KeyValuePair<team, team>, int> round)
        {
            this.round = new Dictionary<KeyValuePair<team, team>, int>();
            foreach (KeyValuePair<team, team> i in round.Keys)
            {
                this.round.Add(i, round[i]);
            }
        }
    }

    /// <summary>
    /// Class CareTakerResults.
    /// </summary>
    class CareTakerResults
    {

        /// <summary>
        /// Gets the old.
        /// </summary>
        /// <value>The old.</value>
        public List<MementoResults> Old { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="CareTakerResults" /> class.
        /// </summary>
        public CareTakerResults()
        {
            Old = new List<MementoResults>();
        }
        /// <summary>
        /// Adds the memento.
        /// </summary>
        /// <param name="memento">The memento.</param>
        public void AddMemento(MementoResults memento)
        {
            this.Old.Add(memento);
        }

        /// <summary>
        /// Gets the memento.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>MementoResults.</returns>
        public MementoResults GetMemento(int index)
        {
            return this.Old[index];
        }
    }

    /// <summary>
    /// Class OriginatorResults.
    /// </summary>
    class OriginatorResults
    {
        /// <summary>
        /// The round
        /// </summary>
        public Dictionary<KeyValuePair<team, team>, int> round;
        /// <summary>
        /// Restores the memento.
        /// </summary>
        /// <param name="memento">The memento.</param>
        public void RestoreMemento(MementoResults memento)
        {
            this.round = memento.round;
        }
        /// <summary>
        /// Saves the memento.
        /// </summary>
        /// <returns>MementoResults.</returns>
        public MementoResults SaveMemento()
        {
            return (new MementoResults(round));
        }
    }
}
