using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.AccessControl;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using HtmlAgilityPack;

namespace tdukaric_zadaca_3
{
    [Serializable()]
    public class Page
    {
        public DateTime addedDateTime;
        public int noUsed;
        public DateTime lastUsedDateTime;
        public string url;
        public string localStorageName;
        public long size;
        
    }

    [Serializable()]
    public class Storage : ISerializable
    {
        public List<Page> Pages;
        public Page page;
        
        public long Size;
        public long MaxSize;
        public long MaxPageNum;
        public bool isNS;
        public string path;

        public Storage()
        {
            
        }

        

        public Storage(long size, bool isByte, bool isNS, string path)
        {
            this.Pages = new List<Page>();
            if (isByte)
            {
                this.MaxSize = size;
                this.MaxPageNum = 99999999999999999;
            }
            else
            {
                this.MaxSize = 99999999999999999;
                this.MaxPageNum = size;
            }
            this.isNS = isNS;
            this.path = path;
        }

        public void NewPage(string url)
        {
            Page page = new Page
            {
                url = url,
                addedDateTime = DateTime.Now,
                lastUsedDateTime = DateTime.Now,
                noUsed = 0
            };
            StringBuilder storageNameBuilder = new StringBuilder();
            storageNameBuilder.Append(path);
            Uri uri = new Uri(url);
            
            string filename = System.IO.Path.GetFileName(uri.LocalPath);
            storageNameBuilder.Append("\\page_");
            storageNameBuilder.Append(uri.Host);
            storageNameBuilder.Append("_");
            storageNameBuilder.Append(System.IO.Path.GetFileName(uri.LocalPath));
            page.localStorageName = storageNameBuilder.ToString();

            HtmlWeb hw = new HtmlWeb();

            HtmlDocument document = hw.Load(url);
            FileStream fs = File.OpenWrite(page.localStorageName);
            StreamWriter writer = new StreamWriter(fs, Encoding.UTF8);
            
            document.Save(writer);
            
            this.Size += fs.Length;
            page.size = fs.Length;
            
            writer.Close();
            fs.Close();

            
            while((this.Pages != null) && ((MaxSize < this.Size) || (MaxPageNum < this.Pages.Count)) && (this.Pages.Count >= 1))
            if (this.isNS)
            {
                this.Pages = this.Pages.OrderByDescending(x => x.addedDateTime).ToList();
                this.Size -= this.Pages[0].size;
                File.Delete(this.Pages[0].localStorageName);
                DnevnikRada.add("Deleted page " + this.Pages[0].url + " in " + DateTime.Now.ToString(CultureInfo.InvariantCulture) + ". Used " + this.Pages[0].noUsed + " times, loaded " + this.Pages[0].addedDateTime + ". Last time loaded at: " + this.Pages[0].lastUsedDateTime);
                Console.WriteLine("Deleted page " + this.Pages[0].url + " in " + DateTime.Now.ToString(CultureInfo.InvariantCulture) + ". Used " + this.Pages[0].noUsed + " times, loaded " + this.Pages[0].addedDateTime + ". Last time loaded at: " + this.Pages[0].lastUsedDateTime);
                this.Pages.Remove(this.Pages[0]);
            }
            else
            {
                this.Pages = this.Pages.OrderByDescending(x => x.noUsed).ToList();
                this.Size--;
                File.Delete(this.Pages[0].localStorageName);
                DnevnikRada.add("Deleted page " + this.Pages[0].url + " in " + DateTime.Now.ToString(CultureInfo.InvariantCulture) + ". Used " + this.Pages[0].noUsed + " times during " + DateTime.Now.Subtract(this.Pages[0].addedDateTime).Seconds.ToString(CultureInfo.InvariantCulture) + " seconds, loaded " + this.Pages[0].addedDateTime + ". Last time loaded at: " + this.Pages[0].lastUsedDateTime);
                Console.WriteLine("Deleted page " + this.Pages[0].url + " in " + DateTime.Now.ToString(CultureInfo.InvariantCulture) + ". Used " + this.Pages[0].noUsed + " times, loaded " + this.Pages[0].addedDateTime + ". Last time loaded at: " + this.Pages[0].lastUsedDateTime);
                this.Pages.Remove(this.Pages[0]);
            }
            
            this.Pages.Add(page);
            this.page = page;

            DnevnikRada.add("Loaded page " + url + " u " + DateTime.Now.ToString(CultureInfo.InvariantCulture));
        }

        public Page pageExist(string url)
        {
            foreach (Page page in Pages)
            {
                if (page.url == url)
                    return page;
            }
            return null;
        }

        public void Use(string url)
        {
            Page page = GetPage(url);
            page.lastUsedDateTime = DateTime.Now;
            page.noUsed++;
            this.page = page;

        }

        public Page GetPage(string url)
        {
            if (Pages == null)
                return null;
            foreach(Page page in Pages)
            {
                if (page.url == url)
                {
                    this.page = page;
                    page.noUsed++;
                    page.lastUsedDateTime = DateTime.Now;
                    return page;
                }
            }
            this.page = null;
            return null;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Pages", Pages, typeof(List<Page>));
            info.AddValue("page", page, typeof(Page));
            info.AddValue("Size", Size, typeof(long));
            info.AddValue("MaxSize", MaxSize, typeof(long));
            info.AddValue("MaxPageNum", MaxPageNum, typeof(long));
            info.AddValue("isNS", isNS, typeof(bool));
            info.AddValue("path", path, typeof(string));
            
        }

        public Storage(SerializationInfo info, StreamingContext ctxt)
        {
            this.Pages = (List<Page>) info.GetValue("Pages", typeof (List<Page>));
            this.page = (Page) info.GetValue("page", typeof (Page));
            this.Size = (long) info.GetValue("Size", typeof (long));
            this.MaxSize = (long) info.GetValue("MaxSize", typeof (long));
            this.MaxPageNum = (long) info.GetValue("MaxPageNum", typeof (long));
            this.isNS = (bool) info.GetValue("isNS", typeof (bool));
            this.path = (string) info.GetValue("path", typeof (string));

        }
    }

    public static class storage
    {
        public static Storage LoadStorage()
        {
            Storage storage = new Storage();
            XmlDocument xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.Load("spremiste.dat");
            }
            catch (Exception)
            {
                Console.WriteLine("Storage file doesn't exist!");
                Console.WriteLine("Creating a new storage...");
            }
            
            string xmlString = xmlDocument.OuterXml;

            using (StringReader read = new StringReader(xmlString))
            {
                Type outType = typeof(Storage);

                XmlSerializer serializer = new XmlSerializer(outType);
                using (XmlReader reader = new XmlTextReader(read))
                {
                    storage = (Storage)serializer.Deserialize(reader);
                    reader.Close();
                }

                read.Close();
            }
            return storage;
        }

        public static bool SaveStorage(Storage storage)
        {
            try
            {

                string attributeXml = string.Empty;
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(storage.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, storage);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save("spremiste.dat");
                    stream.Close();
                    return true;
                }

            }
            catch
            {
                Console.WriteLine("Can't create the storage file!");
                return false;
            }
        }
    }
}
