using Microsoft.Reactive.Testing;
using NUnit.Framework;
using NUnit.Rx;
using Telegram.Bot;

namespace Bot.Tests
{
	[TestFixture]
	public class WebHokTests
	{
		private object update_id;

		[Test]
		public void Message()
		{
			const string body = @"
            {
                ""update_id"":10000,
                ""message"":{
                    ""date"":1441645532,
                    ""chat"":{
                        ""last_name"":""Test Lastname"",
                        ""id"":1111111,
                        ""first_name"":""Test"",
                        ""username"":""Test""
                    },
                    ""message_id"":1365,
                    ""from"":{
                        ""last_name"":""Test Lastname"",
                        ""id"":1111111,
                         ""first_name"":""Test"",
                         ""username"":""Test""
                   },
                  ""text"":""/start""
                }
            }";

			var handler = new WebHookHandler();

#pragma warning disable CS1702 // Assuming assembly reference matches identity
			var scheduler = new TestScheduler();

			var inlineObserver = new MockObserver<InlineEvent>(scheduler);
			handler.InlineEvents.Subscribe(inlineObserver);

			var messageObserver = new MockObserver<MessageEvent>(scheduler);
			handler.MessageEvents.Subscribe(messageObserver);
			handler.MessageEvents.Subscribe(new LocalObserver<MessageEvent>(@event =>
			{
				Assert.AreEqual(10000, @event.UpdateId);
				Assert.AreEqual(MessageUpdateType.Message, @event.UpdateType);
				Assert.AreEqual(1365, @event.Message.MessageId);
				Assert.AreEqual("/start", @event.Message.Text);

				Assert.IsNotNull(@event.Message.Chat);
				Assert.AreEqual(1111111, @event.Message.Chat.Id);
				Assert.AreEqual("Test Lastname", @event.Message.Chat.LastName);
				Assert.AreEqual("Test", @event.Message.Chat.FirstName);
				Assert.AreEqual("Test", @event.Message.Chat.Username);

				Assert.IsNotNull(@event.Message.From);
				Assert.AreEqual(@event.Message.Chat.Id, @event.Message.From.Id);
				Assert.AreEqual(@event.Message.Chat.LastName, @event.Message.From.LastName);
				Assert.AreEqual(@event.Message.Chat.FirstName, @event.Message.From.FirstName);
				Assert.AreEqual(@event.Message.Chat.Username, @event.Message.From.Username);
			}));

			var paymentObserver = new MockObserver<PaymentEvent>(scheduler);
			handler.PaymentEvents.Subscribe(paymentObserver);

			var response = handler.Process(body);

#pragma warning restore CS1702 // Assuming assembly reference matches identity

			Assert.IsTrue(response.Ok);
			Assert.IsNull(response.ErrorCode);

			Assert.IsEmpty(inlineObserver.Messages);
			Assert.IsEmpty(paymentObserver.Messages);

			Assert.IsNotEmpty(messageObserver.Messages);

			var notification = messageObserver.Messages[0].Value;
			Assert.IsTrue(notification.HasValue);
		}

		[Test]
		public void MessageWithText()
		{
			const string body = @"
            {
                ""update_id"":10000,
                ""message"":{
                                ""date"":1441645532,
                  ""chat"":{
                                    ""last_name"":""Test Lastname"",
                     ""id"":1111111,
                     ""type"": ""private"",
                     ""first_name"":""Test Firstname"",
                     ""username"":""Testusername""
                  },
                  ""message_id"":1365,
                  ""from"":{
                                    ""last_name"":""Test Lastname"",
                     ""id"":1111111,
                     ""first_name"":""Test Firstname"",
                     ""username"":""Testusername""
                  },
                  ""text"":""/start""
                }
            }";

			Assert.Inconclusive();
		}

		[Test]
		public void ForwardedMessage()
		{
			const string body = @"
            {
                ""update_id"":10000,
                ""message"":{
                                ""date"":1441645532,
                  ""chat"":{
                                    ""last_name"":""Test Lastname"",
                     ""id"":1111111,
                     ""type"": ""private"",
                     ""first_name"":""Test Firstname"",
                     ""username"":""Testusername""
                  },
                  ""message_id"":1365,
                  ""from"":{
                                    ""last_name"":""Test Lastname"",
                     ""id"":1111111,
                     ""first_name"":""Test Firstname"",
                     ""username"":""Testusername""
                  },
                  ""forward_from"": {
                                    ""last_name"":""Forward Lastname"",
                     ""id"": 222222,
                     ""first_name"":""Forward Firstname""
                  },
                  ""forward_date"":1441645550,
                  ""text"":""/start""
                }
            }";

			Assert.Inconclusive();
		}

		[Test]
		public void ForwardedChannelMessage()
		{
			const string body = @"
            {
                ""update_id"":10000,
                ""message"":{
                                ""date"":1441645532,
                  ""chat"":{
                                    ""last_name"":""Test Lastname"",
                     ""type"": ""private"",
                     ""id"":1111111,
                     ""first_name"":""Test Firstname"",
                     ""username"":""Testusername""
                  },
                  ""message_id"":1365,
                  ""from"":{
                                    ""last_name"":""Test Lastname"",
                     ""id"":1111111,
                     ""first_name"":""Test Firstname"",
                     ""username"":""Testusername""
                  },
                  ""forward_from"": {
                                    ""id"": -10000000000,
                     ""type"": ""channel"",
                     ""title"": ""Test channel""
                  },
                  ""forward_date"":1441645550,
                  ""text"":""/start""
                }
            }";

			Assert.Inconclusive();
		}

		[Test]
		public void MessageWithReply()
		{
			const string body = @"
            {
                ""update_id"":10000,
                ""message"":{
                                ""date"":1441645532,
                  ""chat"":{
                                    ""last_name"":""Test Lastname"",
                     ""type"": ""private"",
                     ""id"":1111111,
                     ""first_name"":""Test Firstname"",
                     ""username"":""Testusername""
                  },
                  ""message_id"":1365,
                  ""from"":{
                                    ""last_name"":""Test Lastname"",
                     ""id"":1111111,
                     ""first_name"":""Test Firstname"",
                     ""username"":""Testusername""
                  },
                  ""text"":""/start"",
                  ""reply_to_message"":{
                                    ""date"":1441645000,
                      ""chat"":{
                                        ""last_name"":""Reply Lastname"",
                          ""type"": ""private"",
                          ""id"":1111112,
                          ""first_name"":""Reply Firstname"",
                          ""username"":""Testusername""
                      },
                      ""message_id"":1334,
                      ""text"":""Original""
                  }
                }
            }";

			Assert.Inconclusive();
		}

		[Test]
		public void EditedMessage()
		{
			const string body = @"
            {
                ""update_id"":10000,
                ""edited_message"":{
                                ""date"":1441645532,
                  ""chat"":{
                                    ""last_name"":""Test Lastname"",
                     ""type"": ""private"",
                     ""id"":1111111,
                     ""first_name"":""Test Firstname"",
                     ""username"":""Testusername""
                  },
                  ""message_id"":1365,
                  ""from"":{
                                    ""last_name"":""Test Lastname"",
                     ""id"":1111111,
                     ""first_name"":""Test Firstname"",
                     ""username"":""Testusername""
                  },
                  ""text"":""Edited text"",
                  ""edit_date"": 1441646600
                }
            }";

			Assert.Inconclusive();
		}

		[Test]
		public void EditedWithEntities()
		{
			const string body = @"
            {
                ""update_id"":10000,
                ""message"":{
                                ""date"":1441645532,
                  ""chat"":{
                                    ""last_name"":""Test Lastname"",
                     ""type"": ""private"",
                     ""id"":1111111,
                     ""first_name"":""Test Firstname"",
                     ""username"":""Testusername""
                  },
                  ""message_id"":1365,
                  ""from"":{
                                    ""last_name"":""Test Lastname"",
                     ""id"":1111111,
                     ""first_name"":""Test Firstname"",
                     ""username"":""Testusername""
                  },
                  ""text"":""Bold and italics"",
                  ""entities"": [
                      {
                          ""type"": ""italic"",
                          ""offset"": 9,
                          ""length"": 7
                      },
                      {
                          ""type"": ""bold"",
                          ""offset"": 0,
                          ""length"": 4
                      }
                      ]
                }
            }";

			Assert.Inconclusive();
		}

		[Test]
		public void EditedWithAudio()
		{
			const string body = @"
            {
                ""update_id"":10000,
                ""message"":{
                                ""date"":1441645532,
                  ""chat"":{
                                    ""last_name"":""Test Lastname"",
                     ""type"": ""private"",
                     ""id"":1111111,
                     ""first_name"":""Test Firstname"",
                     ""username"":""Testusername""
                  },
                  ""message_id"":1365,
                  ""from"":{
                                    ""last_name"":""Test Lastname"",
                     ""id"":1111111,
                     ""first_name"":""Test Firstname"",
                     ""username"":""Testusername""
                  },
                  ""audio"": {
                                    ""file_id"": ""AwADBAADbXXXXXXXXXXXGBdhD2l6_XX"",
                      ""duration"": 243,
                      ""mime_type"": ""audio/mpeg"",
                      ""file_size"": 3897500,
                      ""title"": ""Test music file""
                  }
                }
            }";

			Assert.Inconclusive();
		}

		[Test]
		public void VoiceMessage()
		{
			const string body = @"
            {
                ""update_id"":10000,
                ""message"":{
                                ""date"":1441645532,
                  ""chat"":{
                                    ""last_name"":""Test Lastname"",
                     ""type"": ""private"",
                     ""id"":1111111,
                     ""first_name"":""Test Firstname"",
                     ""username"":""Testusername""
                  },
                  ""message_id"":1365,
                  ""from"":{
                                    ""last_name"":""Test Lastname"",
                     ""id"":1111111,
                     ""first_name"":""Test Firstname"",
                     ""username"":""Testusername""
                  },
                  ""voice"": {
                                    ""file_id"": ""AwADBAADbXXXXXXXXXXXGBdhD2l6_XX"",
                      ""duration"": 5,
                      ""mime_type"": ""audio/ogg"",
                      ""file_size"": 23000
                  }
                }
            }";

			Assert.Inconclusive();
		}

		[Test]
		public void MessageWithDocument()
		{
			const string body = @"
            {
                ""update_id"":10000,
                ""message"":{
                                ""date"":1441645532,
                  ""chat"":{
                                    ""last_name"":""Test Lastname"",
                     ""type"": ""private"",
                     ""id"":1111111,
                     ""first_name"":""Test Firstname"",
                     ""username"":""Testusername""
                  },
                  ""message_id"":1365,
                  ""from"":{
                                    ""last_name"":""Test Lastname"",
                     ""id"":1111111,
                     ""first_name"":""Test Firstname"",
                     ""username"":""Testusername""
                  },
                  ""document"": {
                                    ""file_id"": ""AwADBAADbXXXXXXXXXXXGBdhD2l6_XX"",
                      ""file_name"": ""Testfile.pdf"",
                      ""mime_type"": ""application/pdf"",
                      ""file_size"": 536392
                  }
                }
            }";

			Assert.Inconclusive();
		}

		[Test]
		public void InlineQuery()
		{
			const string body = @"
            {
                ""update_id"":10000,
                ""inline_query"":{
                                ""id"": 134567890097,
                  ""from"":{
                                    ""last_name"":""Test Lastname"",
                     ""type"": ""private"",
                     ""id"":1111111,
                     ""first_name"":""Test Firstname"",
                     ""username"":""Testusername""
                  },
                  ""query"": ""inline query"",
                  ""offset"": """"
                }
            }";

			Assert.Inconclusive();
		}

		[Test]
		public void ChosenInlineQuery()
		{
			const string body = @"
            {
                ""update_id"":10000,
                ""chosen_inline_result"":{
                                ""result_id"": ""12"",
                  ""from"":{
                                    ""last_name"":""Test Lastname"",
                     ""type"": ""private"",
                     ""id"":1111111,
                     ""first_name"":""Test Firstname"",
                     ""username"":""Testusername""
                  },
                  ""query"": ""inline query"",
                  ""inline_message_id"": ""1234csdbsk4839""
                }
            }";

			Assert.Inconclusive();
		}

		[Test]
		public void CallbackQuery()
		{
			const string body = @"
            {
                ""update_id"":10000,
                ""callback_query"":{
                                ""id"": ""4382bfdwdsb323b2d9"",
                  ""from"":{
                                    ""last_name"":""Test Lastname"",
                     ""type"": ""private"",
                     ""id"":1111111,
                     ""first_name"":""Test Firstname"",
                     ""username"":""Testusername""
                  },
                  ""data"": ""Data from button callback"",
                  ""inline_message_id"": ""1234csdbsk4839""
                }
            }";

			Assert.Inconclusive();
		}
	}
}
