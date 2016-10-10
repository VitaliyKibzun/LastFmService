using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms.VisualStyles;
using System.Xml;
using System.Xml.Linq;

namespace LastFmService
{
    public class GetAPI
    {
        public string apiRootUrl = "http://ws.audioscrobbler.com/2.0/";
        public string apiKey = "94f58634646022ee71007e44f48b7fd4";
        public string apiMethod = "tag.gettoptracks";
        public string apiGenre = "disco";

        public void callApi()
        {
            StreamReader streamReader;
            string fullApiUrl =
                String.Format("{2}?method={1}&tag={3}&api_key={0}", apiKey, apiMethod, apiRootUrl, apiGenre);
            
            XDocument xdoc = XDocument.Load(fullApiUrl);
            List<string> chart = new List<string>();
            var tracks = from tr in xdoc.Descendants().Elements("track") select tr;
            
            foreach (XElement trackElement in tracks)
            {
                var trackRank = trackElement.Attribute("rank").Value;
                var trackName = trackElement.Element("name").Value;
                var artistName = trackElement.Element("artist").Element("name").Value;

                string row = String.Format("{0} - {1} - {2}", trackRank, artistName, trackName);
                Debugger.Log(1, "", row + "\n");
                chart.Add(row);
            }
            //xdoc.Descendants("track").Select(p => new
            //{
            //    rank = p.Element("track").Attribute("rank").Value
            //}).ToList().ForEach(p => { Console.WriteLine("track = " + p.rank); });
            Console.ReadKey();

        }
        
    }
}