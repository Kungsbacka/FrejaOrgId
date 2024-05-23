namespace FrejaOrgId.Model
{
    public abstract record UserInfoBase { }
    public record StringUserInfo(string Value) : UserInfoBase;
    public record SsnUserInfo(string Country, string Ssn) : UserInfoBase;
    public record AdditionalAttribute(string Key, string DisplayText, string Value);
    public enum UserInfoType { Phone, Email, Ssn, Inferred }
    public enum MinRegistrationLevel { Extended, Plus }
}
