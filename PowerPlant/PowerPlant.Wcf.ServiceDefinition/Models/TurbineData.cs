using System.Runtime.Serialization;

namespace PowerPlant.Wcf.ServiceDefinition.Models
{
    [DataContract]
    public class TurbineData
    {
        [DataMember]
        public string Name;

        [DataMember]
        public AssetParameterData OverheaterSteamTemperature;

        [DataMember]
        public AssetParameterData SteamPressure;

        [DataMember]
        public AssetParameterData RotationSpeed;

        [DataMember]
        public AssetParameterData CurrentPower;

        [DataMember]
        public AssetParameterData OutputVoltage;
    }
}