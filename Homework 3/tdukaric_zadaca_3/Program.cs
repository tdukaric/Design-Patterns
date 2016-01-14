// ***********************************************************************
// Assembly         : tdukaric_zadaca_3
// Author           : Tomislav
// Created          : 01-01-2014
//
// Last Modified By : Tomislav
// Last Modified On : 01-10-2014
// ***********************************************************************
// <copyright file="Program.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace tdukaric_zadaca_3
{
    /// <summary>
    /// Class Program.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {

            string url = args[0];
            int sekunde;
            if (!int.TryParse(args[1], out sekunde))
                return;
            

            /*
            http://www.rfc-editor.org/rfc/
            */
            MVC_Model myModel = new MVC_Model(url);
            MVC_View myView = new MVC_View(sekunde);
            myView.initialize(myModel);
        }
    }
}
