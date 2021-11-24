using ApplicationCore.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ApplicationCore.Middlewares
{
    public class IPListMiddleware
    {
		private readonly RequestDelegate _next;
		private readonly IAppLogger _logger;
		private readonly string _safelist;

		public IPListMiddleware(RequestDelegate next, IAppLogger logger, string safelist)
		{
			_next = next;
			_logger = logger;
			_safelist = safelist;
		}

		public async Task InvokeAsync(HttpContext context)
		{
            var remoteIp = context.Connection.RemoteIpAddress;

            string[] ip = _safelist.Split(',');

            var bytes = remoteIp.GetAddressBytes();
            var badIp = true;
            foreach (var address in ip)
            {
                var testIp = IPAddress.Parse(address);
                if (testIp.GetAddressBytes().SequenceEqual(bytes))
                {
                    badIp = false;
                    break;
                }
            }

            if (badIp)
            {
                _logger.LogWarn($"IPListMiddleware: Forbidden Request from Remote IP address: {remoteIp}");
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }

            await _next.Invoke(context);
        }
	}
}
