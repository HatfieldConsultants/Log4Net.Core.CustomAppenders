/*
   Copyright 2019 Hatfield Consultants

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at
   
   http://www.apache.org/licenses/LICENSE-2.0
   
   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
 */

using System.IO;
using System.Net.Mail;
using log4net.Appender;
using log4net.Core;

namespace Hatfield.Log4Net.Core.CustomAppenders.Appenders
{
    /// <summary>
    /// https://github.com/apache/logging-log4net/blob/master/src/Appender/SmtpAppender.cs
    /// </summary>
    public class SMTPAppender : BufferingAppenderSkeleton
    {
        public string To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string SmtpHost { get; set; }
        public string Port { get; set; }

        protected void SendEmail(string messageBody)
        {
            SmtpClient client = new SmtpClient(SmtpHost);
            client.UseDefaultCredentials = false;
            client.Port = int.Parse(Port);
            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(From);
                mailMessage.To.Add(To);
                mailMessage.Body = messageBody;
                mailMessage.Subject = Subject;
                client.Send(mailMessage);
            }
        }

        protected override bool RequiresLayout => true;

        protected override void SendBuffer(LoggingEvent[] events)
        {
            StringWriter writer = new StringWriter(System.Globalization.CultureInfo.InvariantCulture);

            string t = Layout.Header;
            if (t != null)
            {
                writer.Write(t);
            }

            for (int i = 0; i < events.Length; i++)
            {
                // Render the event and append the text to the buffer
                RenderLoggingEvent(writer, events[i]);
            }

            t = Layout.Footer;
            if (t != null)
            {
                writer.Write(t);
            }

            SendEmail(writer.ToString());
        }
    }
}
