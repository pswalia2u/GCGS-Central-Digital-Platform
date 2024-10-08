using CO.CDP.DataSharing.WebApiClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CO.CDP.OrganisationApp.Pages.ShareInformation;

public class ShareCodeConfirmationModel(
    IDataSharingClient dataSharingClient) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public Guid OrganisationId { get; set; }

    [BindProperty(SupportsGet = true)]
    public Guid FormId { get; set; }

    [BindProperty(SupportsGet = true)]
    public Guid SectionId { get; set; }

    public string? ShareCode { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            var sharingDataDetails = await dataSharingClient.CreateSharedDataAsync(new ShareRequest(FormId, OrganisationId));
            ShareCode = sharingDataDetails.ShareCode;
        }
        catch (ApiException ex) when (ex.StatusCode == 404)
        {
            return Redirect("/page-not-found");
        }
        return Page();
    }
}