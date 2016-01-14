// ***********************************************************************
// Assembly         : tdukaric_zadaca_2
// Author           : Tomislav
// Created          : 11-25-2013
//
// Last Modified By : Tomislav
// Last Modified On : 11-28-2013
// ***********************************************************************
// <copyright file="load.cs" company="">
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

/// <summary>
/// The tdukaric_zadaca_2 namespace.
/// </summary>
namespace tdukaric_zadaca_2
{
    /// <summary>
    /// Class load.
    /// </summary>
    class load
    {
        /// <summary>
        /// The n
        /// </summary>
        int prag;
        /// <summary>
        /// Initializes a new instance of the <see cref="load" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="prag">The prag.</param>
        public load(string path, int prag)
        {
            this.prag = prag;
            loadTeams(path);
        }

        /// <summary>
        /// The teams
        /// </summary>
        private List<team> teams;

        /// <summary>
        /// Gets the teams.
        /// </summary>
        /// <returns>List{team}.</returns>
        public List<team> getTeams()
        {
            return this.teams;
        }


        /// <summary>
        /// Loads the teams.
        /// </summary>
        /// <param name="path">Path to file.</param>
        private void loadTeams(string path)
        {
            StreamReader streamReader = new StreamReader(path);
            this.teams = new List<team>();
            String record;

            while ((record = streamReader.ReadLine()) != null)
            {
                int id;
                int.TryParse(record.Substring(0, 5), out id);
                string name = record.Substring(5);
                teams.Add(new team(id, name, this.prag));
            }
        }
    }
}
