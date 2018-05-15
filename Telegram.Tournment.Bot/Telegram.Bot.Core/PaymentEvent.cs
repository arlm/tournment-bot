using System;
using NetTelegramBotApi.Types;

namespace Telegram.Bot
{
    public class PaymentEvent
    {
		public long UpdateId { get; private set; }

		public static PaymentEvent From(Update update)
        {
			return new PaymentEvent { UpdateId = update.UpdateId };
        }
    }
}