namespace AmeriCorps.Users.Data.Core;

public sealed class About
{
    public required int Gender { get; set; }
    public required string Ethnicity { get; set; }
    public required string Race { get; set; }
    public required string CitizenshipStatus { get; set; }
    public int FamilyCombinedIncome { get; set; }
    public string UnexpectedExpenseConfidence { get; set; }
    public bool HasValidGovtDriversLicense { get; set; }
    public string VeteranStatus { get; set; }

    public About (int gender, string ethnicity, string race, string citizenshipStatus, 
                int familyCombinedIncome, string unexpectedExpenseConfidence, bool hasValidGovtDriversLicense,
                string veteranStatus) {
                    Gender = gender;
                    Ethnicity = ethnicity;
                    Race = race;
                    CitizenshipStatus = citizenshipStatus;
                    FamilyCombinedIncome = familyCombinedIncome;
                    UnexpectedExpenseConfidence = unexpectedExpenseConfidence;
                    HasValidGovtDriversLicense = hasValidGovtDriversLicense;
                    VeteranStatus = veteranStatus;
                }
}
