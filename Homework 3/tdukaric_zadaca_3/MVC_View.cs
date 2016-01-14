// ***********************************************************************
// Assembly         : tdukaric_zadaca_3
// Author           : Tomislav
// Created          : 01-07-2014
//
// Last Modified By : Tomislav
// Last Modified On : 01-10-2014
// ***********************************************************************
// <copyright file="MVC_View.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Timers;

namespace tdukaric_zadaca_3
{
    /// <summary>
    /// Class MVC_View.
    /// </summary>
    class MVC_View : IDisposable
    {
        /// <summary>
        /// My model
        /// </summary>
        MVC_Model myModel;
        /// <summary>
        /// My controller
        /// </summary>
        MVC_Controller myController;
        /// <summary>
        /// The timer
        /// </summary>
        private Timer timer;

        /// <summary>
        /// The sekunde
        /// </summary>
        int seconds;

        /// <summary>
        /// Initializes a new instance of the <see cref="MVC_View"/> class.
        /// </summary>
        /// <param name="seconds">The sekunde.</param>
        public MVC_View(int seconds)
        {
            this.seconds = seconds;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            timer.Dispose();
        }

        /// <summary>
        /// Initializes the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
        public bool initialize(MVC_Model model)
        {
            this.myModel = model;
            makeController();

            this.commands();
            return true;
        }

        /// <summary>
        /// Makes the controller.
        /// </summary>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
        private bool makeController()
        {
            myController = new MVC_Controller();
            this.reloadController();
            return true;
        }

        /// <summary>
        /// Reloads the controller.
        /// </summary>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
        private bool reloadController()
        {
            myController.initialize(myModel, this);
            myModel.attachController(myController);
            myModel.attachView(this);

            timer = new Timer();
            timer.Elapsed += new ElapsedEventHandler(myController.reloadLinksAuto);
            timer.Interval = this.seconds * 1000;
            timer.Start();

            return true;
        }

        /// <summary>
        /// Commandses this instance.
        /// </summary>
        public void commands()
        {
            Console.WriteLine("Commands:");
            Console.WriteLine("-B - print number of links");
            Console.WriteLine("-I - print link by number");
            Console.WriteLine("-J n - go to link by number");
            Console.WriteLine("-R - refresh current web page");
            Console.WriteLine("-S - print work statistics");
            Console.WriteLine("-U - print current URL");

            Console.WriteLine("-A - back to previous page");

            Console.WriteLine("-Q - quit");
            string command = Console.ReadLine();
            if (command.Length < 2)
                this.commands();
            switch (command[1])
            {
                case 'B':
                    Console.WriteLine("Number of links: " + myController.numLinks());
                    break;
                case 'I':
                    List<KeyValuePair<string, string>> links = myController.getURLs();
                    int id = 0;
                    foreach (KeyValuePair<string, string> link in links)
                    {
                        Console.WriteLine("ID: " + id + "\tURL: " + link.Key);
                        Console.WriteLine("Type: " + myController.getType(link.Key));
                        id++;
                    }
                    break;
                case 'J':
                    string[] commands = command.Split(' ');
                    int _id;
                    if (commands.Length == 1)
                        break;
                    if (int.TryParse(commands[1], out _id))
                    {
                        string _url = myController.getURL(_id);
                        string tip = myController.getType(_url);
                        if (tip == "link/other")
                        {
                            myModel = myController.newPage(_id);
                            myModel.loadTime = DateTime.Now;
                            timer.Stop();
                            reloadController();
                        }
                        else if (tip == "email")
                        {
                            Process.Start(_url);
                        }
                        else
                            try
                            {
                                var request = System.Net.WebRequest.Create(_url);
                                using (var response = request.GetResponse())
                                {
                                    Console.WriteLine("Name: " + Path.GetFileName(_url));
                                    Console.WriteLine("Type " + response.ContentType);
                                    Console.WriteLine("Size: " + response.ContentLength);
                                    Console.WriteLine("Open? (y for yes) [no]");
                                    string key = Console.ReadLine();
                                    if (key == "y")
                                    {
                                        try
                                        {
                                            WebClient client = new WebClient();
                                            Uri uri = new Uri(_url);
                                            client.DownloadFile(uri, Path.GetFileName(uri.LocalPath));
                                            Process.Start(Path.GetFileName(uri.LocalPath));

                                        }
                                        catch
                                        {
                                            Console.WriteLine("Greška!");
                                        }
                                    }

                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                            }
                    }
                    else
                    {
                        Console.WriteLine("Error during parsing!");
                        break;
                    }
                    break;
                case 'A':
                    myModel = myController.goBack();
                    timer.Stop();
                    reloadController();
                    break;
                case 'R':
                    myModel.updateLinksManual();
                    break;
                case 'S':
                    Console.WriteLine("Previous opened pages: ");
                    myController.showStatistics();
                    Console.WriteLine("Current opened page: " + this.myModel.url);
                    Console.WriteLine("Waiting time: " + (myModel.VisitTime + DateTime.Now.Subtract(myModel.loadTime).Seconds));
                    Console.WriteLine("Number of manual refresh: " + myModel.ReloadTimesManual);
                    Console.WriteLine("Number of automatic refresh: " + myModel.ReloadTimesAuto);
                    Console.WriteLine("Number of changes on the page: " + myModel.noChanges);

                    break;
                case 'U':
                    Console.WriteLine(myModel.url);
                    break;
                case 'Q':
                    return;
            }
            this.commands();
        }

        /// <summary>
        /// Events the triggered.
        /// </summary>
        public void eventTriggered()
        {
            Console.WriteLine("Change on the page occured!");
            return;
        }
    }
}
