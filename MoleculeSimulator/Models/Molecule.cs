using System.ComponentModel.DataAnnotations;

namespace MoleculeSimulator.Models
{
    public class Molecule
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Range(50, 1000)]
        public double MolecularWeight { get; set; }
        
        [Range(-5.0, 10.0)]
        public double LogP { get; set; }
        
        [Range(0, 20)]
        public int HydrogenBondDonors { get; set; }
        
        [Range(0, 20)]
        public int HydrogenBondAcceptors { get; set; }
        
        [Range(0, 500)]
        public int HydrogenAtoms { get; set; }
        
        [Range(0, 100)]
        public int CarbonAtoms { get; set; }
        
        [Range(0, 50)]
        public int NitrogenAtoms { get; set; }
        
        [Range(0, 50)]
        public int OxygenAtoms { get; set; }
        
        [Range(0.0, 1.0)]
        public double Polarity { get; set; }
        
        public double TherapeuticScore { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Calculated property for total atoms
        public int TotalAtoms => HydrogenAtoms + CarbonAtoms + NitrogenAtoms + OxygenAtoms;
        
        // Therapeutic potential category
        public string TherapeuticCategory
        {
            get
            {
                return TherapeuticScore switch
                {
                    >= 80 => "Excellent",
                    >= 60 => "Good",
                    >= 40 => "Moderate",
                    >= 20 => "Poor",
                    _ => "Very Poor"
                };
            }
        }
    }
}
