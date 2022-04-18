namespace PowerPlant.WebApi.Client.Models
{
    public class TurbineData
    {
        public string Name { get; set; }
        public AssetParameterData OverheaterSteamTemperature { get; set; }
        public AssetParameterData SteamPressure { get; set; }
        public AssetParameterData RotationSpeed { get; set; }
        public AssetParameterData CurrentPower { get; set; }
        public AssetParameterData OutputVoltage { get; set; }
    }
}