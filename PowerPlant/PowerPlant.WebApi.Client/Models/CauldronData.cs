namespace PowerPlant.WebApi.Client.Models
{
    public class CauldronData
    {
        public string Name { get; set; }
        public AssetParameterData WaterPressure { get; set; }
        public AssetParameterData WaterTemperature { get; set; }
        public AssetParameterData CamberTemperature { get; set; }
    }
}