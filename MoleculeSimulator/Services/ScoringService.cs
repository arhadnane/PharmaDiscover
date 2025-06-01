using MoleculeSimulator.Models;

namespace MoleculeSimulator.Services
{
    public interface IScoringService
    {
        double CalculateTherapeuticScore(Molecule molecule);
        Dictionary<string, double> GetScoreBreakdown(Molecule molecule);
    }

    public class ScoringService : IScoringService
    {
        private readonly Random _random;

        public ScoringService()
        {
            _random = new Random();
        }

        public double CalculateTherapeuticScore(Molecule molecule)
        {
            // Simplified drug-likeness scoring based on Lipinski's Rule of Five and other factors
            double score = 100.0; // Start with perfect score

            // Molecular weight penalty (optimal range: 300-500 Da)
            if (molecule.MolecularWeight < 300)
                score -= Math.Abs(300 - molecule.MolecularWeight) * 0.1;
            else if (molecule.MolecularWeight > 500)
                score -= (molecule.MolecularWeight - 500) * 0.15;

            // LogP penalty (optimal range: 1-3)
            if (molecule.LogP < 1)
                score -= Math.Abs(1 - molecule.LogP) * 8;
            else if (molecule.LogP > 3)
                score -= (molecule.LogP - 3) * 10;

            // Hydrogen bond donors penalty (should be ≤ 5)
            if (molecule.HydrogenBondDonors > 5)
                score -= (molecule.HydrogenBondDonors - 5) * 5;

            // Hydrogen bond acceptors penalty (should be ≤ 10)
            if (molecule.HydrogenBondAcceptors > 10)
                score -= (molecule.HydrogenBondAcceptors - 10) * 3;

            // Polarity bonus (moderate polarity is often better)
            var optimalPolarity = 0.5;
            var polarityDeviation = Math.Abs(molecule.Polarity - optimalPolarity);
            score -= polarityDeviation * 20;

            // Atom count considerations
            var totalAtoms = molecule.TotalAtoms;
            if (totalAtoms < 20)
                score -= (20 - totalAtoms) * 2;
            else if (totalAtoms > 100)
                score -= (totalAtoms - 100) * 1.5;

            // Add some random variation to simulate AI prediction uncertainty
            var randomFactor = (_random.NextDouble() - 0.5) * 10; // ±5 points
            score += randomFactor;

            // Ensure score is between 0 and 100
            score = Math.Max(0, Math.Min(100, score));

            return Math.Round(score, 2);
        }

        public Dictionary<string, double> GetScoreBreakdown(Molecule molecule)
        {
            var breakdown = new Dictionary<string, double>();

            // Molecular weight component
            double mwScore = 100.0;
            if (molecule.MolecularWeight < 300)
                mwScore -= Math.Abs(300 - molecule.MolecularWeight) * 0.1;
            else if (molecule.MolecularWeight > 500)
                mwScore -= (molecule.MolecularWeight - 500) * 0.15;
            breakdown["Molecular Weight"] = Math.Max(0, Math.Min(100, mwScore));

            // LogP component
            double logPScore = 100.0;
            if (molecule.LogP < 1)
                logPScore -= Math.Abs(1 - molecule.LogP) * 8;
            else if (molecule.LogP > 3)
                logPScore -= (molecule.LogP - 3) * 10;
            breakdown["LogP"] = Math.Max(0, Math.Min(100, logPScore));

            // Hydrogen bonding component
            double hbScore = 100.0;
            if (molecule.HydrogenBondDonors > 5)
                hbScore -= (molecule.HydrogenBondDonors - 5) * 5;
            if (molecule.HydrogenBondAcceptors > 10)
                hbScore -= (molecule.HydrogenBondAcceptors - 10) * 3;
            breakdown["Hydrogen Bonding"] = Math.Max(0, Math.Min(100, hbScore));

            // Polarity component
            var optimalPolarity = 0.5;
            var polarityDeviation = Math.Abs(molecule.Polarity - optimalPolarity);
            var polarityScore = 100.0 - (polarityDeviation * 20);
            breakdown["Polarity"] = Math.Max(0, Math.Min(100, polarityScore));

            return breakdown;
        }
    }
}
