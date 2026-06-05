using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bug.BetterThanYesterday.Domain.Extensions;
using Microsoft.AspNetCore.Http;

namespace Bug.BetterThanYesterday.API.Tests.Commons
{
    public class Route
    {
        public string Name { get; set; }
		public MocksCollection MocksSetUp { get; set; } = new MocksCollection();
		public HttpMethod Method { get; set; }
        public string Path { get; set; }
        public object? Body { get; set; }
		public bool NeedsToResetMocksAfter { get; set; } = false;
        public int ExpectedStatusCode { get; set; }
        public string ExpectedMessageContains { get; set; }

		public override string ToString()
		{
			var sbCurl = new StringBuilder();

			sbCurl.AppendLine($"\n{Name}");
			sbCurl.AppendLine($@"curl --request {Method.Method} \");
			sbCurl.AppendLine($@" --url http://localhost:5018/testapi/{Path} \");
			sbCurl.AppendLine($@" --header 'accept: */*' \");

			if (Method != HttpMethod.Get && Body is not null)
			{
				sbCurl.AppendLine($@" --header 'Content-Type: application/json' \");
				sbCurl.AppendLine($@" --data '{Body.ToJson()}' \");
			}

			sbCurl.AppendLine();

			return sbCurl.ToString();
		}
	}
}