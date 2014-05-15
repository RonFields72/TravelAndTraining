using SWN.TTR.Common;
using System;
using SWN.TTR.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using SWN.TTR.Properties;

namespace SWN.TTR.Estimates
{    
    public class TrainingEstimate
    {
        public TrainingEstimate()
        {
            Initialize();
        }

        private void Initialize()
        {
            this.trainingName = "";
            this.StartingDate = BizObjectValidator.DEFAULT_DATETIME;
            this.EndingDate = BizObjectValidator.DEFAULT_DATETIME;
            this.TotalCost = BizObjectValidator.DEFAULT_COST;
        }

        private string trainingName;
                
        [StringLengthValidator(0, 100,
            Ruleset = "Save",
            Tag = "training name",
            MessageTemplateResourceName = "InvalidStringLengthMessage",
            MessageTemplateResourceType = typeof(Resources))]
        [NotNullValidator(Ruleset = "Save",
            Tag = "training name",
            MessageTemplateResourceName = "InvalidStringLengthMessage",
            MessageTemplateResourceType = typeof(Resources))]
        [StringLengthValidator(1, RangeBoundaryType.Inclusive, 0, RangeBoundaryType.Ignore,
            Ruleset = "Submit",
            Tag = "training name",
            MessageTemplateResourceName = "RequiredFieldMessage",
            MessageTemplateResourceType = typeof(Resources))]
        public string TrainingName
        {
            get { return trainingName; }
            set { trainingName = value.NullToEmptyString(); }
        }
        
        [DateTimeRangeValidator(BizObjectValidator.MIN_DATETIME, RangeBoundaryType.Inclusive,
            BizObjectValidator.MAX_DATETIME, RangeBoundaryType.Inclusive,
            Ruleset = "Save",
            Tag = "training start date",
            MessageTemplateResourceName = "InvalidDateRangeMessage",
            MessageTemplateResourceType = typeof(Resources))]
        public DateTime StartingDate { get; set; }

        [DateTimeRangeValidator(BizObjectValidator.MIN_DATETIME, RangeBoundaryType.Inclusive,
            BizObjectValidator.MAX_DATETIME, RangeBoundaryType.Inclusive,
            Ruleset = "Save",
            Tag = "training end date",
            MessageTemplateResourceName = "InvalidDateRangeMessage",
            MessageTemplateResourceType = typeof(Resources))]
        [PropertyComparisonValidator("StartingDate", ComparisonOperator.GreaterThanEqual,
            Ruleset = "Submit",
            Tag = "training end date",
            MessageTemplateResourceName = "EndDateInvalidMessage",
            MessageTemplateResourceType = typeof(Resources))]
        public DateTime EndingDate { get; set; }

        [RangeValidator(typeof(decimal), "1", RangeBoundaryType.Inclusive,
            "0", RangeBoundaryType.Ignore,
            Ruleset = "Submit",
            Tag = "training estimated cost",
            MessageTemplateResourceName = "RequiredFieldMessage",
            MessageTemplateResourceType = typeof(Resources))]
        public decimal TotalCost { get; set; }
    }
}