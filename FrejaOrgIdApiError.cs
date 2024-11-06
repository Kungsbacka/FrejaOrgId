namespace FrejaOrgId;

public enum FrejaOrgIdApiError
{
    InvalidAdditionalAttributes = 4009,
    InvalidDisplayType = 4008,
    InvalidExpiryTime = 4003,
    InvalidMinRegistrationLevel = 1007,
    InvalidOrgIdIdentifier = 4000,
    InvalidOrgIdIdentifierName = 4005,
    InvalidOrgIdTitle = 4004,
    InvalidOrMissingUserInfo = 1002,
    InvalidOrMissingUserInfoType = 1001,
    InvalidOrgIdReference = 1100,
    JsonParseError = 1010,
    MethodNotAllowed = 1004,
    MissingOrgId = 4006,
    OrgIdIdentifierAlreadyInUse = 4002,
    ParameterNotAllowed = 1009,
    ServiceNotEnabled = 1005,
    UnknownRelyingParty = 1008,
    UserNotFound = 1012,
    UserNotFoundForOrgId = 4001
}
