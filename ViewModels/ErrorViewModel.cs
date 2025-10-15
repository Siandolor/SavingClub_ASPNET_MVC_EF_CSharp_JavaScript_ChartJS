// =============================================================================
// FILE: ErrorViewModel.cs
// PROJECT: SavingsClub
// =============================================================================
// Represents error information passed to the error view. Used by the global
// error handling pipeline to display diagnostic information in a user-friendly
// manner.
// =============================================================================

namespace SavingsClub.Models;

// =============================================================================
// CLASS: ErrorViewModel
// =============================================================================
// Provides the error request identifier and logic for determining whether
// it should be displayed in the view.
// =============================================================================
public class ErrorViewModel
{
    // -------------------------------------------------------------------------
    // PROPERTIES
    // -------------------------------------------------------------------------

    // Unique request identifier used to trace specific errors.
    public string? RequestId { get; set; }

    // Indicates whether the RequestId should be shown in the UI.
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}