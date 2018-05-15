using System;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Amazon.Lambda.APIGatewayEvents;
using NetTelegramBotApi;
using NetTelegramBotApi.Types;
using NetTelegramBotApi.Util;
using Newtonsoft.Json;

namespace Telegram.Bot
{
	public class WebHookHandler
	{
		private readonly Subject<MessageEvent> messageSubject = new Subject<MessageEvent>();
		public IObservable<MessageEvent> MessageEvents => messageSubject.AsObservable();

		private readonly Subject<InlineEvent> inlineSubject = new Subject<InlineEvent>();
		public IObservable<InlineEvent> InlineEvents => inlineSubject.AsObservable();

		private readonly Subject<PaymentEvent> paymentSubject = new Subject<PaymentEvent>();
		public IObservable<PaymentEvent> PaymentEvents => paymentSubject.AsObservable();

		private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
		{
			ContractResolver = new NetTelegramBotApi.Util.JsonLowerCaseUnderscoreContractResolver(),
			NullValueHandling = NullValueHandling.Ignore
		};

		static WebHookHandler()
		{
			JsonSettings.Converters.Add(new UnixDateTimeConverter());
		}
        
		/// <summary>
		/// Use this method to deserialize <see cref="Update">Update</see> object, sent to <see cref="NetTelegramBotApi.Requests.SetWebhook">your webhook</see> by Telegram server.
		/// </summary>
		/// <param name="json">Json-string with Update (body of HTTP POST to your webhook)</param>
		/// <returns>Deserialized <see cref="Update"/> message</returns>
		private Update DeserializeUpdate(string json) => DeserializeMessage<Update>(json);

		private T DeserializeMessage<T>(string json) => JsonConvert.DeserializeObject<T>(json, JsonSettings);

		public BotResponse<string> Process(APIGatewayProxyRequest request)
		{
			return Process(request?.Body);
		}

		public BotResponse<string> Process(HttpRequestMessage request)
		{
			var task = request.Content.ReadAsStringAsync();
			string requestBody = task.GetAwaiter().GetResult();
			return Process(requestBody);
		}

		public BotResponse<string> Process(string requestBody)
		{
			var response = new BotResponse<string>
			{
				Ok = false,
				Description = "Not implemented",
				ErrorCode = -1,
				Parameters = null,
				Result = "Not implemented"
			};

			if (ExtractUpdate(requestBody))
			{
				response.Ok = true;
				response.Description = "OK";
				response.Result = response.Description;
				response.ErrorCode = null;
			}
			else
			{
				response.Description = "Invalid JSON";
				response.Result = response.Description;
				response.ErrorCode = 1;
			}

			return response;
		}

		internal bool ExtractUpdate(string requestBody)
		{
			var update = DeserializeUpdate(requestBody);

			if (update == null)
			{
				return false;
			}

			if (update.Message != null || update.EditedMessage != null ||
				update.ChannelPost != null || update.EditedChannelPost != null)
			{
				messageSubject.OnNext(MessageEvent.From(update));
			}

			if (update.InlineQuery != null || update.ChosenInlineResult != null ||
				update.CallbackQuery != null)
			{
				inlineSubject.OnNext(InlineEvent.From(update));
			}

			if (false && update.Message != null)
			{
				paymentSubject.OnNext(PaymentEvent.From(update));
			}

			return true;
		}
	}
}
