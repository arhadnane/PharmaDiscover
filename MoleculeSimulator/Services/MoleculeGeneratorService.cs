using MoleculeSimulator.Models;

namespace MoleculeSimulator.Services
{
    public interface IMoleculeGeneratorService
    {
        List<Molecule> GenerateMolecules(MoleculeGenerationRequest request);
        Molecule GenerateSingleMolecule(MoleculeGenerationRequest? request = null);
    }

    public class MoleculeGeneratorService : IMoleculeGeneratorService
    {
        private readonly Random _random;
        private readonly IScoringService _scoringService;
        private readonly string[] _prefixes = { "Mol", "Comp", "Drug", "Chem", "Bio", "Pharma", "Med", "Syn" };
        private readonly string[] _suffixes = { "ine", "ol", "ate", "ide", "ase", "cin", "nol", "phen" };

        public MoleculeGeneratorService(IScoringService scoringService)
        {
            _random = new Random();
            _scoringService = scoringService;
        }

        public List<Molecule> GenerateMolecules(MoleculeGenerationRequest request)
        {
            var molecules = new List<Molecule>();
            
            for (int i = 0; i < request.Count; i++)
            {
                molecules.Add(GenerateSingleMolecule(request));
            }

            return molecules;
        }

        public Molecule GenerateSingleMolecule(MoleculeGenerationRequest? request = null)
        {
            request ??= new MoleculeGenerationRequest();

            var molecule = new Molecule
            {
                Name = GenerateMoleculeName(),
                MolecularWeight = GenerateRandomDouble(request.MinMolecularWeight, request.MaxMolecularWeight),
                LogP = GenerateRandomDouble(request.MinLogP, request.MaxLogP),
                HydrogenBondDonors = _random.Next(0, 11),
                HydrogenBondAcceptors = _random.Next(0, 11),
                Polarity = GenerateRandomDouble(0.0, 1.0)
            };

            // Generate atom counts based on molecular weight (simplified approximation)
            var totalAtoms = (int)(molecule.MolecularWeight / 12); // Rough approximation
            molecule.CarbonAtoms = (int)(totalAtoms * 0.4 + _random.Next(-5, 6)); // ~40% carbon
            molecule.HydrogenAtoms = (int)(totalAtoms * 0.45 + _random.Next(-5, 6)); // ~45% hydrogen
            molecule.OxygenAtoms = (int)(totalAtoms * 0.1 + _random.Next(-2, 3)); // ~10% oxygen
            molecule.NitrogenAtoms = (int)(totalAtoms * 0.05 + _random.Next(-1, 2)); // ~5% nitrogen

            // Ensure non-negative values
            molecule.CarbonAtoms = Math.Max(1, molecule.CarbonAtoms);
            molecule.HydrogenAtoms = Math.Max(1, molecule.HydrogenAtoms);
            molecule.OxygenAtoms = Math.Max(0, molecule.OxygenAtoms);
            molecule.NitrogenAtoms = Math.Max(0, molecule.NitrogenAtoms);

            // Calculate therapeutic score
            molecule.TherapeuticScore = _scoringService.CalculateTherapeuticScore(molecule);

            return molecule;
        }

        private string GenerateMoleculeName()
        {
            var prefix = _prefixes[_random.Next(_prefixes.Length)];
            var suffix = _suffixes[_random.Next(_suffixes.Length)];
            var number = _random.Next(1, 9999);
            
            return $"{prefix}-{number}-{suffix}";
        }

        private double GenerateRandomDouble(double min, double max)
        {
            return Math.Round(min + (_random.NextDouble() * (max - min)), 2);
        }
    }
}
