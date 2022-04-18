namespace PowerPlant.Domain.Models
{
    public class CauldronData
    {
        public string Name { get; set; }
        public AssetParameterData WaterPressure { get; set; } //ok 12
        public AssetParameterData WaterTemperature { get; set; } //ok 350
        public AssetParameterData CamberTemperature { get; set; } //ok 400
    }
}