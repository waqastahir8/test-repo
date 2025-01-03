﻿using System.Data;
using System.Drawing.Printing;
using AmeriCorps.Users.Data.Core;
using AmeriCorps.Users.Data.Core.Model;

namespace AmeriCorps.Users.Api.Services;

public interface IResponseMapper
{
    UserResponse? Map(User? user);

    RoleResponse? Map(Role? role);

    SavedSearchResponseModel Map(SavedSearch search);

    ReferenceResponseModel Map(Reference reference);

    List<SavedSearchResponseModel> Map(List<SavedSearch> searches);

    CollectionResponseModel Map(Collection collection);

    CollectionListResponseModel Map(List<Collection>? collection);

    List<ReferenceResponseModel> Map(List<Reference> references);
    List<CountryOfBirthResponse> Map(List<CountryOfBirth> countryOfBirth);
    List<StateOfBirthResponse> Map(List<StateOfBirth> stateOfBirth);
    List<CityOfBirthResponse> Map(List<CityOfBirth> cityOfBirth);
    List<DateOfBirthResponse> Map(List<DateOfBirth> dateOfBirth);
    List<EncryptedSocialSecurityNumberResponse> Map(List<EncryptedSocialSecurityNumber> encryptedSocialSecurityNumber);

    List<DirectDepositResponse> Map(List<DirectDeposit> directDeposits);

    List<RoleResponse> Map(List<Role> role);

    OrganizationResponse? Map(Organization? organization);

    ProjectResponse? Map(Project? project);

    UserListResponse? Map(UserList? userList);

    AccessResponse? Map(Access? access);

    List<AccessResponse> Map(List<Access> access);

    List<OrganizationResponse> Map(List<Organization> orgList);

    List<ProjectResponse> Map(List<Project> projList);

    List<OperatingSiteResponse> Map(List<OperatingSite> operatingSiteList);

    OperatingSiteResponse? Map(OperatingSite? operatingSite);

    AwardResponse Map(Award award);

    SubGranteeResponse Map(SubGrantee subGrantee);
    CountryOfBirthResponse Map(CountryOfBirth countryOfBirth);

    DirectDepositResponse Map(DirectDeposit directDeposit);

    TaxWithHoldingResponse Map(TaxWithHolding taxWithHolding);

    SocialSecurityVerificationResponse? Map(SocialSecurityVerification? status);

}

public sealed class ResponseMapper : IResponseMapper
{
    public SavedSearchResponseModel Map(SavedSearch search) => new()
    {
        Id = search.Id,
        UserId = search.UserId,
        Name = search.Name,
        Filters = search.Filters,
        NotificationsOn = search.NotificationsOn
    };

    public List<SavedSearchResponseModel> Map(List<SavedSearch> searches) =>
        MapperUtils.MapList<AmeriCorps.Users.Data.Core.SavedSearch, SavedSearchResponseModel>(
                            searches,
                            a => new SavedSearchResponseModel
                            {
                                Id = a.Id,
                                UserId = a.UserId,
                                Name = a.Name,
                                Filters = a.Filters,
                                NotificationsOn = a.NotificationsOn
                            });

    public CollectionResponseModel Map(Collection collection)
    {
        return new CollectionResponseModel()
        {
            Id = collection.Id,
            ListingId = collection.ListingId,
            UserId = collection.UserId,
            Type = collection.Type
        };
    }

    public CollectionListResponseModel Map(List<Collection>? collection)
    {
        var collectionResponseList = new CollectionListResponseModel();
        if (collection == null || collection.Count == 0)
            return collectionResponseList;

        var collectionItem = collection.First();
        collectionResponseList.Type = collectionItem.Type;
        collectionResponseList.UserId = collectionItem.UserId;

        collectionResponseList.Listings = collection.Select(c => c.ListingId).ToList();
        return collectionResponseList;
    }

    public List<ReferenceResponseModel> Map(List<Reference> references) =>
        MapperUtils.MapList<AmeriCorps.Users.Data.Core.Reference, ReferenceResponseModel>(
                            references,
                            r => new ReferenceResponseModel
                            {
                                Id = r.Id,
                                UserId = r.UserId,
                                TypeId = r.TypeId,
                                Relationship = r.Relationship,
                                RelationshipLength = r.RelationshipLength,
                                ContactName = r.ContactName,
                                Email = r.Email,
                                Phone = r.Phone,
                                Address = r.Address,
                                Company = r.Company,
                                Position = r.Position,
                                Notes = r.Notes,
                                CanContact = r.CanContact,
                                Contacted = r.Contacted,
                                DateContacted = r.DateContacted
                            });

    public ReferenceResponseModel Map(Reference reference) => new()
    {
        Id = reference.Id,
        UserId = reference.UserId,
        TypeId = reference.TypeId,
        Relationship = reference.Relationship,
        RelationshipLength = reference.RelationshipLength,
        ContactName = reference.ContactName,
        Email = reference.Email,
        Phone = reference.Phone,
        Address = reference.Address,
        Company = reference.Company,
        Position = reference.Position,
        Notes = reference.Notes,
        CanContact = reference.CanContact,
        Contacted = reference.Contacted,
        DateContacted = reference.DateContacted
    };

    public List<CountryOfBirthResponse> Map(List<CountryOfBirth> countryOfBirth) =>
        MapperUtils.MapList<CountryOfBirth, CountryOfBirthResponse>(
            countryOfBirth,
            c => new CountryOfBirthResponse
            {
                BirthCountry = c.BirthCountry
            });

    public List<StateOfBirthResponse> Map(List<StateOfBirth> stateOfBirth) =>
        MapperUtils.MapList<StateOfBirth, StateOfBirthResponse>(
            stateOfBirth,
            c => new StateOfBirthResponse
            {
                BirthState = c.BirthState
            });
    public List<CityOfBirthResponse> Map(List<CityOfBirth> cityOfBirth) =>
           MapperUtils.MapList<CityOfBirth, CityOfBirthResponse>(
               cityOfBirth,
               c => new CityOfBirthResponse
               {
                   BirthCity = c.BirthCity
               });
    public List<DateOfBirthResponse> Map(List<DateOfBirth> dateOfBirth) =>
            MapperUtils.MapList<DateOfBirth, DateOfBirthResponse>(
                dateOfBirth,
                c => new DateOfBirthResponse
                {
                    BirthDate = c.BirthDate
                });
    public List<EncryptedSocialSecurityNumberResponse> Map(List<EncryptedSocialSecurityNumber> encryptedSocialSecurityNumber) =>
            MapperUtils.MapList<EncryptedSocialSecurityNumber, EncryptedSocialSecurityNumberResponse>(
                encryptedSocialSecurityNumber,
                c => new EncryptedSocialSecurityNumberResponse
                {
                    SociaSecurityNumber = c.SociaSecurityNumber
                });
    public List<DirectDepositResponse> Map(List<DirectDeposit> directDeposits) =>
        MapperUtils.MapList<AmeriCorps.Users.Data.Core.Model.DirectDeposit, DirectDepositResponse>(
                            directDeposits,
                            d => new DirectDepositResponse
                            {
                                Id = d.Id,
                                UserId = d.UserId,
                                AccountType = (Models.AccountType)d.AccountType,
                                InstitutionName = d.InstitutionName,
                                AchRoutingNumber = d.AchRoutingNumber,
                                ReEnterAchRoutingNumber = d.ReEnterAchRoutingNumber,
                                AccountNumber = d.AccountNumber,
                                ReEnterAccountNumber = d.ReEnterAccountNumber,
                                MailByPaycheck = d.MailByPaycheck
                            });



    public List<TaxWithHoldingResponse> Map(List<TaxWithHolding> taxWithHolding) =>
        MapperUtils.MapList<AmeriCorps.Users.Data.Core.Model.TaxWithHolding, TaxWithHoldingResponse>(
            taxWithHolding,
            t => new TaxWithHoldingResponse
            {
                Id = t.Id,
                UserId = t.UserId,
                TaxWithHoldingType = (Models.TaxWithHoldingType)t.TaxWithHoldingType,
                AdditionalWithHoldings = t.AdditionalWithHoldings,
                AdditionalWithHoldings2 = t.AdditionalWithHoldings2,
                DependentsUnder17 = t.DependentsUnder17,
                DependentsOver17 = t.DependentsOver17,
                OtherIncome = t.OtherIncome,
                Deductions = t.Deductions,
                ExtraWithHoldingAmount = t.ExtraWithHoldingAmount,
                ModifiedDate = t.ModifiedDate
            });

    public CountryOfBirthResponse Map(CountryOfBirth countryOfBirth) => new()
    {
        Id = countryOfBirth.Id,
        BirthCountry = countryOfBirth.BirthCountry,
        UserId = countryOfBirth.UserId
    };

    public StateOfBirthResponse Map(StateOfBirth stateOfBirth) => new()
    {
        Id = stateOfBirth.Id,
        BirthState = stateOfBirth.BirthState,
        UserId = stateOfBirth.UserId
    };
    public CityOfBirthResponse Map(CityOfBirth cityOfBirth) => new()
    {
        Id = cityOfBirth.Id,
        BirthCity = cityOfBirth.BirthCity,
        UserId = cityOfBirth.UserId
    };
    public DateOfBirthResponse Map(DateOfBirth dateOfBirth) => new()
    {
        Id = dateOfBirth.Id,
        BirthDate = dateOfBirth.BirthDate,
        UserId = dateOfBirth.UserId
    };
    public EncryptedSocialSecurityNumberResponse Map(EncryptedSocialSecurityNumber encryptedSocialSecurityNumber) => new()
    {
        Id = encryptedSocialSecurityNumber.Id,
        SociaSecurityNumber = encryptedSocialSecurityNumber.SociaSecurityNumber,
        UserId = encryptedSocialSecurityNumber.UserId
    };
    public DirectDepositResponse Map(DirectDeposit directDeposit) => new()
    {
        Id = directDeposit.Id,
        UserId = directDeposit.UserId,
        AccountType = (Models.AccountType)directDeposit.AccountType,
        InstitutionName = directDeposit.InstitutionName,
        AchRoutingNumber = directDeposit.AchRoutingNumber,
        ReEnterAchRoutingNumber = directDeposit.ReEnterAchRoutingNumber,
        AccountNumber = directDeposit.AccountNumber,
        ReEnterAccountNumber = directDeposit.ReEnterAccountNumber,
        MailByPaycheck = directDeposit.MailByPaycheck
    };

    public TaxWithHoldingResponse Map(TaxWithHolding taxWithHolding) => new()
    {
        Id = taxWithHolding.Id,
        UserId = taxWithHolding.UserId,
        TaxWithHoldingType = (Models.TaxWithHoldingType)taxWithHolding.TaxWithHoldingType,
        AdditionalWithHoldings = taxWithHolding.AdditionalWithHoldings,
        AdditionalWithHoldings2 = taxWithHolding.AdditionalWithHoldings2,
        DependentsUnder17 = taxWithHolding.DependentsUnder17,
        DependentsOver17 = taxWithHolding.DependentsOver17,
        OtherIncome = taxWithHolding.OtherIncome,
        Deductions = taxWithHolding.Deductions,
        ExtraWithHoldingAmount = taxWithHolding.ExtraWithHoldingAmount,
        ModifiedDate = taxWithHolding.ModifiedDate
    };

    public UserResponse? Map(User? user) => user == null ? null : new()
    {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        MiddleName = user.MiddleName,
        PreferredName = user.PreferredName,
        UserName = user.UserName,
        ExternalAccountId = user.ExternalAccountId,
        DateOfBirth = user.DateOfBirth,
        Pronouns = user.Pronouns,
        Suffix = user.Suffix,
        Prefix = user.Prefix,
        OrgCode = user.OrgCode,
        EncryptedSocialSecurityNumber = user.EncryptedSocialSecurityNumber,
        PPIUpdateNote = user.PPIUpdateNote,
        CitzenShipStatus = (global::AmeriCorps.Users.Models.CitizenshipStatusRequestModel)user.CitzenShipStatus,
        CountryOfBirth = MapperUtils.MapList<CountryOfBirth, CountryOfBirthRequestModel>(
            user.CountryOfBirth, c =>
                new CountryOfBirthRequestModel
                {
                    BirthCountry = c.BirthCountry
                }),
        StateOfBirth = MapperUtils.MapList<StateOfBirth, StateOfBirthRequestModel>(
            user.StateOfBirth, c =>
                new StateOfBirthRequestModel
                {
                    BirthState = c.BirthState
                }),
        CityOfBirth = MapperUtils.MapList<CityOfBirth, CityOfBirthRequestModel>(
            user.CityOfBirth, c =>
                new CityOfBirthRequestModel
                {
                    BirthCity = c.BirthCity
                }),
        DateOfBirths = MapperUtils.MapList<DateOfBirth, DateOfBirthRequestModel>(
            user.DateOfBirths, d =>
                new DateOfBirthRequestModel
                {
                    BirthDate = d.BirthDate
                }),
        EncryptedSocialSecurityNumbers = MapperUtils.MapList<EncryptedSocialSecurityNumber, EncryptedSocialSecurityNumberRequestModel>(
            user.EncryptedSocialSecurityNumbers, d =>
                new EncryptedSocialSecurityNumberRequestModel
                {
                    SociaSecurityNumber = d.SociaSecurityNumber
                }),
        UserAccountStatus = (AccountStatusRequestModel)user.UserAccountStatus,
        InviteUserId = user.InviteUserId,
        InviteDate = user.InviteDate,
        SocialSecurityVerification = Map(user.SocialSecurityVerification),
        Attributes = MapperUtils.MapList<AmeriCorps.Users.Data.Core.Attribute, AttributeRequestModel>(
                        user.Attributes,
                        a => new AttributeRequestModel
                        {
                            Id = a.Id,
                            Type = a.Type,
                            Value = a.Value
                        }),

        Languages = MapperUtils.MapList<Language, LanguageRequestModel>(
                        user.Languages, l =>
                        new LanguageRequestModel
                        {
                            Id = l.Id,
                            PickListId = l.PickListId,
                            IsPrimary = l.IsPrimary,
                            SpeakingAbility = l.SpeakingAbility,
                            WritingAbility = l.WritingAbility
                        }),

        Addresses = MapperUtils.MapList<Address, AddressRequestModel>(
                        user.Addresses, a =>
                        new AddressRequestModel
                        {
                            Id = a.Id,
                            IsForeign = a.IsForeign,
                            Type = a.Type,
                            Street1 = a.Street1,
                            Street2 = a.Street2,
                            City = a.City,
                            State = a.State,
                            Country = a.Country,
                            ZipCode = a.ZipCode,
                            MovingWithinSixMonths = a.MovingWithinSixMonths
                        }),

        Education = MapperUtils.MapList<Education, EducationRequestModel>(
                        user.Education, e =>
                        new EducationRequestModel
                        {
                            Id = e.Id,
                            Level = e.Level,
                            MajorAreaOfStudy = e.MajorAreaOfStudy,
                            Institution = e.Institution,
                            City = e.City,
                            State = e.State,
                            DateAttendedFrom = e.DateAttendedFrom,
                            DateAttendedTo = e.DateAttendedTo,
                            DegreeTypePursued = e.DegreeTypePursued,
                            DegreeCompleted = e.DegreeCompleted
                        }),

        Skills = MapperUtils.MapList<Skill, SkillRequestModel>(
                        user.Skills, s =>
                        new SkillRequestModel
                        {
                            Id = s.Id,
                            PickListId = s.PickListId
                        }),

        MilitaryService = MapperUtils.MapList<MilitaryService, MilitaryServiceRequestModel>(
                        user.MilitaryService, s =>
                        new MilitaryServiceRequestModel
                        {
                            Id = s.Id,
                            PickListId = s.PickListId
                        }),

        SavedSearches = MapperUtils.MapList<SavedSearch, SavedSearchRequestModel>(
                            user.SavedSearches, s => new SavedSearchRequestModel
                            {
                                Id = s.Id,
                                UserId = s.UserId,
                                Name = s.Name,
                                Filters = s.Filters,
                                NotificationsOn = s.NotificationsOn
                            }),

        References = MapperUtils.MapList<Reference, ReferenceRequestModel>(
                    user.References, r => new ReferenceRequestModel
                    {
                        TypeId = r.TypeId,
                        Relationship = r.Relationship,
                        RelationshipLength = r.RelationshipLength,
                        ContactName = r.ContactName,
                        Email = r.Email,
                        Phone = r.Phone,
                        Address = r.Address,
                        Company = r.Company,
                        Position = r.Position,
                        Notes = r.Notes,
                        CanContact = r.CanContact,
                        Contacted = r.Contacted,
                        DateContacted = r.DateContacted
                    }),

        Relatives = MapperUtils.MapList<Relative, RelativeRequestModel>(
                        user.Relatives, r =>
                        new RelativeRequestModel
                        {
                            Id = r.Id,
                            Relationship = r.Relationship,
                            HighestEducationLevel = r.HighestEducationLevel,
                            AnnualIncome = r.AnnualIncome
                        }),

        CommunicationMethods = MapperUtils.MapList<CommunicationMethod,
                                        CommunicationMethodRequestModel>(
                        user.CommunicationMethods, cm =>
                        new CommunicationMethodRequestModel
                        {
                            Id = cm.Id,
                            Type = cm.Type,
                            Value = cm.Value,
                            IsPreferred = cm.IsPreferred
                        }),

        Collection = MapperUtils.MapList<Collection, CollectionRequestModel>(
            user.Collection, c =>
                new CollectionRequestModel()
                {
                    UserId = c.UserId,
                    Type = c.Type,
                    ListingId = c.ListingId,
                }),

        UserRoles = MapperUtils.MapList<UserRole, UserRoleRequestModel>(
            user.Roles, r =>
                new UserRoleRequestModel()
                {
                    //Id = r.Id,
                    RoleName = r.RoleName,
                    FunctionalName = r.FunctionalName
                }),

        UserProjects = MapperUtils.MapList<UserProject, UserProjectRequestModel>(
            user.UserProjects, p =>
                new UserProjectRequestModel()
                {
                    ProjectName = p.ProjectName,
                    ProjectCode = p.ProjectCode,
                    ProjectType = p.ProjectType,
                    ProjectOrg = p.ProjectOrg,
                    Active = p.Active,
                    ProjectRoles = Map(p.ProjectRoles),
                    ProjectAccess = Map(p.ProjectAccess)
                }),
        DirectDeposits = MapperUtils.MapList<DirectDeposit, DirectDepositRequestModel>(
            user.DirectDeposits, d =>
                new DirectDepositRequestModel()
                {
                    AccountType = (Models.AccountType)d.AccountType,
                    InstitutionName = d.InstitutionName,
                    AchRoutingNumber = d.AchRoutingNumber,
                    ReEnterAchRoutingNumber = d.ReEnterAchRoutingNumber,
                    AccountNumber = d.AccountNumber,
                    ReEnterAccountNumber = d.ReEnterAccountNumber,
                    MailByPaycheck = d.MailByPaycheck
                }),
        TaxWithHoldings = MapperUtils.MapList<TaxWithHolding, TaxWithHoldingRequestModel>(
            user.TaxWithHoldings, t =>
                new TaxWithHoldingRequestModel()
                {
                    TaxWithHoldingType = (Models.TaxWithHoldingType)t.TaxWithHoldingType,
                    AdditionalWithHoldings = t.AdditionalWithHoldings,
                    AdditionalWithHoldings2 = t.AdditionalWithHoldings2,
                    DependentsUnder17 = t.DependentsUnder17,
                    DependentsOver17 = t.DependentsOver17,
                    OtherIncome = t.OtherIncome,
                    Deductions = t.Deductions,
                    ExtraWithHoldingAmount = t.ExtraWithHoldingAmount,
                    ModifiedDate = t.ModifiedDate
                })
    };

    public RoleResponse? Map(Role? role) => role == null ? null : new()
    {
        Id = role.Id,
        RoleName = role.RoleName,
        FunctionalName = role.FunctionalName,
        Description = role.Description,
        RoleType = role.RoleType
    };

    public List<RoleResponse> Map(List<Role> role) =>
       MapperUtils.MapList<AmeriCorps.Users.Data.Core.Role, RoleResponse>(
                           role,
                           a => new RoleResponse
                           {
                               Id = a.Id,
                               RoleName = a.RoleName,
                               FunctionalName = a.FunctionalName,
                               Description = a.Description,
                               RoleType = a.RoleType
                           });

    public List<ProjectRoleRequestModel> Map(List<ProjectRole> role) =>
       MapperUtils.MapList<ProjectRole, ProjectRoleRequestModel>(
                           role,
                           a => new ProjectRoleRequestModel
                           {
                               //    Id = a.Id,
                               RoleName = a.RoleName,
                               FunctionalName = a.FunctionalName
                           });

    public List<ProjectAccessRequestModel> Map(List<ProjectAccess> access) =>
       MapperUtils.MapList<ProjectAccess, ProjectAccessRequestModel>(
                           access,
                           a => new ProjectAccessRequestModel
                           {
                               AccessName = a.AccessName,
                               AccessLevel = a.AccessLevel
                           });

    public List<AccessResponse> Map(List<Access> access) =>
       MapperUtils.MapList<Access, AccessResponse>(
                           access,
                           a => new AccessResponse
                           {
                               Id = a.Id,
                               AccessName = a.AccessName,
                               AccessLevel = a.AccessLevel,
                               AccessType = a.AccessType,
                               Description = a.Description
                           });

    public List<OrganizationResponse> Map(List<Organization> orgList) =>
       MapperUtils.MapList<Organization, OrganizationResponse>(
                           orgList,
                           o => new OrganizationResponse
                           {
                               Id = o.Id,
                               OrgName = o.OrgName,
                               OrgCode = o.OrgCode,
                               Description = o.Description
                           });

    public List<ProjectResponse> Map(List<Project> projList) =>
       MapperUtils.MapList<Project, ProjectResponse>(
                           projList,
                           p => new ProjectResponse
                           {
                               Id = p.Id,
                               ProjectName = p.ProjectName,
                               ProjectOrgCode = p.ProjectOrgCode,
                               ProjectCode = p.ProjectCode,
                               ProjectId = p.ProjectId,
                               GspProjectId = p.GspProjectId,
                               ProgramName = p.ProgramName,
                               ProgramYear = p.ProgramYear,
                               AuthorizedRep = Map(p.AuthorizedRep),
                               ProjectDirector = Map(p.ProjectDirector),
                               StreetAddress = p.StreetAddress,
                               City = p.City,
                               State = p.State,
                               ZipCode = p.ZipCode,
                               Active = p.Active,

                               Award = Map(p.Award),

                               ProjectPeriodStartDt = p.ProjectPeriodStartDt,
                               ProjectPeriodEndDt = p.ProjectPeriodEndDt,
                               EnrollmentStartDt = p.EnrollmentStartDt,
                               EnrollmentEndDt = p.EnrollmentEndDt,

                               OperatingSites = MapperUtils.MapList<AmeriCorps.Users.Data.Core.OperatingSite, OperatingSiteRequestModel>(
                                    p.OperatingSites,
                                    o => new OperatingSiteRequestModel
                                    {
                                        Id = o.Id,
                                        ProgramYear = o.ProgramYear,
                                        Active = o.Active,
                                        OperatingSiteName = o.OperatingSiteName,
                                        ContactName = o.ContactName,
                                        EmailAddress = o.EmailAddress,
                                        PhoneNumber = o.PhoneNumber,
                                        StreetAddress = o.StreetAddress,
                                        StreetAddress2 = o.StreetAddress2,
                                        City = o.City,
                                        State = o.State,
                                        ZipCode = o.ZipCode,
                                        Plus4 = o.Plus4,
                                        InviteUserId = o.InviteUserId,
                                        InviteDate = o.InviteDate,
                                        AwardedMsys = o.AwardedMsys,
                                        LivingAllowanceMsys = o.LivingAllowanceMsys,
                                        NonLivingAllowanceMsys = o.NonLivingAllowanceMsys
                                    }),

                               //Map(p.OperatingSites),
                               SubGrantees = Map(p.SubGrantees),

                               ProjectType = p.ProjectType,
                               Description = p.Description,
                               TotalAwardedMsys = p.TotalAwardedMsys,
                               LivingAllowanceMsys = p.LivingAllowanceMsys,
                               NonLivingAllowanceMsys = p.NonLivingAllowanceMsys
                           });

    public OrganizationResponse? Map(Organization? organization) => organization == null ? null : new()
    {
        Id = organization.Id,
        OrgName = organization.OrgName,
        OrgCode = organization.OrgCode,
        Description = organization.Description
    };

    public ProjectResponse? Map(Project? project) => project == null ? null : new()
    {
        Id = project.Id,
        ProjectName = project.ProjectName,
        ProjectOrgCode = project.ProjectOrgCode,
        ProjectCode = project.ProjectCode,
        ProjectId = project.ProjectId,
        GspProjectId = project.GspProjectId,
        ProgramName = project.ProgramName,
        ProgramYear = project.ProgramYear,
        AuthorizedRep = Map(project.AuthorizedRep),
        ProjectDirector = Map(project.ProjectDirector),
        StreetAddress = project.StreetAddress,
        City = project.City,
        State = project.State,
        ZipCode = project.ZipCode,
        Active = project.Active,

        Award = Map(project.Award),

        ProjectPeriodStartDt = project.ProjectPeriodStartDt,
        ProjectPeriodEndDt = project.ProjectPeriodEndDt,
        EnrollmentStartDt = project.EnrollmentStartDt,
        EnrollmentEndDt = project.EnrollmentEndDt,

        OperatingSites = MapperUtils.MapList<AmeriCorps.Users.Data.Core.OperatingSite, OperatingSiteRequestModel>(
                        project.OperatingSites,
                        o => new OperatingSiteRequestModel
                        {
                            Id = o.Id,
                            ProgramYear = o.ProgramYear,
                            Active = o.Active,
                            OperatingSiteName = o.OperatingSiteName,
                            ContactName = o.ContactName,
                            EmailAddress = o.EmailAddress,
                            PhoneNumber = o.PhoneNumber,
                            StreetAddress = o.StreetAddress,
                            StreetAddress2 = o.StreetAddress2,
                            City = o.City,
                            State = o.State,
                            ZipCode = o.ZipCode,
                            Plus4 = o.Plus4,
                            InviteUserId = o.InviteUserId,
                            InviteDate = o.InviteDate,
                            AwardedMsys = o.AwardedMsys,
                            LivingAllowanceMsys = o.LivingAllowanceMsys,
                            NonLivingAllowanceMsys = o.NonLivingAllowanceMsys
                        }),

        SubGrantees = Map(project.SubGrantees),

        ProjectType = project.ProjectType,
        Description = project.Description,
        TotalAwardedMsys = project.TotalAwardedMsys,
        LivingAllowanceMsys = project.LivingAllowanceMsys,
        NonLivingAllowanceMsys = project.NonLivingAllowanceMsys
    };

    public AwardResponse Map(Award award) => new()
    {
        Id = award.Id,
        AwardCode = award.AwardCode,
        AwardName = award.AwardName,
        GspListingNumber = award.GspListingNumber,
        Fain = award.Fain,
        Uei = award.Uei,
        PerformanceStartDt = award.PerformanceStartDt,
        PerformanceEndDt = award.PerformanceEndDt
    };

    public SubGranteeResponse Map(SubGrantee subGrantee) => new()
    {
        Id = subGrantee.Id,
        GranteeCode = subGrantee.GranteeCode,
        GranteeName = subGrantee.GranteeName,
        Uei = subGrantee.Uei,
        StreetAddress = subGrantee.StreetAddress,
        City = subGrantee.City,
        State = subGrantee.State,
        ZipCode = subGrantee.ZipCode,
        AwardedMsys = subGrantee.AwardedMsys,
        LivingAllowanceMsys = subGrantee.LivingAllowanceMsys,
        NonLivingAllowanceMsys = subGrantee.NonLivingAllowanceMsys
    };

    public List<SubGranteeRequestModel> Map(List<SubGrantee> subGranteeList) =>
       MapperUtils.MapList<SubGrantee, SubGranteeRequestModel>(
                           subGranteeList,
                           o => new SubGranteeRequestModel
                           {
                               GranteeCode = o.GranteeCode,
                               GranteeName = o.GranteeName,
                               Uei = o.Uei,
                               StreetAddress = o.StreetAddress,
                               City = o.City,
                               State = o.State,
                               ZipCode = o.ZipCode,
                               AwardedMsys = o.AwardedMsys,
                               LivingAllowanceMsys = o.LivingAllowanceMsys,
                               NonLivingAllowanceMsys = o.NonLivingAllowanceMsys
                           });

    public OperatingSiteResponse? Map(OperatingSite? operatingSite) => operatingSite == null ? null : new()
    {
        Id = operatingSite.Id,
        ProgramYear = operatingSite.ProgramYear,
        Active = operatingSite.Active,
        OperatingSiteName = operatingSite.OperatingSiteName,
        ContactName = operatingSite.ContactName,
        EmailAddress = operatingSite.EmailAddress,
        PhoneNumber = operatingSite.PhoneNumber,
        StreetAddress = operatingSite.StreetAddress,
        StreetAddress2 = operatingSite.StreetAddress2,
        City = operatingSite.City,
        State = operatingSite.State,
        ZipCode = operatingSite.ZipCode,
        Plus4 = operatingSite.Plus4,
        InviteUserId = operatingSite.InviteUserId,
        InviteDate = operatingSite.InviteDate,
        AwardedMsys = operatingSite.AwardedMsys,
        LivingAllowanceMsys = operatingSite.LivingAllowanceMsys,
        NonLivingAllowanceMsys = operatingSite.NonLivingAllowanceMsys
    };

    public List<OperatingSiteResponse> Map(List<OperatingSite> operatingSiteList) =>
       MapperUtils.MapList<OperatingSite, OperatingSiteResponse>(
                           operatingSiteList,
                           o => new OperatingSiteResponse
                           {
                               Id = o.Id,
                               ProgramYear = o.ProgramYear,
                               Active = o.Active,
                               OperatingSiteName = o.OperatingSiteName,
                               ContactName = o.ContactName,
                               EmailAddress = o.EmailAddress,
                               PhoneNumber = o.PhoneNumber,
                               StreetAddress = o.StreetAddress,
                               StreetAddress2 = o.StreetAddress2,
                               City = o.City,
                               State = o.State,
                               ZipCode = o.ZipCode,
                               Plus4 = o.Plus4,
                               InviteUserId = o.InviteUserId,
                               InviteDate = o.InviteDate,
                               AwardedMsys = o.AwardedMsys,
                               LivingAllowanceMsys = o.LivingAllowanceMsys,
                               NonLivingAllowanceMsys = o.NonLivingAllowanceMsys
                           });

    public AccessResponse? Map(Access? access) => access == null ? null : new()
    {
        Id = access.Id,
        AccessName = access.AccessName,
        AccessLevel = access.AccessLevel,
        AccessType = access.AccessType,
        Description = access.Description
    };

    public UserListResponse? Map(UserList? userList) => userList == null ? null : new()
    {
        OrgCode = userList.OrgCode,
        Users = MapperUtils.MapList<User, UserResponse>(
            userList.Users, user => new UserResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                MiddleName = user.MiddleName,
                PreferredName = user.PreferredName,
                UserName = user.UserName,
                ExternalAccountId = user.ExternalAccountId,
                DateOfBirth = user.DateOfBirth,
                Pronouns = user.Pronouns,
                Suffix = user.Suffix,
                Prefix = user.Prefix,
                OrgCode = user.OrgCode,
                EncryptedSocialSecurityNumber = user.EncryptedSocialSecurityNumber,
                PPIUpdateNote = user.PPIUpdateNote,
                CitzenShipStatus = (global::AmeriCorps.Users.Models.CitizenshipStatusRequestModel)user.CitzenShipStatus,
                CountryOfBirth = MapperUtils.MapList<CountryOfBirth, CountryOfBirthRequestModel>(
                    user.CountryOfBirth, c =>
                        new CountryOfBirthRequestModel
                        {
                            BirthCountry = c.BirthCountry
                        }),
                StateOfBirth = MapperUtils.MapList<StateOfBirth, StateOfBirthRequestModel>(
                    user.StateOfBirth, c =>
                        new StateOfBirthRequestModel
                        {
                            BirthState = c.BirthState
                        }),
                CityOfBirth = MapperUtils.MapList<CityOfBirth, CityOfBirthRequestModel>(
                    user.CityOfBirth, c =>
                        new CityOfBirthRequestModel
                        {
                            BirthCity = c.BirthCity
                        }),
                DateOfBirths = MapperUtils.MapList<DateOfBirth, DateOfBirthRequestModel>(
                    user.DateOfBirths, d =>
                        new DateOfBirthRequestModel
                        {
                            BirthDate = d.BirthDate
                        }),
                EncryptedSocialSecurityNumbers = MapperUtils.MapList<EncryptedSocialSecurityNumber, EncryptedSocialSecurityNumberRequestModel>(
                    user.EncryptedSocialSecurityNumbers, d =>
                        new EncryptedSocialSecurityNumberRequestModel
                        {
                            SociaSecurityNumber = d.SociaSecurityNumber
                        }),
                UserAccountStatus = (AccountStatusRequestModel)user.UserAccountStatus,
                InviteUserId = user.InviteUserId,
                InviteDate = user.InviteDate,
                SocialSecurityVerification = Map(user.SocialSecurityVerification),
                Attributes = MapperUtils.MapList<AmeriCorps.Users.Data.Core.Attribute, AttributeRequestModel>(
                                user.Attributes,
                                a => new AttributeRequestModel
                                {
                                    Id = a.Id,
                                    Type = a.Type,
                                    Value = a.Value
                                }),

                Languages = MapperUtils.MapList<Language, LanguageRequestModel>(
                                user.Languages, l =>
                                new LanguageRequestModel
                                {
                                    Id = l.Id,
                                    PickListId = l.PickListId,
                                    IsPrimary = l.IsPrimary,
                                    SpeakingAbility = l.SpeakingAbility,
                                    WritingAbility = l.WritingAbility
                                }),

                Addresses = MapperUtils.MapList<Address, AddressRequestModel>(
                                user.Addresses, a =>
                                new AddressRequestModel
                                {
                                    Id = a.Id,
                                    IsForeign = a.IsForeign,
                                    Type = a.Type,
                                    Street1 = a.Street1,
                                    Street2 = a.Street2,
                                    City = a.City,
                                    State = a.State,
                                    Country = a.Country,
                                    ZipCode = a.ZipCode,
                                    MovingWithinSixMonths = a.MovingWithinSixMonths
                                }),

                Education = MapperUtils.MapList<Education, EducationRequestModel>(
                                user.Education, e =>
                                new EducationRequestModel
                                {
                                    Id = e.Id,
                                    Level = e.Level,
                                    MajorAreaOfStudy = e.MajorAreaOfStudy,
                                    Institution = e.Institution,
                                    City = e.City,
                                    State = e.State,
                                    DateAttendedFrom = e.DateAttendedFrom,
                                    DateAttendedTo = e.DateAttendedTo,
                                    DegreeTypePursued = e.DegreeTypePursued,
                                    DegreeCompleted = e.DegreeCompleted
                                }),

                Skills = MapperUtils.MapList<Skill, SkillRequestModel>(
                                user.Skills, s =>
                                new SkillRequestModel
                                {
                                    Id = s.Id,
                                    PickListId = s.PickListId
                                }),

                MilitaryService = MapperUtils.MapList<MilitaryService, MilitaryServiceRequestModel>(
                                user.MilitaryService, s =>
                                new MilitaryServiceRequestModel
                                {
                                    Id = s.Id,
                                    PickListId = s.PickListId
                                }),

                SavedSearches = MapperUtils.MapList<SavedSearch, SavedSearchRequestModel>(
                                    user.SavedSearches, s => new SavedSearchRequestModel
                                    {
                                        Id = s.Id,
                                        UserId = s.UserId,
                                        Name = s.Name,
                                        Filters = s.Filters,
                                        NotificationsOn = s.NotificationsOn
                                    }),

                References = MapperUtils.MapList<Reference, ReferenceRequestModel>(
                            user.References, r => new ReferenceRequestModel
                            {
                                TypeId = r.TypeId,
                                Relationship = r.Relationship,
                                RelationshipLength = r.RelationshipLength,
                                ContactName = r.ContactName,
                                Email = r.Email,
                                Phone = r.Phone,
                                Address = r.Address,
                                Company = r.Company,
                                Position = r.Position,
                                Notes = r.Notes,
                                CanContact = r.CanContact,
                                Contacted = r.Contacted,
                                DateContacted = r.DateContacted
                            }),

                Relatives = MapperUtils.MapList<Relative, RelativeRequestModel>(
                                user.Relatives, r =>
                                new RelativeRequestModel
                                {
                                    Id = r.Id,
                                    Relationship = r.Relationship,
                                    HighestEducationLevel = r.HighestEducationLevel,
                                    AnnualIncome = r.AnnualIncome
                                }),

                CommunicationMethods = MapperUtils.MapList<CommunicationMethod,
                                                CommunicationMethodRequestModel>(
                                user.CommunicationMethods, cm =>
                                new CommunicationMethodRequestModel
                                {
                                    Id = cm.Id,
                                    Type = cm.Type,
                                    Value = cm.Value,
                                    IsPreferred = cm.IsPreferred
                                }),

                Collection = MapperUtils.MapList<Collection, CollectionRequestModel>(
                    user.Collection, c =>
                        new CollectionRequestModel()
                        {
                            UserId = c.UserId,
                            Type = c.Type,
                            ListingId = c.ListingId,
                        }),

                UserRoles = MapperUtils.MapList<UserRole, UserRoleRequestModel>(
                    user.Roles, r =>
                        new UserRoleRequestModel()
                        {
                            //Id = r.Id,
                            RoleName = r.RoleName,
                            FunctionalName = r.FunctionalName
                        }),

                UserProjects = MapperUtils.MapList<UserProject, UserProjectRequestModel>(
                    user.UserProjects, p =>
                        new UserProjectRequestModel()
                        {
                            ProjectName = p.ProjectName,
                            ProjectCode = p.ProjectCode,
                            ProjectType = p.ProjectType,
                            ProjectOrg = p.ProjectOrg,
                            Active = p.Active,
                            ProjectRoles = Map(p.ProjectRoles),
                            ProjectAccess = Map(p.ProjectAccess)
                        }),
                DirectDeposits = MapperUtils.MapList<DirectDeposit, DirectDepositRequestModel>(
                    user.DirectDeposits, d =>
                        new DirectDepositRequestModel()
                        {
                            AccountType = (Models.AccountType)d.AccountType,
                            InstitutionName = d.InstitutionName,
                            AchRoutingNumber = d.AchRoutingNumber,
                            ReEnterAchRoutingNumber = d.ReEnterAchRoutingNumber,
                            AccountNumber = d.AccountNumber,
                            ReEnterAccountNumber = d.ReEnterAccountNumber,
                            MailByPaycheck = d.MailByPaycheck
                        }),
                TaxWithHoldings = MapperUtils.MapList<TaxWithHolding, TaxWithHoldingRequestModel>(
                    user.TaxWithHoldings, t =>
                        new TaxWithHoldingRequestModel()
                        {
                            TaxWithHoldingType = (Models.TaxWithHoldingType)t.TaxWithHoldingType,
                            AdditionalWithHoldings = t.AdditionalWithHoldings,
                            AdditionalWithHoldings2 = t.AdditionalWithHoldings2,
                            DependentsUnder17 = t.DependentsUnder17,
                            DependentsOver17 = t.DependentsOver17,
                            OtherIncome = t.OtherIncome,
                            Deductions = t.Deductions,
                            ExtraWithHoldingAmount = t.ExtraWithHoldingAmount,
                            ModifiedDate = t.ModifiedDate
                        })
            }
        )
    };

    public SocialSecurityVerificationResponse? Map(SocialSecurityVerification? status) => status == null ? null : new()
    {
        Id = status.Id,
        UserId = status.UserId,
        CitizenshipStatus = (VerificationStatusResponse)status.CitizenshipStatus,
        SocialSecurityStatus = (VerificationStatusResponse)status.SocialSecurityStatus,
        VerificationCode = status.VerificationCode,
        CitizenshipCode = status.CitizenshipCode,
        ProcessStartDate = status.ProcessStartDate,
        CitizenshipUpdatedDate = status.CitizenshipUpdatedDate,
        SocialSecurityUpdatedDate = status.SocialSecurityUpdatedDate,
        SubmitCount = status.SubmitCount,
        LastSubmitUser = status.LastSubmitUser
    };

}