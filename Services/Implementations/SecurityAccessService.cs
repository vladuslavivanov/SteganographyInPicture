using System.Security.Principal;

namespace SteganographyInPicture.Services.Implementations;

internal static class SecurityAccessService
{
    public static bool IsUserAdministrator()
    {
        WindowsIdentity identity = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal = new WindowsPrincipal(identity);
        //return true;
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }
}
