using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using LastFmApi;

namespace LastFmApi
{
    public class LastFMService : ILastFMService
    {
        public string apiRootUrl = "http://ws.audioscrobbler.com/2.0/";
        public string apiKey = "94f58634646022ee71007e44f48b7fd4";

        Dictionary<string, Tuple<DateTime, List<string>>> cacheTopTracks =
            new Dictionary<string, Tuple<DateTime, List<string>>>();

        public List<string> GetTopTracks(string apiTag)
        {
            if (!UseCache(apiTag))
            {
                string apiMethod = "tag.gettoptracks";
                string fullApiUrl =
                    String.Format("{0}?method={1}&tag={2}&api_key={3}", apiRootUrl, apiMethod, apiTag, apiKey);
                // full ApiUrl example: http://ws.audioscrobbler.com/2.0/?method=tag.gettoptracks&tag=rock&api_key=94f58634646022ee71007e44f48b7fd4
                XDocument xdoc;
                try
                {
                    xdoc = XDocument.Load(fullApiUrl);
                }
                catch (Exception messageException)
                {
                    throw messageException;
                }

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

                Tuple<DateTime, List<string>> tuple = new Tuple<DateTime, List<string>>(DateTime.Now, chart);
                cacheTopTracks.Add(apiTag, tuple);
                
                return chart;
            }

            else
            {
                return cacheTopTracks[apiTag].Item2;
            }
            
        }

        private bool UseCache(string apiTag)
        {
            if (!cacheTopTracks.ContainsKey(apiTag))
            {
                return false;
            }

            if (DateTime.UtcNow.DayOfYear > cacheTopTracks[apiTag].Item1.DayOfYear)
            {
                cacheTopTracks.Remove(apiTag);
                return false;
            }

            return true;
        }

        public List<string> GetTopTags()
        {
            string apiMethod = "chart.gettoptags";
            string fullApiUrl =
                String.Format("{0}?method={1}&api_key={2}", apiRootUrl, apiMethod, apiKey );
            // full ApiUrl example: http://ws.audioscrobbler.com/2.0/?method=tag.getTopTags&api_key=94f58634646022ee71007e44f48b7fd4
            XDocument xdoc;
            try
            {
                xdoc = XDocument.Load(fullApiUrl);
            }
            catch (Exception messageException)
            {
                throw messageException;
            }
            List<string> tagsList = new List<string>();
            var tags = from tag in xdoc.Descendants().Elements("tag") select tag;

            foreach (var tagElement in tags)
            {
                var tagName = tagElement.Element("name").Value;
                tagsList.Add(tagName);
            }

            return tagsList;
        }
    }
}
