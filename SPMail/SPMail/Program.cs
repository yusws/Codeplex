using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.IO;
using Net.Mail;
using MyLocalBroadband.Logging;

namespace MyLocalBroadband.SPMail
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string host = ConfigurationManager.AppSettings.Get("popHost");
                int port = Convert.ToInt32(ConfigurationManager.AppSettings.Get("popPort"));
                bool useSSL = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("useSSL"));
                string username = ConfigurationManager.AppSettings.Get("popUser");
                string password = ConfigurationManager.AppSettings.Get("popPassword");
                string dropPath = ConfigurationManager.AppSettings.Get("dropFolderPath");
                bool traceToConsole = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("traceToConsole"));
                bool deleteAfterRetrieve = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("deleteAfterRetrieve"));

                TraceProvider.RegisterTraceProvider();

                using (Pop3Client popClient = new Pop3Client(host, port, useSSL, username, password))
                {
                    if (traceToConsole)
                        popClient.Trace += new Action<string>(Console.WriteLine);

                    //connects to Pop3 Server, Executes POP3 USER and PASS
                    popClient.Authenticate();
                    popClient.Stat();

                    System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient();
                    smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = dropPath;
                    StringBuilder traceMessage = new StringBuilder("Messages To Retrieve: " + popClient.List().Count);
                    Console.WriteLine(traceMessage.ToString());
                    ULS.LogMessage("Pop Client", traceMessage.ToString(), "E-Mail", TraceProvider.TraceSeverity.InformationEvent);
                    foreach (Pop3ListItem item in popClient.List())
                    {
                        using (MailMessageEx popMessage = popClient.RetrMailMessageEx(item))
                        {
                            using (System.Net.Mail.MailMessage smtpMessage = new System.Net.Mail.MailMessage())
                            {
                                if (traceToConsole)
                                {
                                    foreach (System.Net.Mail.MailAddress toAddress in popMessage.To)
                                    {
                                        Console.WriteLine("To:  " + toAddress.Address);
                                    }
                                    Console.WriteLine("From: " + popMessage.From.Address);
                                    Console.WriteLine("Subject: " + popMessage.Subject);
                                    Console.WriteLine("Attachments: " + popMessage.Attachments.Count);
                                    foreach (System.Net.Mail.Attachment attachment in popMessage.Attachments)
                                    {
                                        Console.WriteLine("Attachment: " + attachment.Name);
                                    }
                                    Console.Write("Body: " + popMessage.Body + "\n");
                                }

                                smtpMessage.From = popMessage.From;
                                smtpMessage.Subject = popMessage.Subject;
                                smtpMessage.SubjectEncoding = popMessage.SubjectEncoding;
                                smtpMessage.Body = popMessage.Body;
                                smtpMessage.BodyEncoding = popMessage.BodyEncoding;
                                smtpMessage.Sender = popMessage.Sender;

                                traceMessage = new StringBuilder("Message Processed:<br/>\n");
                  
                                foreach (System.Net.Mail.MailAddress toAddress in popMessage.To)
                                {
                                    traceMessage.Append("To: " + toAddress + "<br/>\n");
                                    smtpMessage.To.Add(toAddress);
                                }
                                foreach (System.Net.Mail.MailAddress ccAddress in popMessage.CC)
                                {
                                    traceMessage.Append("CC: " + ccAddress + "<br/>\n");
                                    smtpMessage.CC.Add(ccAddress);
                                }
                                traceMessage.Append("From: " + popMessage.From.Address + "<br/>\n");
                                traceMessage.Append("Subject: " + popMessage.Subject + "<br/>\n");

                                foreach (System.Net.Mail.Attachment attachment in popMessage.Attachments)
                                {
                                    smtpMessage.Attachments.Add(attachment);
                                    traceMessage.Append("Attachment: " + attachment.Name + "<br/>\n");
                                }

                                smtpMessage.IsBodyHtml = popMessage.IsBodyHtml;

                                foreach (string key in popMessage.Headers.AllKeys)
                                {
                                    smtpMessage.Headers.Add(key, popMessage.Headers[key]);
                                }

                                ULS.LogMessage("Pop Client", traceMessage.ToString(), "E-Mail", TraceProvider.TraceSeverity.InformationEvent);

                                smtpClient.Send(smtpMessage);

                                if (deleteAfterRetrieve)
                                {
                                    popClient.Dele(item);
                                }
                            }
                        }
                    }
                    popClient.Noop();
                    if (!deleteAfterRetrieve)
                    {
                        popClient.Rset();
                    }
                    popClient.Quit();
                }
            }
            catch(Exception ex)
            {
                ULS.LogError("Pop Client",ex.ToString(),"E-Mail");
            }
            TraceProvider.UnregisterTraceProvider();
        }
    }
}
