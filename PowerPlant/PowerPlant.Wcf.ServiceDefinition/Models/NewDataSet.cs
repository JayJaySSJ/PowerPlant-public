using System.Runtime.Serialization;

namespace PowerPlant.Wcf.ServiceDefinition.Models
{
    [DataContract]
    public class NewDataSet
    {
        [DataMember]
        public string PlantName;

        [DataMember]
        public CauldronData[] CauldronsData;

        [DataMember]
        public TurbineData[] TurbinesData;

        [DataMember]
        public TransformatorData[] TransformatorsData;
    }
}