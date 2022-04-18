using System.Runtime.Serialization;

namespace PowerPlant.Wcf.ServiceDefinition.Models
{
    [DataContract]
    public class CauldronData
    {
        [DataMember]
        public string Name;

        [DataMember]
        public AssetParameterData WaterPressure;

        [DataMember]
        public AssetParameterData WaterTemperature;

        [DataMember]
        public AssetParameterData CamberTemperature;
    }
}