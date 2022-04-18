namespace PowerPlant.Domain.Models
{
    public class TransformatorData
    {
        public string Name { get; set; }
        public AssetParameterData InputVoltage { get; set; } //14tys
        public AssetParameterData OutputVoltage { get; set; } //110tys
    }
}