namespace MoleculeSimulator.Models
{
    public class MoleculeGenerationRequest
    {
        public int Count { get; set; } = 10;
        public double MinMolecularWeight { get; set; } = 200;
        public double MaxMolecularWeight { get; set; } = 800;
        public double MinLogP { get; set; } = -2.0;
        public double MaxLogP { get; set; } = 5.0;
        public bool SaveToDatabase { get; set; } = false;
    }
}
