using Microsoft.AspNetCore.Mvc;
using MoleculeSimulator.Models;
using MoleculeSimulator.Services;

namespace MoleculeSimulator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoleculesController : ControllerBase
    {
        private readonly IMoleculeGeneratorService _generatorService;
        private readonly IMoleculeDataService _dataService;
        private readonly IScoringService _scoringService;
        private readonly ILogger<MoleculesController> _logger;

        public MoleculesController(
            IMoleculeGeneratorService generatorService,
            IMoleculeDataService dataService,
            IScoringService scoringService,
            ILogger<MoleculesController> logger)
        {
            _generatorService = generatorService;
            _dataService = dataService;
            _scoringService = scoringService;
            _logger = logger;
        }

        /// <summary>
        /// Generate new molecules based on specified parameters
        /// </summary>
        [HttpPost("generate")]
        public async Task<ActionResult<List<Molecule>>> GenerateMolecules([FromBody] MoleculeGenerationRequest request)
        {
            try
            {
                if (request.Count <= 0 || request.Count > 1000)
                {
                    return BadRequest("Count must be between 1 and 1000");
                }

                var molecules = _generatorService.GenerateMolecules(request);

                if (request.SaveToDatabase)
                {
                    await _dataService.SaveMoleculesAsync(molecules);
                    _logger.LogInformation("Generated and saved {Count} molecules to database", molecules.Count);
                }
                else
                {
                    _logger.LogInformation("Generated {Count} molecules (not saved)", molecules.Count);
                }

                return Ok(molecules);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating molecules");
                return StatusCode(500, "An error occurred while generating molecules");
            }
        }

        /// <summary>
        /// Get all molecules from the database
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<Molecule>>> GetAllMolecules(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50,
            [FromQuery] string? sortBy = null,
            [FromQuery] bool ascending = false)
        {
            try
            {
                var molecules = await _dataService.GetMoleculesPaginatedAsync(page, pageSize, sortBy, ascending);
                var totalCount = await _dataService.GetMoleculeCountAsync();                Response.Headers["X-Total-Count"] = totalCount.ToString();
                Response.Headers["X-Page"] = page.ToString();
                Response.Headers["X-Page-Size"] = pageSize.ToString();

                return Ok(molecules);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving molecules");
                return StatusCode(500, "An error occurred while retrieving molecules");
            }
        }

        /// <summary>
        /// Get top scoring molecules
        /// </summary>
        [HttpGet("top/{count}")]
        public async Task<ActionResult<List<Molecule>>> GetTopMolecules(int count = 10)
        {
            try
            {
                if (count <= 0 || count > 100)
                {
                    return BadRequest("Count must be between 1 and 100");
                }

                var molecules = await _dataService.GetTopScoringMoleculesAsync(count);
                return Ok(molecules);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving top molecules");
                return StatusCode(500, "An error occurred while retrieving top molecules");
            }
        }

        /// <summary>
        /// Get a specific molecule by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Molecule>> GetMolecule(int id)
        {
            try
            {
                var molecule = await _dataService.GetMoleculeByIdAsync(id);
                if (molecule == null)
                {
                    return NotFound($"Molecule with ID {id} not found");
                }

                return Ok(molecule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving molecule {Id}", id);
                return StatusCode(500, "An error occurred while retrieving the molecule");
            }
        }

        /// <summary>
        /// Get score breakdown for a specific molecule
        /// </summary>
        [HttpGet("{id}/score-breakdown")]
        public async Task<ActionResult<Dictionary<string, double>>> GetScoreBreakdown(int id)
        {
            try
            {
                var molecule = await _dataService.GetMoleculeByIdAsync(id);
                if (molecule == null)
                {
                    return NotFound($"Molecule with ID {id} not found");
                }

                var breakdown = _scoringService.GetScoreBreakdown(molecule);
                return Ok(breakdown);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting score breakdown for molecule {Id}", id);
                return StatusCode(500, "An error occurred while calculating score breakdown");
            }
        }

        /// <summary>
        /// Search molecules by name or category
        /// </summary>
        [HttpGet("search")]
        public async Task<ActionResult<List<Molecule>>> SearchMolecules([FromQuery] string searchTerm)
        {
            try
            {
                var molecules = await _dataService.SearchMoleculesAsync(searchTerm);
                return Ok(molecules);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching molecules");
                return StatusCode(500, "An error occurred while searching molecules");
            }
        }

        /// <summary>
        /// Delete a molecule by ID
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMolecule(int id)
        {
            try
            {
                var success = await _dataService.DeleteMoleculeAsync(id);
                if (!success)
                {
                    return NotFound($"Molecule with ID {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting molecule {Id}", id);
                return StatusCode(500, "An error occurred while deleting the molecule");
            }
        }

        /// <summary>
        /// Get statistics about molecules in the database
        /// </summary>
        [HttpGet("statistics")]
        public async Task<ActionResult<object>> GetStatistics()
        {
            try
            {
                var allMolecules = await _dataService.GetAllMoleculesAsync();
                
                if (!allMolecules.Any())
                {
                    return Ok(new { message = "No molecules in database" });
                }

                var stats = new
                {
                    TotalCount = allMolecules.Count,
                    AverageTherapeuticScore = allMolecules.Average(m => m.TherapeuticScore),
                    HighestScore = allMolecules.Max(m => m.TherapeuticScore),
                    LowestScore = allMolecules.Min(m => m.TherapeuticScore),
                    AverageMolecularWeight = allMolecules.Average(m => m.MolecularWeight),
                    AverageLogP = allMolecules.Average(m => m.LogP),
                    CategoryDistribution = allMolecules
                        .GroupBy(m => m.TherapeuticCategory)
                        .ToDictionary(g => g.Key, g => g.Count())
                };

                return Ok(stats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating statistics");
                return StatusCode(500, "An error occurred while calculating statistics");
            }
        }
    }
}
