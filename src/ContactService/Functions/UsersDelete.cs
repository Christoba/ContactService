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
    /// A function to delete users.
    /// </summary>
    public static class UsersDelete
    {
        /// <summary>
        /// Creates users
        /// </summary>
        /// <param name="req">The req.</param>
        /// <param name="log">The log.</param>
        /// <param name="executionContext">The execution context.</param>
        /// <param name="token">The token.</param>
        /// <returns>
        /// The <see cref="HttpResponseMessage"/>.
        /// </returns>
        [FunctionName("UsersDelete")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "Post")]
            HttpRequest req,
            ILogger log,
            ExecutionContext executionContext,
            CancellationToken token)
        {
            var runner = new FunctionRunner(ServiceResolver.Instance, executionContext, log);

            try
            {
                await runner.DeleteUsersAsync(token).ConfigureAwait(false);

                return new HttpResponseMessage(HttpStatusCode.OK);
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
