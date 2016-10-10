using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml.Linq;

namespace LastFMServiceWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ServiceGetTracks" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ServiceGetTracks.svc or ServiceGetTracks.svc.cs at the Solution Explorer and start debugging.
    public class ServiceGetTracks : IServiceGetTracks
    {
/*
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
*/
        public string apiRootUrl = "http://ws.audioscrobbler.com/2.0/";
        public string apiKey = "94f58634646022ee71007e44f48b7fd4";
        public string apiMethod = "tag.gettoptracks";

        public List<string> GetTopTracks(string apiGenre)
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

            return chart;
        }
    }
}
