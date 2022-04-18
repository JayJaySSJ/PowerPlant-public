namespace PowerPlant.Domain.Models
{
    public class TurbineData
    {
        public string Name { get; set; }
        public AssetParameterData OverheaterSteamTemperature { get; set; } //ok 550 stC
        public AssetParameterData SteamPressure { get; set; } //90Mpa
        public AssetParameterData RotationSpeed { get; set; } //ok 3000RPM
        public AssetParameterData CurrentPower { get; set; } //100MW
        public AssetParameterData OutputVoltage { get; set; } //Ok 14tys
    }
}