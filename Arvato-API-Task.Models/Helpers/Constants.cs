using System;
using System.Collections.Generic;
using System.Text;

namespace Arvato_API_Task.Models.Helpers
{
    public static class Constants
    {
        public static readonly string REGEX_OWNER_NAME = @"^[a-zA-Z \-æøåäöü']+$";
        public static readonly string REGEX_CVV = @"^[0-9]+$";
    }
}
