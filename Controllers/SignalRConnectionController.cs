using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace serverless_signalr_poc_client.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SignalRConnectionController : ControllerBase
    {
        [HttpGet("{username}")]
        public async Task<IActionResult> Get(string username)
        {
            var hubConnectionBuilder = new HubConnectionBuilder()
                .WithUrl("http://localhost:7071/api", options => {
                    options.Headers.Add("x-ms-signalr-user-id", username);
                });

            var conn = hubConnectionBuilder.Build();
            await conn.StartAsync();

            await conn.InvokeAsync("joinUserToGroup", username, "group1");

            return Ok();
        }

        [HttpGet("InvokeError/{username}")]
        public async Task<IActionResult> InvokeError(string username)
        {
            var hubConnectionBuilder = new HubConnectionBuilder()
                .WithUrl("http://localhost:7071/api", options => {
                    options.Headers.Add("x-ms-signalr-user-id", username);
                });

            var conn = hubConnectionBuilder.Build();
            await conn.StartAsync();

            await conn.InvokeAsync("joinUserError", username, "group1");

            return Ok();
        }
    }
}
