using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http.Headers;

public class CPHInline
{
	const string discordWebhook = "https://discord.com/api/webhooks/100664........";
  const string avatarUrl = "https://...";
  const string botName = "MyBot";

	public bool Execute()
	{
		var user = args["user"].ToString();

		var file_name = Path.GetFileName(args["screenshotFile"].ToString());
		var message = string.Empty;
		if(args.ContainsKey("discordMessage"))
			message = args["discordMessage"].ToString();
		else
			message = $"{user} just made me a meme!";

		string discordJson = JsonConvert.SerializeObject(new {
                content = message,
				username = botName,
				avatar_url = avatarUrl,
				embeds = new[]{ new { image = new { url = "attachment://" + file_name } } }
            });

		MultipartFormDataContent dataContent = new MultipartFormDataContent();
		dataContent.Add(new StringContent("@" + args["screenshotFile"].ToString()), "file1");
		dataContent.Add(new StringContent(discordJson), "payload_json");

		var file_content = new ByteArrayContent(File.ReadAllBytes(args["screenshotFile"].ToString()));
		file_content.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
		file_content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
		{
			FileName = file_name
		};
		dataContent.Add(file_content);

		HttpClient httpClient = new HttpClient();
		httpClient.PostAsync(discordWebhook, dataContent).Wait();
		httpClient.Dispose();

		return true;
	}
}
