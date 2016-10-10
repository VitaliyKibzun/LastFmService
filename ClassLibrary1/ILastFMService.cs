using System.Collections.Generic;
using System.ServiceModel;

namespace LastFmApi
{
    [ServiceContract]
    public interface ILastFMService
    {
        [OperationContract]
        List<string> GetTopTracks(string genre);

        [OperationContract]
        List<string> GetTopTags();
    }

    
}
