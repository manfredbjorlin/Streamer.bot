using System;
using System.Net.Http;
using System.IO;

public class CPHInline
{
	public bool Execute()
	{
		try
		{
			string key = "API KEY HERE";
      string fileNameAndPath = @"C:\My\Path\TTS.mp3";
      
			var textToSpeach = args["ttsMessage"].ToString();

			if(textToSpeach.Trim().Length == 0)
			{
				CPH.SetGlobalVar("HasSomethingToSay", 0, false);
			}
			else
			{
				CPH.SetGlobalVar("HasSomethingToSay", 1, false);
				var gender = "Male";
				var name = "en-US-ChristopherNeural";

				if(args.ContainsKey("ttsGender"))
					gender = args["ttsGender"].ToString();
				
				if(args.ContainsKey("ttsName"))
					name = args["ttsName"].ToString();

				textToSpeach = textToSpeach.Replace("<", "&lt;").Replace(">", "&gt;").Replace("&", "&amp;");

				var requestContent = $"<speak version='1.0' xml:lang='en-US'><voice xml:lang='en-US' xml:gender='{gender}' name='{name}'>{textToSpeach}</voice></speak>";
				
				using(var client = new HttpClient())
				{
					client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);

					var tokenRes = client.PostAsync("https://westeurope.api.cognitive.microsoft.com/sts/v1.0/issuetoken", null).Result.Content.ReadAsStringAsync().Result;

					client.DefaultRequestHeaders.Clear();
					client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);
					client.DefaultRequestHeaders.Add("X-Microsoft-OutputFormat", "audio-16khz-32kbitrate-mono-mp3");
					client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenRes}");
					client.DefaultRequestHeaders.Add("User-Agent", "MannyBot/0.1a");

					var res = client.PostAsync("https://westeurope.tts.speech.microsoft.com/cognitiveservices/v1", new StringContent(requestContent)).Result;

					var file = client.PostAsync("https://westeurope.tts.speech.microsoft.com/cognitiveservices/v1", new StringContent(requestContent)).Result.Content.ReadAsByteArrayAsync().Result;
					File.WriteAllBytes(fileNameAndPath, file);
				}
			}
		}
		catch(Exception ex)
		{
			CPH.LogInfo($"Exception in TTS: {ex.Message}");
		}
		return true;
	}
}
