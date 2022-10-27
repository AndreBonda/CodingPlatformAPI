using CodingPlatform.Domain;
using CodingPlatform.Web.DTO;

namespace CodingPlatform.Web.Extensions;

public static class ExtensionsToDTO
{
    public static ChallengeStatusDto ToDTO(this Submission submission)
    {
        return new ChallengeStatusDto
        {
            StartDate = submission.Challenge.CreateDate,
            EndDate = submission.Challenge.EndDate,
            SubmitDate = submission.SubmitDate,
            EvaluateDate = submission.EvaluateDate,
            Title = submission.Challenge.Title,
            Description = submission.Challenge.Description,
            Content = submission.Content,
            TotalTips = submission.Challenge.TotalTips(),
            RemainingTips = submission.RemainingTips(),
            Tips = submission.GetAvailableTips().Select(t => new TipDto
            {
                Order = t.Order,
                Description = t.Description
            })
        };
    }

    public static TournamentDto ToDTO(this Tournament tournament)
    {
        return new TournamentDto()
        {
            Id = tournament.Id,
            Name = tournament.Name,
            DateCreated = tournament.CreateDate,
            MaxParticipants = tournament.MaxParticipants,
            UsernameAdmin = tournament.Admin.Username,
            SubscriberNumber = tournament.SubscribedNumber(),
            AvailableSeats = tournament.AvailableSeats()
        };
    }
}