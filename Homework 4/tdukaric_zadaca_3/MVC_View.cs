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
using tdukaric_zadaca_3;

namespace tdukaric_zadaca_4
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
        int sekunde;

        /// <summary>
        /// Initializes a new instance of the <see cref="MVC_View"/> class.
        /// </summary>
        /// <param name="seconds">The seconds.</param>
        public MVC_View(int seconds)
        {
            this.sekunde = seconds;
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
        public bool initialize(MVC_Model model, long maxSize, bool isByte, bool isNS, bool isClean, string path)
        {
            this.myModel = model;
            makeController(maxSize, isByte, isNS, isClean, path);

            this.commands();
            return true;
        }

        /// <summary>
        /// Makes the controller.
        /// </summary>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
        private bool makeController(long maxSize, bool isByte, bool isNS, bool isClean, string path)
        {
            myController = new MVC_Controller(maxSize, isByte, isNS, isClean, path);
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
            timer.Interval = this.sekunde * 1000;
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
            Console.WriteLine("-D - delete storage");
            Console.WriteLine("-P - show storage");

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

                    Console.WriteLine(String.Format("┌─────┬────────────────────────────────────────────────────────────┬──────────┐"));
                    Console.WriteLine(String.Format("│{0, 5}│{1, -60}│{1, -10}│", "id", "url", "tip"));

                    Console.WriteLine(String.Format("├─────┼────────────────────────────────────────────────────────────┼──────────┤"));

                    foreach (KeyValuePair<string, string> link in links)
                    {
                        Console.WriteLine("│{0, 5}│{1, -60}│{2, -10}│", id, link.Key, myController.getType(link.Key));
                        id++;
                    }
                    Console.WriteLine(String.Format("└─────┴────────────────────────────────────────────────────────────┴──────────┘"));
                    break;
                case 'J':
                    string[] commands = command.Split(' ');
                    int _id;
                    if (commands.Length == 1)
                        break;
                    if (int.TryParse(commands[1], out _id))
                    {
                        string _url = myController.getURL(_id);
                        string type = myController.getType(_url);
                        if (type == "link/other")
                        {
                            myModel = myController.newPage(_id);
                            try
                            {
                                myController.storage.page.noUsed--;
                            }
                            catch 
                            {
                                
                                
                            }
                            
                            myModel.loadTime = DateTime.Now;
                            timer.Stop();
                            reloadController();
                        }
                        else if (type == "email")
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
                                            Console.WriteLine("Error!");
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
                case 'D':
                    myController.cleanStorage();
                    break;
                case 'P':
                    showStorage();
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

        private void showStorage()
        {
            int id = 1;
            Console.WriteLine(String.Format("┌───┬───────────────────────────────────┬───────┬────────┬───────────────────┐"));
                Console.WriteLine(String.Format("│{0, 3}│{1, -35}│{2, 7}│{3, -8}│{4, -19}│", "id", "local file", "noUsed", "Veličina", "Dodano"));

                Console.WriteLine(String.Format("├───┼───────────────────────────────────┼───────┼────────┼───────────────────┤"));
            foreach (Page page in myController.storage.Pages)
            {
                if(page.localStorageName.Length > 35)
                    Console.WriteLine("│{0, 3}│{1, -35}│{2, 7}│{3, -8}│{4, -19}│", id, page.localStorageName.Substring(0,35), page.noUsed, page.size, page.addedDateTime);
                else
                {
                    Console.WriteLine("│{0, 3}│{1, -35}│{2, 7}│{3, -8}│{4, -19}│", id, page.localStorageName, page.noUsed, page.size, page.addedDateTime);
                }
                id++;
            }
            Console.WriteLine(String.Format("└───┴───────────────────────────────────┴───────┴────────┴───────────────────┘"));
        }
    }
}
