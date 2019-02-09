using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;

namespace challenge_api.Formatters.Output {
    public class VcardOutputFormatter : TextOutputFormatter {

        public VcardOutputFormatter() {
            SupportedMediaTypes.Add(Microsoft.Net.Http.Headers.MediaTypeHeaderValue.Parse("text/vcard"));

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding) {
            IServiceProvider serviceProvider = context.HttpContext.RequestServices;
            var logger = serviceProvider.GetService(typeof(ILogger<VcardOutputFormatter>)) as ILogger;

            var response = context.HttpContext.Response;

            var buffer = new StringBuilder();
            if (context.Object is IEnumerable<Database.Models.Person>) {
                foreach (Database.Models.Person contact in context.Object as IEnumerable<Database.Models.Person>) {
                    FormatVcard(buffer, contact, logger);
                }
            }
            else {
                var contact = context.Object as Database.Models.Person;
                FormatVcard(buffer, contact, logger);
            }
            return response.WriteAsync(buffer.ToString());
        }

        protected override bool CanWriteType(Type type) {
            if (typeof(Database.Models.Person).IsAssignableFrom(type) || typeof(IEnumerable<Database.Models.Person>).IsAssignableFrom(type)) {
                return base.CanWriteType(type);
            }
            return false;
        }

        private static void FormatVcard(StringBuilder buffer, Database.Models.Person contact, ILogger logger) {
            buffer.AppendLine("BEGIN:VCARD");
            buffer.AppendLine("VERSION:2.1");
            buffer.AppendFormat($"N:{contact.LastName};{contact.FirstName}\r\n");
            buffer.AppendFormat($"FN:{contact.FirstName} {contact.LastName}\r\n");
            buffer.AppendFormat($"UID:{contact.BusinessEntityId}\r\n");
            buffer.AppendLine("END:VCARD");
            logger.LogInformation($"Writing {contact.FirstName} {contact.LastName}");
        }
    }
}
