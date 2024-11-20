﻿using FrejaOrgId.Converters;
using FrejaOrgId.Model.Shared;
using System.Text.Json.Serialization;

namespace FrejaOrgId.Model.GetOne;

public class ApprovedGetOneDetails : GetOneDetailsBase
{
    public string OrgIdRef { get; private set; }

    [JsonConverter(typeof(UpperCaseEnumConverter<TransactionStatus>))]
    public TransactionStatus Status { get; private set; }

    [JsonConverter(typeof(UpperCaseEnumConverter<UserInfoType>))]
    public UserInfoType UserInfoType { get; private set; }

    [JsonConverter(typeof(UserInfoJsonConverter))]
    public UserInfoBase UserInfo { get; private set; }

    [JsonConverter(typeof(UpperCaseEnumConverter<MinRegistrationLevel>))]
    public MinRegistrationLevel MinRegistrationLevel { get; private set; }

    [JsonConverter(typeof(UnixTimeConverter))]
    public DateTime Timestamp { get; private set; }

    [JsonConverter(typeof(UpperCaseEnumConverter<SignatureType>))]
    public SignatureType SignatureType { get; private set; }

    public SignatureData SignatureData { get; private set; }

    public string OriginalJws { get; private set; }

    [JsonConstructor]
    internal ApprovedGetOneDetails(string orgIdRef, TransactionStatus status, UserInfoType userInfoType,
        UserInfoBase userInfo, MinRegistrationLevel minRegistrationLevel, DateTime timestamp,
        SignatureType signatureType, SignatureData signatureData)
    {
        orgIdRef.ThrowIfNullOrEmpty(nameof(orgIdRef));
        userInfo.ThrowIfNull(nameof(userInfo));
        signatureData.ThrowIfNull(nameof(signatureData));

        OrgIdRef = orgIdRef;
        Status = status;
        UserInfoType = userInfoType;
        UserInfo = userInfo;
        MinRegistrationLevel = minRegistrationLevel;
        Timestamp = timestamp;
        SignatureType = signatureType;
        SignatureData = signatureData;
        OriginalJws = string.Empty;
    }

    internal void SetOriginalJws(string jws)
    {
        if (!string.IsNullOrEmpty(OriginalJws))
        {
            throw new InvalidOperationException("OriginalJws value is already set.");
        }

        OriginalJws = jws;
    }
}
