using NetTelegramBotApi.Types;

namespace Telegram.Bot
{
	public class MessageEvent
	{
		public long UpdateId { get; private set; }
		public Message Message { get; private set; }
		public MessageUpdateType UpdateType { get; private set; }

		public static MessageEvent From(Update update)
		{
			var result = new MessageEvent
			{
				UpdateId = update.UpdateId
			};

			if (update.Message != null)
			{
				result.UpdateType = MessageUpdateType.Message;
				result.Message = update.Message;
			}

			if (update.EditedMessage != null)
			{
				result.UpdateType = MessageUpdateType.EditedMessage;
				result.Message = update.EditedMessage;
			}

			if (update.ChannelPost != null)
			{
				result.UpdateType = MessageUpdateType.ChannelPost;
				result.Message = update.ChannelPost;
			}

			if (update.EditedChannelPost != null)
			{
				result.UpdateType = MessageUpdateType.EditedChannelPost;
				result.Message = update.EditedChannelPost;
			}

			return result;
		}
	}
}
