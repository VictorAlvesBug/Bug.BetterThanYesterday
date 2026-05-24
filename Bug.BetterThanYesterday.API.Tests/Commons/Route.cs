using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Bug.BetterThanYesterday.API.Tests.Commons
{
    public class Route
    {
        public string Name { get; set; }
        public HttpMethod Method { get; set; }
        public string Path { get; set; }
        public object? Body { get; set; }
        public int ExpectedStatusCode { get; set; }
        public string ExpectedMessageContains { get; set; }

		public override string ToString()
		{
			return $"Name: ({Method}) {Path}";
		}
	}
}