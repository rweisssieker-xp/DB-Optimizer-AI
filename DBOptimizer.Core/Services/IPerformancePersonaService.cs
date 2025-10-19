using DBOptimizer.Core.Models;

namespace DBOptimizer.Core.Services;

/// <summary>
/// Expert AI personas trained on best practices from top AX consultants
/// </summary>
public interface IPerformancePersonaService
{
    Task<List<PerformancePersona>> GetAvailablePersonasAsync();
    Task<ExpertRecommendation> GetExpertAdviceAsync(string personaId, PerformanceProblem problem);
    Task<ConsensusRecommendation> GetConsensusAdviceAsync(PerformanceProblem problem);
}

