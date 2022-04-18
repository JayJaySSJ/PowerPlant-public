using System.Runtime.Serialization;

namespace PowerPlant.Wcf.ServiceDefinition.Models
{
    [DataContract]
    public class TransformatorData
    {
        [DataMember]
        public string Name;

        [DataMember]
        public AssetParameterData InputVoltage;

        [DataMember]
        public AssetParameterData OutputVoltage;
    }
}