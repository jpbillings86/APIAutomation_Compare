using RestSharp;
using Newtonsoft.Json.Linq;
using Microsoft.VisualBasic;

namespace AuthRegression;

public class AuthRegressionTests
{
    private RestClient client;
    private readonly string valid_username = "admin";
    private readonly string valid_password = "password123";

    [SetUp]
    public void Setup()
    {
        client = new RestClient("https://restful-booker.herokuapp.com");
    }
    private RestResponse AuthCall(string? username, string? password)
    {
        var request = new RestRequest("/auth", Method.Post);
        request.AddJsonBody(new { username, password });
        return client.Execute(request);
    }

    [Test]
    public void Post_Auth_With_Valid_Credentials()
    {
        var response = AuthCall(valid_username, valid_password);

        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        JObject jsonResponse = JObject.Parse(response.Content);
        Assert.That(jsonResponse.ContainsKey("token"), Is.True);
    }

    [Test]
    public void Post_Auth_With_Invalid_Username_Credentials()
    {
        var response = AuthCall("doesnotexist", valid_password);

        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        JObject jsonResponse = JObject.Parse(response.Content);
        Assert.That(jsonResponse.ContainsKey("reason"), Is.True);
        Assert.That(jsonResponse["reason"].ToString(), Is.EqualTo("Bad credentials"));

    }
    
    [Test]
    public void Post_Auth_With_Invalid_Password_Credentials()
    {
        var response = AuthCall(valid_username, "badpassword");

        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        JObject jsonResponse = JObject.Parse(response.Content);
        Assert.That(jsonResponse.ContainsKey("reason"), Is.True);
        Assert.That(jsonResponse["reason"].ToString(), Is.EqualTo("Bad credentials"));

    }
    
    [Test]
    public void Post_Auth_With_No_Credentials()
    {
        var response = AuthCall(null, null);

        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        JObject jsonResponse = JObject.Parse(response.Content);
        Assert.That(jsonResponse.ContainsKey("reason"), Is.True);
        Assert.That(jsonResponse["reason"].ToString(), Is.EqualTo("Bad credentials"));

    }
    
    [Test]
    public void Post_Auth_With_Missing_Username_Credentials()
    {
        var response = AuthCall(null, valid_password);

        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        JObject jsonResponse = JObject.Parse(response.Content);
        Assert.That(jsonResponse.ContainsKey("reason"), Is.True);
        Assert.That(jsonResponse["reason"].ToString(), Is.EqualTo("Bad credentials"));

    }
    
    [Test]
    public void Post_Auth_With_Missing_Password_Credentials()
    {
        var response = AuthCall(valid_username, null);

        Assert.That(response.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.OK));
        JObject jsonResponse = JObject.Parse(response.Content);
        Assert.That(jsonResponse.ContainsKey("reason"), Is.True);
        Assert.That(jsonResponse["reason"].ToString(), Is.EqualTo("Bad credentials"));

    }
}