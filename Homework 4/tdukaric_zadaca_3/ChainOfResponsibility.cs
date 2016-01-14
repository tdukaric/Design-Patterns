// ***********************************************************************
// Assembly         : tdukaric_zadaca_3
// Author           : Tomislav
// Created          : 01-08-2014
//
// Last Modified By : Tomislav
// Last Modified On : 01-10-2014
// ***********************************************************************
// <copyright file="ChainOfResponsibility.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Globalization;

namespace tdukaric_zadaca_4
{
    /// <summary>
    /// The 'Handler' abstract class
    /// </summary>
    abstract class Handler
    {
        /// <summary>
        /// The successor
        /// </summary>
        protected Handler Successor;

        /// <summary>
        /// Sets the successor.
        /// </summary>
        /// <param name="successor">The successor.</param>
        public void SetSuccessor(Handler successor)
        {
            this.Successor = successor;
        }

        /// <summary>
        /// Handles the request.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="ext">The extension.</param>
        /// <returns>Type.</returns>
        public abstract string HandleRequest(string url, string ext);
    }

    /// <summary>
    /// Class ConcreteHandlerMail.
    /// </summary>
    class ConcreteHandlerMail : Handler
    {

        /// <summary>
        /// Handles the request.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="ext">The extension.</param>
        /// <returns>Type.</returns>
        public override string HandleRequest(string url, string ext)
        {
            CompareInfo ci = CultureInfo.InvariantCulture.CompareInfo;
            bool x = ci.IsPrefix(url, "mailto:");
            if (x)
            {
                return "email";
            }
            else if (Successor != null)
            {
                return Successor.HandleRequest(url, ext);
            }
            else
                return "other";
        }
    }

    /// <summary>
    /// Class ConcreteHandlerImage.
    /// </summary>
    class ConcreteHandlerImage : Handler
    {
        /// <summary>
        /// Enum exts
        /// </summary>
        private enum exts
        {
            /// <summary>
            /// The BMP
            /// </summary>
            bmp,
            /// <summary>
            /// The GIF
            /// </summary>
            gif,
            /// <summary>
            /// The JPG
            /// </summary>
            jpg,
            /// <summary>
            /// The PNG
            /// </summary>
            png,
            /// <summary>
            /// The tif
            /// </summary>
            tif,
            /// <summary>
            /// The PSD
            /// </summary>
            psd,
            /// <summary>
            /// The eps
            /// </summary>
            eps,
            /// <summary>
            /// The ps
            /// </summary>
            ps,
            /// <summary>
            /// The SVG
            /// </summary>
            svg
        }
        /// <summary>
        /// Handles the request.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="ext">The extension.</param>
        /// <returns>Type.</returns>
        public override string HandleRequest(string url, string ext)
        {
            if (Enum.IsDefined(typeof(exts), ext))
            {
                return "image";
            }
            else if (Successor != null)
            {
                return Successor.HandleRequest(url, ext);
            }
            else
                return "other";
        }
    }

    /// <summary>
    /// Class ConcreteHandlerVideo.
    /// </summary>
    class ConcreteHandlerVideo : Handler
    {

        /// <summary>
        /// Enum exts
        /// </summary>
    private enum exts
        {
            /// <summary>
            /// The avi
            /// </summary>
            avi,
            /// <summary>
            /// The FLV
            /// </summary>
            flv,
            /// <summary>
            /// The mov
            /// </summary>
            mov,
            /// <summary>
            /// The MPG
            /// </summary>
            mpg,
            /// <summary>
            /// The SWF
            /// </summary>
            swf,
            /// <summary>
            /// The WMV
            /// </summary>
            wmv,
            /// <summary>
            /// The MKV
            /// </summary>
            mkv
        }
    /// <summary>
    /// Handles the request.
    /// </summary>
    /// <param name="url">The URL.</param>
    /// <param name="ext">The extension.</param>
    /// <returns>Type.</returns>
    public override string HandleRequest(string url, string ext)
        {
            if (Enum.IsDefined(typeof(exts), ext))
            {
                return "video";
            }
            else if (Successor != null)
            {
                return Successor.HandleRequest(url,  ext);
            }
            else
                return "other";
        }
    }

    /// <summary>
    /// Class ConcreteHandlerDocument.
    /// </summary>
    class ConcreteHandlerDocument : Handler
    {
        /// <summary>
        /// Enum exts
        /// </summary>
        private enum exts
        {
            /// <summary>
            /// The document
            /// </summary>
            doc,
            /// <summary>
            /// The docx
            /// </summary>
            docx,
            /// <summary>
            /// The PDF
            /// </summary>
            pdf,
            /// <summary>
            /// The odt
            /// </summary>
            odt,
            /// <summary>
            /// The pdfa
            /// </summary>
            pdfa,
            /// <summary>
            /// The pages
            /// </summary>
            pages,
            /// <summary>
            /// The text
            /// </summary>
            txt,
            /// <summary>
            /// The RTF
            /// </summary>
            rtf
        }
        /// <summary>
        /// Handles the request.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="ext">The extension.</param>
        /// <returns>Type.</returns>
        public override string HandleRequest(string url, string ext)
        {
            if (Enum.IsDefined(typeof(exts), ext))
            {
                return "document";
            }
            else if (Successor != null)
            {
                return Successor.HandleRequest(url, ext);
            }
            else
                return "other";
        }
    }

    /// <summary>
    /// Class ConcreteHandlerOther.
    /// </summary>
    class ConcreteHandlerOther : Handler
    {
        /// <summary>
        /// Handles the request.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="ext">The extension.</param>
        /// <returns>Type.</returns>
        public override string HandleRequest(string url, string ext)
        {
            return "link/other";
        }
    }

    /// <summary>
    /// Class COR.
    /// </summary>
    class COR
    {
        /// <summary>
        /// The handle
        /// </summary>
        public Handler handle;
        /// <summary>
        /// Initializes a new instance of the <see cref="COR"/> class.
        /// </summary>
        public COR()
        {
            Handler imageHandler = new ConcreteHandlerImage();
            Handler videoHandler = new ConcreteHandlerVideo();
            Handler documentHandler = new ConcreteHandlerDocument();
            Handler otherHandler = new ConcreteHandlerOther();
            Handler mailHandler = new ConcreteHandlerMail();

            mailHandler.SetSuccessor(documentHandler);
            documentHandler.SetSuccessor(imageHandler);
            imageHandler.SetSuccessor(videoHandler);
            videoHandler.SetSuccessor(otherHandler);

            handle = mailHandler;
            
        }
    }
}
