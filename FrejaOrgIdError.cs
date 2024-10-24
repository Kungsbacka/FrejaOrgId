﻿using System.Text;

namespace FrejaOrgId
{
    public record FrejaOrgIdApiError(int Code, string Message)
    {
        public override string? ToString()
        {
            StringBuilder sb = new();
            sb.Append(Code);
            sb.Append(": ");
            sb.Append(Message);
            return sb.ToString();
        }
    }
}
