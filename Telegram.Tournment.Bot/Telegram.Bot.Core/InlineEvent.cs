using System;
using NetTelegramBotApi.Types;

namespace Telegram.Bot
{
	public class InlineEvent
	{
		public long UpdateId { get; private set; }
		public InlineQuery InlineQuery { get; private set; }
		public ChosenInlineResult ChosenInlineResult { get; private set; }
		public CallbackQuery CallbackQuery { get; private set; }
		public InlineUpdateType UpdateType { get; private set; }

		public static InlineEvent From(Update update)
		{
			var result = new InlineEvent
			{
				UpdateId = update.UpdateId
			};

			if (update.InlineQuery != null)
			{
				result.UpdateType = InlineUpdateType.InlineQuery;
				result.InlineQuery = update.InlineQuery;
			}

			if (update.ChosenInlineResult != null)
			{
				result.UpdateType = InlineUpdateType.ChosenInlineResult;
				result.ChosenInlineResult = update.ChosenInlineResult;
			}

			if (update.CallbackQuery != null)
			{
				result.UpdateType = InlineUpdateType.CallbackQuery;
				result.CallbackQuery = update.CallbackQuery;
			}

			return result;
		}
	}
}
