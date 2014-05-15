using System;
using System.Collections.Generic;

namespace SWN.TTR.Validation
{
    public class BizObjectValidator
    {
        #region Fields

        public const string MIN_DATETIME = "1900-01-01T00:00:00";
        public const string MAX_DATETIME = "9999-12-31T00:00:00";
        public static readonly DateTime DEFAULT_DATETIME = new DateTime(1900, 1, 1);
        public const decimal DEFAULT_COST = 0M;
        
        #endregion        
    }
}