namespace PowerPlant.WebApi.Client.Models
{
    public class TransformatorData
    {
        public string Name { get; set; }
        public AssetParameterData InputVoltage { get; set; }
        public AssetParameterData OutputVoltage { get; set; }
    }
}