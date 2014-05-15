using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using System;
using System.Collections.Generic;
using SWN.TTR.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using SWN.TTR.Properties;

namespace SWN.TTR.Estimates
{
    [HasSelfValidation()]
    public class Ground
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Ground"/> class.
        /// </summary>
        public Ground()
        {
            Initialize();
        }

        private void Initialize()
        {
            this.Modes = new GroundModes(new List<string>());
            this.TotalCost = BizObjectValidator.DEFAULT_COST;
        }

        public GroundModes Modes { get; internal set; }

        [RangeValidator(typeof(decimal), "1", RangeBoundaryType.Inclusive,
            "0", RangeBoundaryType.Ignore,
            Ruleset = "Submit",
            Tag = "ground estimated cost",
            MessageTemplateResourceName = "RequiredFieldMessage",
            MessageTemplateResourceType = typeof(Resources))]
        public decimal TotalCost { get; set; }

        #region Validation

        [SelfValidation(Ruleset = "Submit")]
        public void DoValidateSubmit(ValidationResults results)
        {
            if (this.Modes.Count == 0)
                results.AddResult(new ValidationResult("At least one ground transportation mode is required.",
                    typeof(Ground),
                    "", "", null));
        }

        [SelfValidation(Ruleset = "Save")]
        public void DoValidateSave(ValidationResults results)
        {
            foreach (string mode in this.Modes)
            {
                if (mode.Length != 4)
                    results.AddResult(new ValidationResult(string.Format("{0} is not a valid ground mode.", mode),
                        typeof(Ground),
                        "", "", null));
            }
        }

        #endregion
    }
}