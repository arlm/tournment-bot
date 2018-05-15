using System.Collections.Generic;
using System.Net;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Telegram.Bot
{
	public class WebHook
	{     
		public APIGatewayProxyResponse Post(APIGatewayProxyRequest request, ILambdaContext context)
		{
            // Log entries show up in CloudWatch
			context.Logger.LogLine("Example log entry\n");

			var response = new APIGatewayProxyResponse
			{
				StatusCode = (int)HttpStatusCode.OK,
				Body = "{ \"Message\": \"Hello World\" }",
				Headers = new Dictionary<string, string> { { "Content-Type", "application/json" } }
			};

			return response;
		}
	}
}
