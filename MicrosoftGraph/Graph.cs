using Azure.Identity;
using Microsoft.Graph;
namespace MicrosoftGraph
{
    public static class Graph
    {
        public static GraphServiceClient GenerateClient()
        {
            var clientSecret = "YOUR_APP_CLIENT_SECRET";
            var clientId = "YOUR_APP_AZURE_CLIENT_ID";
            var tenantId = "YOUR_AZURE_TENANT_ID";

            // The client credentials flow requires that you request the
            // /.default scope, and preconfigure your permissions on the
            // app registration in Azure. An administrator must grant consent
            // to those permissions beforehand.
            var scopes = new[] { "https://graph.microsoft.com/.default" };
            var options = new TokenCredentialOptions { AuthorityHost = AzureAuthorityHosts.AzurePublicCloud };             // https://learn.microsoft.com/dotnet/api/azure.identity.clientsecretcredential
            var clientSecretCredential = new ClientSecretCredential(
                                tenantId,
                                clientId,
                                clientSecret,
                                options);
            var graphClient = new GraphServiceClient(clientSecretCredential, scopes);
            return graphClient;
        }

        public static async Task<bool> UserBelongsToGroup(this GraphServiceClient client,  string email, string groupID)
        {
            var users = await client.Users.Request().Filter($"userPrincipalName eq '{email}'").GetAsync();
            var user = users.FirstOrDefault();
            if (user == null)
            {
                return false;
            }

            var memberOf = await client.Users[user.Id].MemberOf.Request().GetAsync();
            var groupIds = memberOf.Select(c => c.Id)?.ToList() ?? new List<string>();
            while (memberOf.NextPageRequest != null)
            {
                memberOf = await memberOf.NextPageRequest.GetAsync();
                groupIds.AddRange(memberOf.Select(c => c.Id).ToList());
            }
            return groupIds.Contains(groupID);
        }
    }
}
