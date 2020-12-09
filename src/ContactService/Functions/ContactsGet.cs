namespace ContactService.Functions
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using Models;
    using Newtonsoft.Json;
    using Services;
    using ExecutionContext = Microsoft.Azure.WebJobs.ExecutionContext;

    /// <summary>
    /// The get function.
    /// </summary>
    public static class ContactsGet
    {
        /// <summary>
        /// Gets contacts
        /// </summary>
        /// <param name="req">The req.</param>
        /// <param name="log">The log.</param>
        /// <param name="executionContext">The execution context.</param>
        /// <param name="token">The token.</param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [FunctionName("ContactsGet")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "Get", "Post")]
            HttpRequest req,
            ILogger log,
            ExecutionContext executionContext,
            CancellationToken token)
        {
            var runner = new FunctionRunner(ServiceResolver.Instance, executionContext, log);

            try
            {
                using var reader = new StreamReader(req.Body);
                var body = await reader.ReadToEndAsync().ConfigureAwait(false);
                var request = JsonConvert.DeserializeObject<ContactsGetRequest>(body);

                var documents = runner.GetContacts(request);

                var json = JsonConvert.SerializeObject(documents, Formatting.Indented);
                return new HttpResponseMessage(HttpStatusCode.OK)
                           {
                               Content = new StringContent(
                                   json,
                                   Encoding.UTF8,
                                   "application/json")
                           };
            }
            catch (ContactServiceException e)
            {
                return new HttpResponseMessage((HttpStatusCode)e.StatusCode)
                {
                    Content = new StringContent(
                        JsonConvert.SerializeObject(e),
                        Encoding.UTF8,
                        "application/json")
                };
            }
            catch (Exception e)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(
                        JsonConvert.SerializeObject(e),
                        Encoding.UTF8,
                        "application/json")
                };
            }
        }
    }
}
