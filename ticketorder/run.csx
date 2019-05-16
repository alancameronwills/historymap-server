#r "SendGrid"

using System.Net;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Text;

// PayPal webhook by moylgrove.wales 

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    var datanc = await req.Content.ReadAsStringAsync();
    //log.Info(datanc);
    var m = await req.Content.ReadAsFormDataAsync();

    var b = new StringBuilder();
    foreach ( String s in m.AllKeys )
         b.AppendLine(String.Format( "   {0,-10} {1}", s, m[s] ));
    log.Info(b.ToString());

    var eventAndDate = (m["item_name"] + "|").Split('|'); // Just in case
    await send(log, m["payer_email"],eventAndDate[0],eventAndDate[1],m["quantity"],m["mc_gross"],
    m["payment_date"],m["address_name"] + " " + m["address_zip"],
    m["option_selection1"],m["option_selection2"]);
    
    return  req.CreateResponse(HttpStatusCode.OK, "OK");
}


private static async Task send (TraceWriter log, string toAddress, string title, string date, 
        string quantity, string paid, string purchased, string name,
        string opt1, string opt2) { 
        log.Info("send: " + toAddress);
        var from = new Email("info@moylgrove.wales");
        var subject = "Tickets for Moylgrove";
        var to = new Email(toAddress);
        var sb = new StringBuilder ();
        sb.AppendLine("<p>Thanks for ordering tickets from Moylgrove.</p>");
        sb.AppendLine("<table>");
        sb.AppendLine("<tr><td>Event:</td><td>" + title + "</td></tr>");
        sb.AppendLine("<tr><td>Date:</td><td>" + date + "</td></tr>");
        sb.AppendLine("<tr><td>Where:</td><td>Moylgrove Old School Hall, SA43 3BW</td></tr>");
        sb.AppendLine("<tr><td>Tickets:</td><td>" + quantity + "</td></tr>");
        sb.AppendLine("<tr><td></td><td>" + opt1 + "</td></tr>");
        sb.AppendLine("<tr><td></td><td>" + opt2 + "</td></tr>");
        sb.AppendLine("<tr><td>Paid:</td><td>£ " + paid + "</td></tr>");
        sb.AppendLine("<tr><td>Purchased:</td><td>" + purchased + "</td></tr>");
        sb.AppendLine("<tr><td>Paid by:</td><td>" + name + "</td></tr>");
        sb.AppendLine("</table>");
        sb.AppendLine("<p>This email is your ticket. It would be helpful if you could bring it with you.</p>");
        sb.AppendLine("<p>Directions: From the middle of Moylgrove, go up the hill SW towards Newport."
            + " The Old School Hall is about 200m on the left. <a href='https://goo.gl/maps/DnGV5rveER6mRXkM6'>Streetview</a></p>");
        Content content = new Content("text/html", sb.ToString());
        Mail mail = new Mail(from, subject, to, content);
        dynamic sg = new SendGridAPIClient("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");
        dynamic response = await sg.client.mail.send.post(requestBody: mail.Get());
}


