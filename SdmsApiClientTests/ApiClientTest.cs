using MsgSdmsClient;
using NUnit.Framework;
using System;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SdmsApiClientTests
{
  public class Tests
  {
    private class Token
    {
      public string AccessToken { get; set; }
      public int ExpiresIn { get; set; }
    }


    private static HttpClient s_TokenClient;

    /// <summary>
    /// Gets the https client to retrieve the token to use for SDMS API calls.
    /// </summary>
    private static HttpClient TokenClient
    {
      get
      {
        if (s_TokenClient == null)
        {
          var tokenUrl = "https://sentry.madesmart.nl/policies/";
          s_TokenClient = new HttpClient() { BaseAddress = new Uri(tokenUrl) };
        }

        return s_TokenClient;
      }
    }

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task GetAllRegionsTest()
    {
      HttpClient sdmsClient = await GetSdmsClient();

      var regionsClient = new RegionClient(sdmsClient);
      var regions = await regionsClient.GetAsync("1.0");
      Assert.IsTrue(regions.Count > 0);

      var mumbaiRegion = regions.FirstOrDefault(r => r.RegionId == new Guid("5b1fbdf4-55a8-4820-936c-2828bf25b304"));
    }

    private static async Task<HttpClient> GetSdmsClient()
    {
      var sdmsClient = new HttpClient();

      var token = await GetToken("survey_sdmsapi", "$dms@pi4VanOordSurv3y");
      if (token == null)
        throw new WarningException("Unable to get a (new) token for the SDMS API.");

      sdmsClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.AccessToken);
      return sdmsClient;
    }

    [Test]
    public async Task GetItemTest()
    {
      var sdmsClient = await GetSdmsClient();

      var itemsClient = new PublishedItemClient(sdmsClient);
      var items = await itemsClient.GetByIdsAsync(new Guid[] { new Guid("7be42924-8b18-41f3-8d43-545e737a12e4") }, "1.0");
      Assert.IsTrue(items.Count > 0);
    }

    /// <summary>
    /// Gets a token from MSG sentry based on the supplied username and password to be able to make SDMS Web Api calls.
    /// </summary>
    /// <param name="userName">The user name to use for retrieving the token.</param>
    /// <param name="password">The password to use for retrieving the token.</param>
    /// <returns>A token to make SDMS Web Api calls. In case no token could be retrieved then an empty token is returned and this prevents making SDMS Web Api calls.</returns>
    private static async Task<Token> GetToken(string userName, string password)
    {
      var tokenClient = TokenClient;

      var request = new HttpRequestMessage(HttpMethod.Get, "token");
      request.Headers.Add("username", userName);
      request.Headers.Add("password", password);

      using HttpResponseMessage response = await tokenClient.SendAsync(request);
      if (!response.IsSuccessStatusCode)
        return new Token();

      return await response.Content.ReadFromJsonAsync<Token>();
    }

  }
}