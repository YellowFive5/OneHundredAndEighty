#region Usings

using System;

#endregion

namespace OneHundredAndEightyCore.Common
{
    public class CamNotFoundException : Exception
    {
        public CamNotFoundException(int camNumber, string failedCamId) 
            : base($"Camera with specified id '{failedCamId}' not found in connected camera devices for Camera#{camNumber}")
        {
        }
    }
}