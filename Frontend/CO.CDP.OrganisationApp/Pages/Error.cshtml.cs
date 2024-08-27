using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CO.CDP.OrganisationApp.Pages;

public class ErrorModel : PageModel
{
static ErrorModel()
    { 
    throw new Exception("ErrorModel class loaded - throwing exception."); 
    }
}
