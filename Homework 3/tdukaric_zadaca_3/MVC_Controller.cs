// ***********************************************************************
// Assembly         : tdukaric_zadaca_3
// Author           : Tomislav
// Created          : 01-07-2014
//
// Last Modified By : Tomislav
// Last Modified On : 01-10-2014
// ***********************************************************************
// <copyright file="MVC_Controller.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Timers;

namespace tdukaric_zadaca_3
{
    /// <summary>
    /// Class MVC_Controller.
    /// </summary>
    class MVC_Controller
    {
        /// <summary>
        /// My model
        /// </summary>
        MVC_Model myModel;
        /// <summary>
        /// My view
        /// </summary>
        MVC_View myView;
        /// <summary>
        /// The caretaker
        /// </summary>
        Caretaker caretaker;
        /// <summary>
        /// The chain of responsibility
        /// </summary>
        COR cor;

        /// <summary>
        /// Initializes a new instance of the <see cref="MVC_Controller"/> class.
        /// </summary>
        public MVC_Controller()
        {
            this.caretaker = new Caretaker();
            this.cor = new COR();
        }
        /// <summary>
        /// Initializes the specified my model.
        /// </summary>
        /// <param name="myModel">My model.</param>
        /// <param name="myView">My view.</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
        public bool initialize(MVC_Model myModel, MVC_View myView)
        {

            this.myModel = myModel;
            this.myView = myView;

            return true;
        }

        /// <summary>
        /// Events the triggered.
        /// </summary>
        public void eventTriggered()
        {

        }

        /// <summary>
        /// Numbers the links.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int numLinks()
        {
            return myModel.Links.Count;
        }

        /// <summary>
        /// Gets the urls.
        /// </summary>
        /// <returns>List{KeyValuePair{System.StringSystem.String}}.</returns>
        public List<KeyValuePair<string, string>> getURLs()
        {
            List<KeyValuePair<string, string>> temp = new List<KeyValuePair<string, string>>();
            foreach (HtmlNode node in myModel.Links)
            {
                temp.Add(new KeyValuePair<string, string>(node.Attributes["href"].Value, node.InnerText));
            }
            return temp;
        }

        /// <summary>
        /// Saves the memento.
        /// </summary>
        public void saveMemento()
        {
            myModel.CalculateVisitTime(DateTime.Now);
            Memento memento = new Memento();
            memento.Save(myModel);
            caretaker.addMemento(memento);
        }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>System.String.</returns>
        public string getURL(int id)
        {
            if (myModel.Links.Count < (id + 1) || id < 0)
                return null;
            return myModel.getURL(id);
        }

        /// <summary>
        /// Checks if exist.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>System.Int32.</returns>
        public int checkIfExist(string url)
        {
            int id = 0;
            foreach (Memento temp in caretaker.Mementos)
            {
                if (temp.url == url)
                    return id;
                id++;
            }

            return 0;
        }

        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>MVC_Model.</returns>
        public MVC_Model getModel(int id)
        {
            return caretaker.getMemento(id);
        }

        /// <summary>
        /// News the page.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>MVC_Model.</returns>
        public MVC_Model newPage(int id)
        {
            this.saveMemento();
            string old_url = myModel.url;
            string url = this.getURL(id);
            if (url == null)
            {
                Console.WriteLine("Greška! Nema te oznake!");
                return myModel;
            }
            int exist = this.checkIfExist(url);
            if (exist == 0)
                myModel = new MVC_Model(url, old_url);
            else
                myModel = this.getModel(exist);

            return myModel;
        }

        /// <summary>
        /// Reloads the links automatic.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="e">The <see cref="ElapsedEventArgs"/> instance containing the event data.</param>
        public void reloadLinksAuto(object source, ElapsedEventArgs e)
        {
            //Console.Write("{0}", (char)1);
            myModel.updateLinksAuto();
        }

        /// <summary>
        /// Reloads the links manual.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="e">The <see cref="ElapsedEventArgs"/> instance containing the event data.</param>
        public void reloadLinksManual(object source, ElapsedEventArgs e)
        {
            //Console.Write("{0}", (char)1);
            myModel.updateLinksManual();
        }

        /// <summary>
        /// Goes the back.
        /// </summary>
        /// <returns>MVC_Model.</returns>
        public MVC_Model goBack()
        {
            if (this.caretaker.Mementos.Count == 0)
                return myModel;

            this.saveMemento();

            myModel = this.getModel(this.caretaker.Mementos.Count - 2);

            return myModel;
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns>System.String.</returns>
        public string getType(string url)
        {
            string r = "link/other";
            try
            {
                Uri u = new Uri(url);
                if (url.StartsWith("mailto"))
                    r = cor.handle.HandleRequest(url, u.LocalPath);
                else if (Path.GetExtension(u.LocalPath).Length == 0)
                    return r;
                else r = cor.handle.HandleRequest(url, Path.GetExtension(u.LocalPath).Remove(0, 1));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return r;
        }

        /// <summary>
        /// Shows the statistics.
        /// </summary>
        public void showStatistics()
        {
            foreach (Memento temp in caretaker.Mementos)
            {
                Console.WriteLine("URL: " + temp.url);
                Console.WriteLine("Vrijeme zadržavanja: " + temp.VisitTime);
                Console.WriteLine("Broj ručnih osvježavanja: " + temp.ReloadTimesManual);
                Console.WriteLine("Broj automatskih osvježavanja: " + temp.ReloadTimesAuto);
                Console.WriteLine("Broj promjena na stranici: " + temp.noChange);
                Console.WriteLine();
            }
        }
    }
}
