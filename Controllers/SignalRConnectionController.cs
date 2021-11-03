using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace serverless_signalr_poc_client.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SignalRConnectionController : ControllerBase
    {
        private static readonly HttpClient client = new HttpClient();

        [HttpGet("{username}")]
        public async Task<IActionResult> Get(string username)
        {
            var tokenResponse = await client.PostAsync("Http://localhost:7071/api/token", new StringContent(string.Empty));
            if (tokenResponse.IsSuccessStatusCode)
            {
                var responseBody = await tokenResponse.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<JObject>(responseBody)["token"].ToString();
                var hubConnectionBuilder = new HubConnectionBuilder()
                    .WithUrl("http://localhost:7071/api", options => {
                        options.Headers.Add("x-ms-signalr-user-id", username);
                        options.Headers.Add("X-Token", token);
                    });

                var conn = hubConnectionBuilder.Build();
                await conn.StartAsync();

                await conn.InvokeAsync("joinUserToGroup", username, "group1");

                return Ok();
            } else return Unauthorized();
        }

        [HttpGet("InvokeError/{username}")]
        public async Task<IActionResult> InvokeError(string username)
        {
            var tokenResponse = await client.PostAsync("Http://localhost:7071/api/token", new StringContent(string.Empty));
            if (tokenResponse.IsSuccessStatusCode)
            {
                var responseBody = await tokenResponse.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<JObject>(responseBody)["token"].ToString();
                var hubConnectionBuilder = new HubConnectionBuilder()
                    .WithUrl("http://localhost:7071/api", options => {
                        options.Headers.Add("x-ms-signalr-user-id", username);
                        options.Headers.Add("X-Token", token);
                    });

                var conn = hubConnectionBuilder.Build();
                await conn.StartAsync();

                await conn.InvokeAsync("joinUserError", username, "group1");

                return Ok();
            } else return Unauthorized();
        }
    }
}
