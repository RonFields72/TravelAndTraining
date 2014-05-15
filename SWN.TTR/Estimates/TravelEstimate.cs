using SWN.TTR.Common;
using System;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using SWN.TTR.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation;

namespace SWN.TTR.Estimates
{
    /// <summary>
    /// 
    /// </summary>
    [HasSelfValidation()]
    public class TravelEstimate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TravelEstimate"/> class.
        /// </summary>
        public TravelEstimate()
        {
            Initialize();
        }

        private void Initialize()
        {
            this.destinationCity = string.Empty;
            this.destinationState = string.Empty;            
            this.HasFlightArrangements = false;
            this.HasGroundArrangements = false;
            this.HasLodgingArrangements = false;
            this.FlightArrangements = new Airfare();
            this.GroundArrangements = new Ground();
            this.LodgingArrangements = new Lodging();
        }

        #region Properties

        private string destinationCity;
              
        [StringLengthValidator(0, 50,
            Ruleset = "Save",
            Tag = "travel destination city",
            MessageTemplateResourceName = "InvalidStringLengthMessage",
            MessageTemplateResourceType = typeof(Resources))]
        [StringLengthValidator(1, RangeBoundaryType.Inclusive, 0, RangeBoundaryType.Ignore,
            Ruleset = "Submit",
            Tag = "travel destination city",
            MessageTemplateResourceName = "RequiredFieldMessage",
            MessageTemplateResourceType = typeof(Resources))]
        public string DestinationCity
        {
            get { return destinationCity; }
            set { destinationCity = value.NullToEmptyString(); }
        }

        private string destinationState;

        [StringLengthValidator(0, 2,
            Ruleset = "Save",
            Tag = "travel destination state",
            MessageTemplateResourceName = "InvalidStringLengthMessage",
            MessageTemplateResourceType = typeof(Resources))]
        [StringLengthValidator(2, RangeBoundaryType.Inclusive, 2, RangeBoundaryType.Inclusive,
            Ruleset = "Submit",
            Tag = "travel destination state",
            MessageTemplateResourceName = "RequiredFieldMessage",
            MessageTemplateResourceType = typeof(Resources))]
        public string DestinationState
        {
            get { return destinationState; }
            set { destinationState = value.NullToEmptyString(); }
        }        

        public decimal TotalCost
        {
            get
            {
                return this.FlightArrangements.TotalCost +
                    this.GroundArrangements.TotalCost +
                    this.LodgingArrangements.TotalCost;
            }
        }

        public bool HasFlightArrangements { get; set; }

        public bool HasGroundArrangements { get; set; }

        public bool HasLodgingArrangements { get; set; }

        public Airfare FlightArrangements { get; internal set; }

        public Ground GroundArrangements { get; internal set; }

        public Lodging LodgingArrangements { get; internal set; }

        #endregion

        #region Validation

        [SelfValidation(Ruleset = "Save")]
        public void DoValidateSave(ValidationResults results)
        {
             results.AddAllResults(
                    Microsoft.Practices.EnterpriseLibrary.Validation.Validation.Validate<Airfare>(this.FlightArrangements, "Save"));
             results.AddAllResults(
                     Microsoft.Practices.EnterpriseLibrary.Validation.Validation.Validate<Ground>(this.GroundArrangements, "Save"));
             results.AddAllResults(
                     Microsoft.Practices.EnterpriseLibrary.Validation.Validation.Validate<Lodging>(this.LodgingArrangements, "Save"));
        }

        [SelfValidation(Ruleset = "Submit")]
        public void DoValidateSubmit(ValidationResults results)
        {
            if (!this.HasFlightArrangements && !this.HasGroundArrangements && !this.HasLodgingArrangements)
                results.AddResult(new ValidationResult("At least one travel arrangement is required.",
                    typeof(TravelEstimate), "", "travel arrangement", null));
            else
            {
                if (this.HasFlightArrangements)
                {
                    results.AddAllResults(
                        Microsoft.Practices.EnterpriseLibrary.Validation.Validation.Validate<Airfare>(this.FlightArrangements, "Submit"));
                }
                if (this.HasGroundArrangements)
                {
                    results.AddAllResults(
                         Microsoft.Practices.EnterpriseLibrary.Validation.Validation.Validate<Ground>(this.GroundArrangements, "Submit"));
                }
                if (this.HasLodgingArrangements)
                {
                    results.AddAllResults(
                         Microsoft.Practices.EnterpriseLibrary.Validation.Validation.Validate<Lodging>(this.LodgingArrangements, "Submit"));
                }
            }
        }

        #endregion
    }
}