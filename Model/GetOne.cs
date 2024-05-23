using System.Text.Json.Serialization;
using FrejaOrgId.Converters;

namespace FrejaOrgId.Model
{
    public record GetOneRequest(string OrgIdRef);

    public record GetOneResponse(
        string OrgIdRef,
        [property: JsonConverter(typeof(UpperCaseEnumConverter<TransactionStatus>))]
        TransactionStatus Status,
        [property: JsonConverter(typeof(GetOneDetailsConverter))]
        GetOneDetailsBase Details
    );

    internal record GetOneIntermediateResponse(
        string OrgIdRef,
        [property: JsonConverter(typeof(UpperCaseEnumConverter<TransactionStatus>))]
        TransactionStatus Status,
        string Details
    );

    public abstract record GetOneDetailsBase();

    public record StringGetOneDetails(string Value) : GetOneDetailsBase;

    public record ApprovedGetOneDetails : GetOneDetailsBase
    {
        public string OrgIdRef { get; init; }
        [JsonConverter(typeof(UpperCaseEnumConverter<TransactionStatus>))]
        public TransactionStatus Status { get; init; }
        [JsonConverter(typeof(UpperCaseEnumConverter<UserInfoType>))]
        public UserInfoType UserInfoType { get; init; }
        [JsonConverter(typeof(UserInfoJsonConverter))]
        public UserInfoBase UserInfo { get; init; }
        [JsonConverter(typeof(UpperCaseEnumConverter<MinRegistrationLevel>))]
        public MinRegistrationLevel MinRegistrationLevel { get; init; }
        [JsonConverter(typeof(UnixTimeConverter))]
        public DateTime Timestamp { get; init; }
        [JsonConverter(typeof(UpperCaseEnumConverter<SignatureType>))]
        public SignatureType SignatureType { get; init; }
        public SignatureData SignatureData { get; init; }
        public string? OriginalJws { get; private set; }

        public ApprovedGetOneDetails(string orgIdRef, TransactionStatus status, UserInfoType userInfoType, UserInfoBase userInfo, MinRegistrationLevel minRegistrationLevel, DateTime timestamp, SignatureType signatureType, SignatureData signatureData)
        {
            OrgIdRef = orgIdRef;
            Status = status;
            UserInfoType = userInfoType;
            UserInfo = userInfo;
            MinRegistrationLevel = minRegistrationLevel;
            Timestamp = timestamp;
            SignatureType = signatureType;
            SignatureData = signatureData;
        }

        public void SetOriginalJws(string jws)
        {
            if (OriginalJws != null)
            {
                throw new InvalidOperationException("OriginalJws already has a value.");
            }
            OriginalJws = jws;
        }
    }

    public record SignatureData(string UserSignature, string CertificateStatus);

    public enum TransactionStatus
    {
        Started, // The transaction has been started but not yet delivered to the user's Freja eID application.
        Delivered_To_Mobile, // The Freja eID app has received the transaction.
        Canceled, // The end user declined the 'Add Organisation ID' request.
        Rp_Canceled, // The 'Add Organisation ID' request was sent to the user but cancelled by the Relying Party before the user could respond.
        Expired, // The 'Add Organisation ID' request was not approved by the user within the set time frame.
        Approved // The end user has approved the 'Add Organisation ID' request.
    };

    public enum SignatureType
    {
        Simple,
        Extended,
        Xml_Minameddelanden
    }
}
