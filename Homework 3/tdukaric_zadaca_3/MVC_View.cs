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
        int sekunde;

        /// <summary>
        /// Initializes a new instance of the <see cref="MVC_View"/> class.
        /// </summary>
        /// <param name="sekunde">The sekunde.</param>
        public MVC_View(int sekunde)
        {
            this.sekunde = sekunde;
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
            timer.Interval = this.sekunde * 1000;
            timer.Start();

            return true;
        }

        /// <summary>
        /// Commandses this instance.
        /// </summary>
        public void commands()
        {
            Console.WriteLine("Komande:");
            Console.WriteLine("-B - ispis ukupnog broja poveznica");
            Console.WriteLine("-I - ispis adresa poveznica s rednim brojem");
            Console.WriteLine("-J n - prijelaz na poveznicu s rednim brojem n");
            Console.WriteLine("-R - obnovi važeću web stranicu");
            Console.WriteLine("-S - ispis statistike rada");
            Console.WriteLine("-U - ispis trenutnog URL-a");

            Console.WriteLine("-A - povratak na prethodnu stranicu");

            Console.WriteLine("-Q - prekid rada programa");
            string command = Console.ReadLine();
            if (command.Length < 2)
                this.commands();
            switch (command[1])
            {
                case 'B':
                    Console.WriteLine("Broj poveznica: " + myController.numLinks());
                    break;
                case 'I':
                    List<KeyValuePair<string, string>> links = myController.getURLs();
                    int id = 0;
                    foreach (KeyValuePair<string, string> link in links)
                    {
                        Console.WriteLine("ID: " + id + "\tURL: " + link.Key);
                        Console.WriteLine("Tip: " + myController.getType(link.Key));
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
                                    Console.WriteLine("Naziv: " + Path.GetFileName(_url));
                                    Console.WriteLine("Tip: " + response.ContentType);
                                    Console.WriteLine("Veličina: " + response.ContentLength);
                                    Console.WriteLine("Otvoriti datoteku? (y za da) [ne]");
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
                        Console.WriteLine("Greška kod parsiranja broja!");
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
                    Console.WriteLine("Prethodno otvorene stranice: ");
                    myController.showStatistics();
                    Console.WriteLine("Trenutno otvorena stranica: " + this.myModel.url);
                    Console.WriteLine("Vrijeme zadržavanja: " + (myModel.VisitTime + DateTime.Now.Subtract(myModel.loadTime).Seconds));
                    Console.WriteLine("Broj ručnih osvježavanja: " + myModel.ReloadTimesManual);
                    Console.WriteLine("Broj automatskih osvježavanja: " + myModel.ReloadTimesAuto);
                    Console.WriteLine("Broj promjena na stranici: " + myModel.noChanges);

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
            Console.WriteLine("Došlo je do promjene u sadržaju stranice!");
            return;
        }
    }
}
