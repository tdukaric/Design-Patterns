// ***********************************************************************
// Assembly         : tdukaric_zadaca_3
// Author           : Tomislav
// Created          : 01-07-2014
//
// Last Modified By : Tomislav
// Last Modified On : 01-09-2014
// ***********************************************************************
// <copyright file="Memento.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using HtmlAgilityPack;
using System;
using System.Collections.Generic;

namespace tdukaric_zadaca_3
{
    /// <summary>
    /// Class Memento.
    /// </summary>
    class Memento
    {
        /// <summary>
        /// Gets or sets the links.
        /// </summary>
        /// <value>The links.</value>
        public List<HtmlNode> Links { get; set; }
        /// <summary>
        /// Gets or sets the document.
        /// </summary>
        /// <value>The document.</value>
        public HtmlDocument doc { get; set; }
        /// <summary>
        /// The load time
        /// </summary>
        public DateTime loadTime;
        /// <summary>
        /// The visit time
        /// </summary>
        public int VisitTime;
        /// <summary>
        /// The reload times manual
        /// </summary>
        public int ReloadTimesManual;
        /// <summary>
        /// The reload times automatic
        /// </summary>
        public int ReloadTimesAuto;
        /// <summary>
        /// The no change
        /// </summary>
        public int noChange;

        /// <summary>
        /// The URL
        /// </summary>
        public string url;
        /// <summary>
        /// The old_url
        /// </summary>
        public string old_url;

        /// <summary>
        /// The access time
        /// </summary>
        public DateTime accessTime;

        public long size;

        /// <summary>
        /// Saves the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        public void Save(MVC_Model model)
        {
            this.Links = new List<HtmlNode>();
            this.doc = new HtmlDocument();
            this.loadTime = new DateTime();
            this.accessTime = new DateTime();

            this.Links = model.Links;
            this.doc = model.doc;
            this.loadTime = model.loadTime;
            this.VisitTime = model.VisitTime;
            this.url = model.url;
            this.accessTime = model.accessTime;
            this.ReloadTimesManual = model.ReloadTimesManual;
            this.ReloadTimesAuto = model.ReloadTimesAuto;
            this.old_url = model.old_url;
            this.noChange = model.noChanges;
            this.size = model.size;
        }
    }

    /// <summary>
    /// Class Caretaker.
    /// </summary>
    [Serializable]
    class Caretaker
    {
        /// <summary>
        /// The mementos
        /// </summary>
        public List<Memento> Mementos;
        /// <summary>
        /// Initializes a new instance of the <see cref="Caretaker"/> class.
        /// </summary>
        public Caretaker()
        {
            this.Mementos = new List<Memento>();
        }

        /// <summary>
        /// Adds the memento.
        /// </summary>
        /// <param name="memento">The memento.</param>
        public void addMemento(Memento memento)
        {
            Memento temp = null;
            foreach (Memento temp2 in Mementos)
            {
                if (temp2.url == memento.url)
                {
                    temp = temp2;
                    break;
                }
            }

            if (temp != null)
                Mementos.Remove(temp);
            
            this.Mementos.Add(memento);
        }

        /// <summary>
        /// Gets the memento.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>MVC_Model.</returns>
        public MVC_Model getMemento(int id)
        {
            MVC_Model model = new MVC_Model(Mementos[id].url, Mementos[id].old_url);
            model.ReloadTimesManual = Mementos[id].ReloadTimesManual;
            model.ReloadTimesAuto = Mementos[id].ReloadTimesAuto;
            model.VisitTime = Mementos[id].VisitTime;
            return model;
        }

    }

}
