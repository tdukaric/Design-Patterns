// ***********************************************************************
// Assembly         : tdukaric_zadaca_2
// Author           : Tomislav
// Created          : 11-25-2013
//
// Last Modified By : Tomislav
// Last Modified On : 11-28-2013
// ***********************************************************************
// <copyright file="observer.cs" company="">
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
    /// Delegate handler
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">PrintEfArg.</param>
    delegate void handler(object sender, PrintEfArg e);

    /// <summary>
    /// Class PrintEfArg.
    /// </summary>
    class PrintEfArg : EventArgs
    {
        /// <summary>
        /// The team
        /// </summary>
        public team t;
        /// <summary>
        /// The efficiency
        /// </summary>
        public double e;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintEfArg" /> class.
        /// </summary>
        /// <param name="t">The team</param>
        /// <param name="e">The efficiency</param>
        public PrintEfArg(team t, double e)
        {
            this.t = t;
            this.e = e;
        }

    }

    /// <summary>
    /// Class PrintEfListener.
    /// </summary>
    class PrintEfListener
    {
        /// <summary>
        /// Prints the specified efficiency.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="e">PrintEfArg</param>
        public void Print(object o, PrintEfArg e)
        {
            if(e != null)
                Console.WriteLine("Klubu " + e.t.name + " se promijenila efikasnost s {0:N3} na {1:N3}", e.e, e.t.efficiency);
        }
    }
}
