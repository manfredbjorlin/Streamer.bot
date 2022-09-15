using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using System.Net.Http.Headers;

public class CPHInline
{
	const string APIAddress = "https://dezgo.p.rapidapi.com/text2image";

	public bool Execute()
	{
		var user = args["user"].ToString();
    var rapidApiKey = "ENTER YOUR KEY HERE";
    var fileFolder = @"C:\TwitchAuthCode\AIFiles\";

		var dataContent = new FormUrlEncodedContent(new[]
			{
				new KeyValuePair<string,string>("width", "512"),
				new KeyValuePair<string,string>("steps", "50"),
				new KeyValuePair<string,string>("guidance", "7.5"),
				new KeyValuePair<string,string>("height", "512"),
				new KeyValuePair<string,string>("prompt", args["rawInput"].ToString())
			});
		
		using (HttpClient httpClient = new HttpClient())
		{
			httpClient.DefaultRequestHeaders.Clear();
			httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", rapidApiKey);
			httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Host", "dezgo.p.rapidapi.com");
		
			var file = httpClient.PostAsync(APIAddress, dataContent).Result.Content.ReadAsByteArrayAsync().Result;
			var fileName = $"{fileFolder}ai_{new Random().Next()}.png";
			File.WriteAllBytes(fileName, file);
			CPH.SetArgument("aiFileName", fileName);
			CPH.SetArgument("screenshotFile", fileName);

			CPH.SetArgument("discordMessage", $"{user} just asked for a picture with the prompt: " + args["rawInput"].ToString());
		}

		return true;
	}
}
