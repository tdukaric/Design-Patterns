// ***********************************************************************
// Assembly         : tdukaric_zadaca_3
// Author           : Tomislav
// Created          : 01-07-2014
//
// Last Modified By : Tomislav
// Last Modified On : 01-10-2014
// ***********************************************************************
// <copyright file="MVC_Model.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using tdukaric_zadaca_3;

namespace tdukaric_zadaca_4
{
    /// <summary>
    /// Delegate handler
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The e.</param>
    delegate void handler(object sender, updateViewOnChange e);

    /// <summary>
    /// Class updateViewOnChange.
    /// </summary>
    class updateViewOnChange : EventArgs
    {
        /// <summary>
        /// The attached views
        /// </summary>
        public List<MVC_View> attachedViews;
        /// <summary>
        /// The attached controllers
        /// </summary>
        public List<MVC_Controller> attachedControllers;

        /// <summary>
        /// Initializes a new instance of the <see cref="updateViewOnChange"/> class.
        /// </summary>
        /// <param name="attachedViews">The attached views.</param>
        /// <param name="attachedControllers">The attached controllers.</param>
        public updateViewOnChange(List<MVC_View> attachedViews, List<MVC_Controller> attachedControllers)
        {
            this.attachedControllers = attachedControllers;
            this.attachedViews = attachedViews;
        }

    }

    /// <summary>
    /// Class Update.
    /// </summary>
    class Update
    {
        /// <summary>
        /// Updates the objects.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <param name="e">The updateViewOnChange.</param>
        public void UpdateObjects(object o, updateViewOnChange e)
        {
            foreach (MVC_Controller controller in e.attachedControllers)
            {
                controller.eventTriggered();
            }
            foreach (MVC_View view in e.attachedViews)
            {
                view.eventTriggered();
            }
        }
    }

    /// <summary>
    /// Class MVC_Model.
    /// </summary>
    class MVC_Model
    {
        public static event handler h;

        /// <summary>
        /// The attached views
        /// </summary>
        public List<MVC_View> attachedViews;
        /// <summary>
        /// The attached controllers
        /// </summary>
        public List<MVC_Controller> attachedControllers;
        
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
        /// The no changes
        /// </summary>
        public int noChanges;

        /// <summary>
        /// The URL
        /// </summary>
        public string url;

        public string local_url = null;
        /// <summary>
        /// The old_url
        /// </summary>
        public string old_url;

        public bool isLocal = false;

        /// <summary>
        /// The access time
        /// </summary>
        public DateTime accessTime;
        /// <summary>
        /// Initializes a new instance of the <see cref="MVC_Model"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        public MVC_Model(string url)
        {
            this.url = url;
            this.old_url = url;

            init();
        }

        public MVC_Model(Page page)
        {
            this.url = page.url;
            this.local_url = page.localStorageName;
            this.old_url = page.url;
            this.isLocal = true;

            init();
        }

        public MVC_Model(Page page, string old_url)
        {
            this.url = page.url;
            this.local_url = page.localStorageName;
            this.old_url = old_url;
            this.isLocal = true;

            init();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MVC_Model"/> class.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="old_url">The old_url.</param>
        public MVC_Model(string url, string old_url)
        {
            this.url = url;
            this.old_url = old_url;
            
            init();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void init()
        {
            HtmlWeb hw = new HtmlWeb();
            try
            {
                if(this.isLocal)
                    this.doc = hw.Load(this.local_url);
                else
                {
                    this.doc = hw.Load(this.url);
                }
            }
            catch
            {
                Console.WriteLine("Nepodržani dokument!");
            }
            this.loadTime = DateTime.Now;
            this.ReloadTimesManual = 0;
            this.ReloadTimesAuto = 0;
            this.VisitTime = 0;
            this.noChanges = -1;
            this.Links = new List<HtmlNode>();
            this.attachedControllers = new List<MVC_Controller>();
            this.attachedViews = new List<MVC_View>();
            updateLinks();
            this.accessTime = DateTime.Now;
            Update up = new Update();

            

        h += new handler(up.UpdateObjects);
        }

        /// <summary>
        /// Updates the links manual.
        /// </summary>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
        public bool updateLinksManual()
        {
            if (doc == null)
                return false;

            this.ReloadTimesManual++;

            return updateLinks();
        }

        /// <summary>
        /// Updates the links automatic.
        /// </summary>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
        public bool updateLinksAuto()
        {
            if (doc == null)
                return false;

            this.ReloadTimesAuto++;

            return updateLinks();
        }

        /// <summary>
        /// Updates the links.
        /// </summary>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
        private bool updateLinks()
        {

            HtmlWeb hw = new HtmlWeb();
            try
            {
                if (this.isLocal)
                    this.doc = hw.Load(this.local_url);
                else
                {
                    this.doc = hw.Load(this.url);
                }
            }
            catch
            {
                Console.WriteLine("Nepodržani dokument!");
            }

            

            HtmlNodeCollection col = doc.DocumentNode.SelectNodes("//a[@href]");

            List<HtmlNode> oldLinks = new List<HtmlNode>(this.Links);

            Links.Clear();

            if (col == null)
                return false;

            foreach (HtmlNode link in col)
            {
                if (link.Attributes["href"].Value.Length == 0)
                    continue;
                CompareInfo ci = CultureInfo.InvariantCulture.CompareInfo;
                bool x = ci.IsPrefix(link.Attributes["href"].Value, "http");
                bool y = ci.IsPrefix(link.Attributes["href"].Value, "mailto:");
                if(y)
                {
                    
                }
                else if (!x)
                {
                    if (link.Attributes["href"].Value[0] == '#')
                    {
                        link.Attributes["href"].Value = url;
                        continue;
                    }
                    if (ci.IsPrefix(link.Attributes["href"].Value, "javascript"))
                        continue;
                    if (ci.IsPrefix(link.Attributes["href"].Value, "mailto"))
                        continue;
                    if (link.Attributes["href"].Value[0] == '/')
                    {
                        Uri uri = new Uri(url);
                        
                        String _url;
                        if (uri.Port == 443)
                            _url = "https://";
                        else
                            _url = "http://";


                        _url += uri.Host;
                        _url += link.Attributes["href"].Value;

                        link.Attributes["href"].Value = _url;
                    }
                    else
                        link.Attributes["href"].Value = String.Concat(url + "/" + link.Attributes["href"].Value);
                }
                Links.Add(link);
            }
            bool diff = false;
            if (this.Links.Count == oldLinks.Count)
            {
                for (int i = 0; i < this.Links.Count; i++)
                    if (this.Links[i].Attributes["href"].Value != oldLinks[i].Attributes["href"].Value)
                    {
                        diff = true;
                    }
                        
            }
            else
                diff = true;
            if(diff)
            {
                this.noChanges++;
                if(this.attachedViews.Count != 0 && this.attachedControllers.Count != 0)
                    h(new object(), new updateViewOnChange(this.attachedViews, this.attachedControllers));
            }
            return true;
        }

        /// <summary>
        /// Attaches the controller.
        /// </summary>
        /// <param name="myController">My controller.</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
        public bool attachController(MVC_Controller myController)
        {
            try
            {
                this.attachedControllers.Add(myController);
            }
            catch (Exception)
            {
                return false;
            }
            
            return true;
        }
        /// <summary>
        /// Attaches the view.
        /// </summary>
        /// <param name="myView">My view.</param>
        /// <returns><c>true</c> if success, <c>false</c> otherwise.</returns>
        public bool attachView(MVC_View myView)
        {
            try
            {
                this.attachedViews.Add(myView);
            }
            catch (Exception)
            {
                
                return false;
            }
            
            return true;
        }

        /// <summary>
        /// Calculates the visit time.
        /// </summary>
        /// <param name="GoTime">The go time.</param>
        public void CalculateVisitTime(DateTime GoTime)
        {
            this.VisitTime += GoTime.Subtract(this.loadTime).Seconds;
        }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>System.String.</returns>
        public string getURL(int id)
        {
            return this.Links[id].Attributes["href"].Value;
        }
    }
}
