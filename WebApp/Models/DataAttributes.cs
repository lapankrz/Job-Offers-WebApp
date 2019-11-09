using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class IsDateAfterAttribute : ValidationAttribute
    {
        private readonly string testedPropertyName;
        public IsDateAfterAttribute(string testedPropertyName)
        {
            this.testedPropertyName = testedPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //value = DateTime.Now;
            var propertyTestedInfo = validationContext.ObjectType.GetProperty(this.testedPropertyName);
            if (propertyTestedInfo == null)
            {
                return new ValidationResult(string.Format("unknown property {0}", this.testedPropertyName));
            }

            var propertyTestedValue = propertyTestedInfo.GetValue(validationContext.ObjectInstance, null);
            propertyTestedValue = DateTime.Now;
            if (value == null || !(value is DateTime))
            {
                return ValidationResult.Success;
            }

            if (propertyTestedValue == null || !(propertyTestedValue is DateTime))
            {
                return ValidationResult.Success;
            }

            // Compare values
            if ((DateTime)value >= (DateTime)propertyTestedValue)
            {
                if (value == propertyTestedValue)
                {
                    return ValidationResult.Success;
                }
                else if ((DateTime)value > (DateTime)propertyTestedValue)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(GetErrorMessage());
        }

        public string GetErrorMessage()
        {
            return "Valid until date cannot be in the past!";
        }
    }

    public class SalaryToIsBiggerAttribute : ValidationAttribute
    {
        private readonly string testedPropertyName;
        public SalaryToIsBiggerAttribute(string testedPropertyName)
        {
            this.testedPropertyName = testedPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var propertyTestedInfo = validationContext.ObjectType.GetProperty(this.testedPropertyName);
            if (propertyTestedInfo == null)
            {
                return new ValidationResult(string.Format("unknown property {0}", this.testedPropertyName));
            }

            var propertyTestedValue = propertyTestedInfo.GetValue(validationContext.ObjectInstance, null);
            if (value == null || !(value is int))
            {
                return new ValidationResult("Input correct values");
            }

            if (propertyTestedValue == null || !(propertyTestedValue is int))
            {
                return new ValidationResult("Input correct values");
            }

            if ((int)propertyTestedValue <= 0 || (int)value <= 0)
            {
                return new ValidationResult("Salary values must be greater than 0!");
            }

            // Compare values
            if ((int)value >= (int)propertyTestedValue)
            {
                if (value == propertyTestedValue)
                {
                    return ValidationResult.Success;
                }
                else if ((int)value > (int)propertyTestedValue)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(GetErrorMessage());
        }

        public string GetErrorMessage()
        {
            return "Salary to must be greater than salary from!";
        }
    }
}
