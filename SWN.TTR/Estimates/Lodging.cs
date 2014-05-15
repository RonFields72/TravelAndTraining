using SWN.TTR.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using SWN.TTR.Properties;

namespace SWN.TTR.Estimates
{
    /// <summary>
    /// 
    /// </summary>
    public class Lodging
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Lodging"/> class.
        /// </summary>
        public Lodging()
        {
            Initialize();
        }

        private void Initialize()
        {
            this.NumberOfNights = 0;
            this.TotalCost = BizObjectValidator.DEFAULT_COST;
        }        
        
        [RangeValidator(1, RangeBoundaryType.Inclusive, 
            0, RangeBoundaryType.Ignore,
            Ruleset = "Submit",
            Tag = "lodging nights",
            MessageTemplateResourceName = "RequiredFieldMessage",
            MessageTemplateResourceType = typeof(Resources))]
        public int NumberOfNights { get; set; }

        [RangeValidator(typeof(decimal), "1", RangeBoundaryType.Inclusive,
            "0", RangeBoundaryType.Ignore,
            Ruleset = "Submit",
            Tag = "lodging estimated cost",
            MessageTemplateResourceName = "RequiredFieldMessage",
            MessageTemplateResourceType = typeof(Resources))]
        public decimal TotalCost { get; set; }
    }
}