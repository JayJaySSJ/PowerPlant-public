namespace PowerPlant.Domain.Models
{
    public class NewDataSet
    {
        public string PlantName { get; set; }
        public CauldronData[] CauldronsData { get; set; }
        public TurbineData[] TurbinesData { get; set; }
        public TransformatorData[] TransformatorsData { get; set; }
    }
}