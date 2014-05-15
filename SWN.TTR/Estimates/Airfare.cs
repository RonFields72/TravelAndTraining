using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using SWN.TTR.Properties;
using SWN.TTR.Validation;

namespace SWN.TTR.Estimates
{
    /// <summary>
    /// 
    /// </summary>
    [HasSelfValidation()]
    public class Airfare
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Airfare"/> class.
        /// </summary>
        public Airfare()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            this.DestinationMethod = "NONE";
            this.ReturnMethod = "NONE";
            this.TotalCost = BizObjectValidator.DEFAULT_COST;
        }

        [StringLengthValidator(0, 4,
            Ruleset = "Save",
            Tag = "air destination method",
            MessageTemplateResourceName = "InvalidStringLengthMessage",
            MessageTemplateResourceType = typeof(Resources))]
        [StringLengthValidator(4, RangeBoundaryType.Inclusive, 4, RangeBoundaryType.Inclusive,
            Ruleset = "Submit",
            Tag = "air destination method",
            MessageTemplateResourceName = "RequiredFieldMessage",
            MessageTemplateResourceType = typeof(Resources))]
        [DomainValidator("NONE",
            Ruleset = "Submit",
            Tag = "air return method",
            MessageTemplateResourceName = "RequiredFieldMessage",
            MessageTemplateResourceType = typeof(Resources),
            Negated = true)]
        public string DestinationMethod { get; set; }

        [StringLengthValidator(0, 4,
            Ruleset = "Save",
            Tag = "air return method",
            MessageTemplateResourceName = "InvalidStringLengthMessage",
            MessageTemplateResourceType = typeof(Resources))]
        [StringLengthValidator(4, RangeBoundaryType.Inclusive, 4, RangeBoundaryType.Inclusive,
            Ruleset = "Submit",
            Tag = "air return method",
            MessageTemplateResourceName = "RequiredFieldMessage",
            MessageTemplateResourceType = typeof(Resources))]
        [DomainValidator("NONE",
            Ruleset = "Submit",
            Tag = "air return method",
            MessageTemplateResourceName = "RequiredFieldMessage",
            MessageTemplateResourceType = typeof(Resources),
            Negated = true)]
        public string ReturnMethod { get; set; }

        [SelfValidation(Ruleset = "Submit")]
        public void DoValidateSubmit(ValidationResults results)
        {
            if (this.IsInterofficeOnly
                    && this.TotalCost != 0)
            {
                results.AddResult(new ValidationResult("The estimated cost of a flight must be $0 for interoffice flights.",
                    typeof(Airfare), "", "", null));
            }
            
            if (this.TotalCost <= 0M
                && !this.IsInterofficeOnly)
            {
                results.AddResult(new ValidationResult("The estimated cost of a flight is required.",
                    typeof(Airfare), "", "", null));
            }
        }

        public bool IsInterofficeOnly
        {
            get 
            {
                return (this.DestinationMethod == "COMP" && this.ReturnMethod == "COMP");            
            }            
        }	

        public decimal TotalCost { get; set; }
    }
}