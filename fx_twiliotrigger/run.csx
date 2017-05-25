using System.Net;
using System.Text;
using Twilio.TwiML;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, IAsyncCollector<string> outputGuestbookItem, TraceWriter log)
{
    log.Info("Webhook triggered from Twilio");

    // gather values from SMS webhook
    var data = await req.Content.ReadAsStringAsync();
    var formValues = data.Split('&')
        .Select(value => value.Split('='))
        .ToDictionary(pair => Uri.UnescapeDataString(pair[0]).Replace("+", " "), 
                      pair => Uri.UnescapeDataString(pair[1]).Replace("+", " "));
    var smsFrom = formValues["From"];
    var smsBody = formValues["Body"];

    // Get sentiment score for post
    var score = await GetSentimentScore(smsBody);

    // write values to Azure queue
    var queueMessage = smsFrom + "|" + smsBody + "|" + score;
    log.Info("Writing to queue: " + queueMessage);
    await outputGuestbookItem.AddAsync(queueMessage);

    // create SMS response to sender
    var response = new MessagingResponse().Message($"Thanks for participating{smsFrom}. Message you sent: {smsBody} (Score={score})");
    var twiml = response.ToString();
    twiml = twiml.Replace("utf-16", "utf-8");

    return new HttpResponseMessage
    {
        Content = new StringContent(twiml, Encoding.UTF8, "application/xml")
    };
}

public async static Task<float> GetSentimentScore(string text)
{
    var document = JsonConvert.SerializeObject(new { documents = new[] { new { language = "en", id = 1, text = text } } });
    string sentimentUri = "<https://your_url_here/text/analytics/v2.0/sentiment>";

    using (var client = new HttpClient())
    {
        client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "<your_key_here>");
        using (var response = await client.PostAsync(sentimentUri, new StringContent(document, Encoding.UTF8, "application/json")))
        {
            JObject result = await response.Content.ReadAsAsync<JObject>();
            return result.SelectToken("documents[0].score")?.Value<float>() ?? 0;
        }
    }
}