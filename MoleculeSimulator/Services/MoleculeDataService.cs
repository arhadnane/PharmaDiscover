using Microsoft.EntityFrameworkCore;
using MoleculeSimulator.Data;
using MoleculeSimulator.Models;

namespace MoleculeSimulator.Services
{
    public interface IMoleculeDataService
    {
        Task<List<Molecule>> GetAllMoleculesAsync();
        Task<List<Molecule>> GetTopScoringMoleculesAsync(int count = 10);
        Task<Molecule?> GetMoleculeByIdAsync(int id);
        Task<Molecule> SaveMoleculeAsync(Molecule molecule);
        Task<List<Molecule>> SaveMoleculesAsync(List<Molecule> molecules);
        Task<bool> DeleteMoleculeAsync(int id);
        Task<int> GetMoleculeCountAsync();
        Task<List<Molecule>> GetMoleculesPaginatedAsync(int page, int pageSize, string? sortBy = null, bool ascending = true);
        Task<List<Molecule>> SearchMoleculesAsync(string searchTerm);
    }

    public class MoleculeDataService : IMoleculeDataService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<MoleculeDataService> _logger;

        public MoleculeDataService(AppDbContext context, ILogger<MoleculeDataService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Molecule>> GetAllMoleculesAsync()
        {
            try
            {
                return await _context.Molecules
                    .OrderByDescending(m => m.TherapeuticScore)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all molecules");
                return new List<Molecule>();
            }
        }

        public async Task<List<Molecule>> GetTopScoringMoleculesAsync(int count = 10)
        {
            try
            {
                return await _context.Molecules
                    .OrderByDescending(m => m.TherapeuticScore)
                    .Take(count)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving top scoring molecules");
                return new List<Molecule>();
            }
        }

        public async Task<Molecule?> GetMoleculeByIdAsync(int id)
        {
            try
            {
                return await _context.Molecules.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving molecule with ID {Id}", id);
                return null;
            }
        }

        public async Task<Molecule> SaveMoleculeAsync(Molecule molecule)
        {
            try
            {
                if (molecule.Id == 0)
                {
                    _context.Molecules.Add(molecule);
                }
                else
                {
                    _context.Molecules.Update(molecule);
                }

                await _context.SaveChangesAsync();
                return molecule;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving molecule {Name}", molecule.Name);
                throw;
            }
        }

        public async Task<List<Molecule>> SaveMoleculesAsync(List<Molecule> molecules)
        {
            try
            {
                _context.Molecules.AddRange(molecules);
                await _context.SaveChangesAsync();
                return molecules;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving {Count} molecules", molecules.Count);
                throw;
            }
        }

        public async Task<bool> DeleteMoleculeAsync(int id)
        {
            try
            {
                var molecule = await _context.Molecules.FindAsync(id);
                if (molecule == null)
                    return false;

                _context.Molecules.Remove(molecule);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting molecule with ID {Id}", id);
                return false;
            }
        }

        public async Task<int> GetMoleculeCountAsync()
        {
            try
            {
                return await _context.Molecules.CountAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting molecule count");
                return 0;
            }
        }

        public async Task<List<Molecule>> GetMoleculesPaginatedAsync(int page, int pageSize, string? sortBy = null, bool ascending = true)
        {
            try
            {
                var query = _context.Molecules.AsQueryable();

                // Apply sorting
                query = sortBy?.ToLower() switch
                {
                    "name" => ascending ? query.OrderBy(m => m.Name) : query.OrderByDescending(m => m.Name),
                    "molecularweight" => ascending ? query.OrderBy(m => m.MolecularWeight) : query.OrderByDescending(m => m.MolecularWeight),
                    "logp" => ascending ? query.OrderBy(m => m.LogP) : query.OrderByDescending(m => m.LogP),
                    "therapeuticscore" => ascending ? query.OrderBy(m => m.TherapeuticScore) : query.OrderByDescending(m => m.TherapeuticScore),
                    "createdat" => ascending ? query.OrderBy(m => m.CreatedAt) : query.OrderByDescending(m => m.CreatedAt),
                    _ => query.OrderByDescending(m => m.TherapeuticScore)
                };

                return await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving paginated molecules");
                return new List<Molecule>();
            }
        }

        public async Task<List<Molecule>> SearchMoleculesAsync(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return await GetAllMoleculesAsync();

                return await _context.Molecules
                    .Where(m => m.Name.Contains(searchTerm) || 
                               m.TherapeuticCategory.Contains(searchTerm))
                    .OrderByDescending(m => m.TherapeuticScore)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching molecules with term {SearchTerm}", searchTerm);
                return new List<Molecule>();
            }
        }
    }
}
