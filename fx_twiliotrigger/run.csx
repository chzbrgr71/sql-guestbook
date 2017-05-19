using System.Net;
using System.Text;
using Twilio.TwiML;

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
    var queueMessage = smsFrom + "|" + smsBody;

    log.Info("SMS received: " + queueMessage);

    // write values to Azure queue
    await outputGuestbookItem.AddAsync(queueMessage);

    // create SMS response to sender
    var response = new MessagingResponse().Message($"Thanks for participating{smsFrom}. Message you sent: {smsBody}");
    var twiml = response.ToString();
    twiml = twiml.Replace("utf-16", "utf-8");

    return new HttpResponseMessage
    {
        Content = new StringContent(twiml, Encoding.UTF8, "application/xml")
    };
}