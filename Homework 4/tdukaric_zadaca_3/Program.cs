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

using System;
using System.Linq;
using tdukaric_zadaca_3;

namespace tdukaric_zadaca_4
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
            string path = args[1];
            int sekunde;
            if (!int.TryParse(args[2], out sekunde))
                return;
            int maxSize;
            if (!int.TryParse(args[3], out maxSize))
                return;
            bool isByte = args[4] == "KB";
            bool isNS;
            bool isClean;
            if (isByte)
            {
                maxSize *= 1024;
                isNS = args[5] == "NS";
                try
                {
                    isClean = args[6] == "clean";
                }
                catch
                {
                    isClean = false;
                }

            }
            else
            {
                isNS = args[4] == "NS";
                try
                {
                    isClean = args[5] == "clean";
                }
                catch
                {
                    isClean = false;
                }
            }


            /*
            http://www.rfc-editor.org/rfc/
            */

            MVC_Model myModel;

            Storage storage;

            if (!isClean)
            {

                try
                {
                    tdukaric_zadaca_3.storage.LoadStorage();
                }
                catch
                {
                    storage = new Storage(maxSize, isByte, isNS, path);
                    tdukaric_zadaca_3.storage.SaveStorage(storage);
                }


                storage = tdukaric_zadaca_3.storage.LoadStorage();
                Page page = storage.GetPage(url);
                if (page != null)
                    myModel = new MVC_Model(page);
                else
                {
                    myModel = new MVC_Model(url);
                }
                 
            }
            else
                myModel = new MVC_Model(url);

            MVC_View myView = new MVC_View(sekunde);
            myView.initialize(myModel, maxSize, isByte, isNS, isClean, path);
        }
    }
}
