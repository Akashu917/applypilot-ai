namespace ApplyPilot.Domain.Enums;

public enum UserRole
{
    Candidate = 1,
    Administrator = 2
}

public enum PlanTier
{
    Free = 1,
    Pro = 2,
    Premium = 3
}

public enum ApplicationStatus
{
    Discovered = 1,
    Saved = 2,
    Eligible = 3,
    ResumeGenerated = 4,
    ApplicationPrepared = 5,
    PendingApproval = 6,
    Approved = 7,
    Submitted = 8,
    SubmissionFailed = 9,
    UnderReview = 10,
    Shortlisted = 11,
    InterviewScheduled = 12,
    Rejected = 13,
    OfferReceived = 14
}

public enum PortalCode
{
    LinkedIn = 1,
    Naukri = 2,
    Foundit = 3,
    Shine = 4,
    Indeed = 5,
    Direct = 6
}

