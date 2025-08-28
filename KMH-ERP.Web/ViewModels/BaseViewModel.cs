using System.ComponentModel.DataAnnotations.Schema;

namespace KMH_ERP.Web.ViewModels;

[NotMapped]
public class BaseViewModel
{
    // For error messages (validation, exceptions, etc.)
    public string? ErrorMessage { get; set; }

    // For success messages (create/update/delete, login, etc.)
    public string? SuccessMessage { get; set; }

    // To store multiple validation errors
    public List<string> ValidationErrors { get; set; } = new List<string>();

}
