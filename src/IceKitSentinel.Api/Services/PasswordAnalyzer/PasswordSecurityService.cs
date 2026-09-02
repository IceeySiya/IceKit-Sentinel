using IceKitSentinel.Api.DTOs.PasswordAnalyzer;
namespace IceKitSentinel.Api.Services.PasswordAnalyzer;
public class PasswordSecurityService
{
    public async Task<PasswordAnalysisResponse> Analyze(PasswordAnalysisRequest request)
    {
        // the analysis logic 
        var response = new PasswordAnalysisResponse()
        {
            Length = request.Password.Length,
            Score = DetermineScore(request),
            HasLowercase = request.Password.Any(char.IsLower),
            HasUppercase = request.Password.Any(char.IsUpper),
            HasNumbers = request.Password.Any(char.IsDigit),
            HasSpecialCharacters = request.Password.Any(ch => !char.IsLetterOrDigit(ch) && !char.IsWhiteSpace(ch)),
            Recommendations = new List<string>()
        };
        // Determine the strength based on the score
        response.Strength = response.Score switch
        {
            0 => "Very Weak",
            1 => "Weak",
            2 => "Moderate",
            3 => "Strong",
            4 => "Very Strong",
            5 => "Excellent",
            _ => throw new NotImplementedException()
        };

        //Determine recommendations based on the analysis
        response.Recommendations = GetRecommendations(request);
        return response;
    }

    public int DetermineScore(PasswordAnalysisRequest request)
    {
        int score = 0;
        if (request.Password.Length >= 8) score++;
        if (request.Password.Any(char.IsLower)) score++;
        if (request.Password.Any(char.IsUpper)) score++;
        if (request.Password.Any(char.IsDigit)) score++;
        if (request.Password.Any(ch => !char.IsLetterOrDigit(ch) && !char.IsWhiteSpace(ch))) score++;
        return score;
    }

    public List<string> GetRecommendations(PasswordAnalysisRequest request)
    {
        var recommendations = new List<string>();
        if (request.Password.Length < 8) recommendations.Add("Use at least 8 characters.");
        if (!request.Password.Any(char.IsLower)) recommendations.Add("Include lowercase letters.");
        if (!request.Password.Any(char.IsUpper)) recommendations.Add("Include uppercase letters.");
        if (!request.Password.Any(char.IsDigit)) recommendations.Add("Include numbers.");
        if (!request.Password.Any(ch => !char.IsLetterOrDigit(ch) && !char.IsWhiteSpace(ch))) recommendations.Add("Include special characters.");
        return recommendations;
    }
}