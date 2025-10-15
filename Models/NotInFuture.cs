// =============================================================================
// FILE: NotInFutureAttribute.cs
// PROJECT: SavingsClub
// =============================================================================
// Custom validation attribute ensuring that a given date value is not set
// in the future. Used for validation of payment and event dates.
// =============================================================================

using System.ComponentModel.DataAnnotations;

namespace SavingsClub.Models;

// =============================================================================
// CLASS: NotInFutureAttribute
// =============================================================================
// Extends ValidationAttribute to restrict date values to today or earlier.
// =============================================================================
public class NotInFutureAttribute : ValidationAttribute
{
    // -------------------------------------------------------------------------
    // METHOD: IsValid
    // -------------------------------------------------------------------------
    // Validates whether the provided value is a date not greater than today.
    // -------------------------------------------------------------------------
    public override bool IsValid(object? value)
    {
        if (value is null)
            return true;

        if (value is DateTime dt)
            return dt.Date <= DateTime.Today;

        return true;
    }


    // -------------------------------------------------------------------------
    // METHOD: FormatErrorMessage
    // -------------------------------------------------------------------------
    // Returns the error message shown when a future date is entered.
    // -------------------------------------------------------------------------
    public override string FormatErrorMessage(string value)
    {
        return $"{value} must not be set in the future.";
    }
}