using RestSharp;
using System.Net;
using NUnit;
using NUnit.Framework;
using RestSharp.Serialization.Json;

namespace RestApiTest
{
	public class WebArchieveTests
	{
		const string BaseUrl = "https://archive.org";
		const string YoutubeUrl = "youtube.com";
		private RestClient Client { get; set; }

		public WebArchieveTests()
		{
			Client = new RestClient(BaseUrl);
		}

		[Test]
		public void Availabe_StatusCodeTest() // testing availability of the endpoint
		{
			// arrange
			RestRequest request = new RestRequest("wayback/available", Method.GET);

			// act
			IRestResponse response = Client.Execute(request);

			// assert
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
		}

		[Test]
		public void Available_UrlParameterIsMandatory()
		{
			// arrange
			string expectedErrorMessage = "Error: no url parameter";
			string expectedContentType = "text/html; charset=utf-8";
			RestRequest request = new RestRequest("wayback/available", Method.GET);

			// act
			IRestResponse response = Client.Execute(request);

			// assert
			Assert.That(response.ContentType, Is.EqualTo(expectedContentType));
			Assert.That(response.Content, Is.EqualTo(expectedErrorMessage));
		}

		[Test]
		public void Available_SuccesfulResultCOntentTypeJSON()
		{
			// arrange
			//RestClient client = new RestClient("https://archive.org/wayback/available");
			RestRequest request = new RestRequest("wayback/available", Method.GET);
			request.AddParameter("url", YoutubeUrl);
			// act
			IRestResponse response = Client.Execute(request);

			// assert
			Assert.That(response.ContentType, Is.EqualTo("application/json"));
		}

		[Test]
		public void Available_FakeUrl_ResponseOk()
		{
			// arrange
			RestRequest request = new RestRequest("wayback/available", Method.GET);
			request.AddParameter("url", "xxxxxx");

			// act
			IRestResponse response = Client.Execute(request);

			SnapshotResponse locationResponse =
				new JsonDeserializer().
				Deserialize<SnapshotResponse>(response);

			// assert
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
			Assert.That(locationResponse.ArchivedSnapshots.Closest, Is.Null);
		}

		[Test]
		public void TakeSnapshot_ResponseCreated()
		{
			// arrange
			var body = new { url = YoutubeUrl, annotation = new { id = "lst-ib", message = "Theres a microphone button in the searchbox" } };
			var client = new RestClient("https://pragma.archivelab.org/");
			var request = new RestRequest("", Method.POST);
			request.AddJsonBody(body);

			// act
			IRestResponse response = Client.Execute(request);

			//Assert
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

			RestRequest finalRequest = new RestRequest("wayback/available", Method.GET);
			request.AddParameter("url", YoutubeUrl);
			IRestResponse finalResponse = Client.Execute(request);
			Assert.That(finalResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
		}

		[Test]
		public void UpdateSnapshot_ResponseCreated()
		{
			// arrange
			var body = new { url = "ecampus.kpi.ua", annotation = new { id = "lst-ib", message = "UPDATEMESSAGE" } };
			var client = new RestClient("https://pragma.archivelab.org/");
			var request = new RestRequest("wayback/available", Method.PUT);
			request.AddJsonBody(body);

			// act
			IRestResponse response = Client.Execute(request);

			//Assert
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

			RestRequest finalRequest = new RestRequest("wayback/available", Method.GET);
			request.AddParameter("url", YoutubeUrl);
			IRestResponse finalResponse = Client.Execute(request);
			Assert.That(finalResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

			SnapshotResponse locationResponse =
				new JsonDeserializer().
				Deserialize<SnapshotResponse>(finalResponse);
			Assert.That(locationResponse.ArchivedSnapshots.Closest.Status, Is.EqualTo("UPDATEMESSAGE"));
		}

		[Test]
		public void DeleteSnapshot_ResponseCreated()
		{
			// arrange
			var body = new { url = YoutubeUrl, annotation = new { id = "lst-ib", message = "Theres a microphone button in the searchbox" } };
			var client = new RestClient("https://pragma.archivelab.org/");
			var request = new RestRequest("wayback/available", Method.DELETE);
			request.AddJsonBody(body);

			// act
			IRestResponse response = Client.Execute(request);

			//Assert
			Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));

			RestRequest finalRequest = new RestRequest("wayback/available", Method.GET);
			request.AddParameter("url", YoutubeUrl);
			IRestResponse finalResponse = Client.Execute(request);
			Assert.That(finalResponse.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));

			SnapshotResponse locationResponse =
				new JsonDeserializer().
				Deserialize<SnapshotResponse>(finalResponse);
			Assert.That(locationResponse.ArchivedSnapshots.Closest.Status, Is.Not.EqualTo("Theres a microphone button in the searchbox"));
		}
	}
}
