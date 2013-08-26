namespace Beem.Core.Models
{
    public class RatingPromptValidator
    {
        public int CurrentLaunchCount { get; set; }
        public bool ShouldShowPrompt { get; set; }
        public bool AlreadyRated { get; set; }
    }
}
