using System;
using Microsoft.Reactive.Testing;
using NUnit.Framework;
using NUnit.Rx;
using Telegram.Bot;

namespace Bot.Tests
{
	[TestFixture]
	public class WebHokTests
	{
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

			TestUpdate(body, new LocalObserver<MessageEvent>(@event =>
			{
				Assert.AreEqual(10000, @event.UpdateId);
				Assert.AreEqual(MessageUpdateType.Message, @event.UpdateType);
				Assert.AreEqual(1365, @event.Message.MessageId);
				Assert.AreEqual("/start", @event.Message.Text);
				Assert.AreEqual(new DateTime(2015,09,07,17,05,32, DateTimeKind.Utc), @event.Message.Date.UtcDateTime);

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

			TestUpdate(body, new LocalObserver<MessageEvent>(@event =>
			{
				Assert.AreEqual(10000, @event.UpdateId);
				Assert.AreEqual(MessageUpdateType.Message, @event.UpdateType);
				Assert.AreEqual(1365, @event.Message.MessageId);
				Assert.AreEqual("/start", @event.Message.Text);
				Assert.AreEqual(new DateTime(2015,09,07,17,05,32, DateTimeKind.Utc), @event.Message.Date.UtcDateTime);
                
				Assert.IsNotNull(@event.Message.Chat);
				Assert.AreEqual(1111111, @event.Message.Chat.Id);
				Assert.AreEqual("Test Lastname", @event.Message.Chat.LastName);
				Assert.AreEqual("Test Firstname", @event.Message.Chat.FirstName);
				Assert.AreEqual("Testusername", @event.Message.Chat.Username);
				Assert.AreEqual("private", @event.Message.Chat.Type);

				Assert.IsNotNull(@event.Message.From);
				Assert.AreEqual(@event.Message.Chat.Id, @event.Message.From.Id);
				Assert.AreEqual(@event.Message.Chat.LastName, @event.Message.From.LastName);
				Assert.AreEqual(@event.Message.Chat.FirstName, @event.Message.From.FirstName);
				Assert.AreEqual(@event.Message.Chat.Username, @event.Message.From.Username);
			}));
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

			TestUpdate(body, new LocalObserver<MessageEvent>(@event =>
			{
				Assert.AreEqual(10000, @event.UpdateId);
				Assert.AreEqual(MessageUpdateType.Message, @event.UpdateType);
				Assert.AreEqual(1365, @event.Message.MessageId);
				Assert.AreEqual("/start", @event.Message.Text);
				Assert.AreEqual(new DateTime(2015,09,07,17,05,32, DateTimeKind.Utc), @event.Message.Date.UtcDateTime);

				Assert.IsTrue(@event.Message.ForwardDate.HasValue);
                Assert.AreEqual(new DateTime(2015,09,07,17,05,50, DateTimeKind.Utc), @event.Message.ForwardDate.Value.UtcDateTime);
                
				Assert.IsNotNull(@event.Message.Chat);
				Assert.AreEqual(1111111, @event.Message.Chat.Id);
				Assert.AreEqual("Test Lastname", @event.Message.Chat.LastName);
				Assert.AreEqual("Test Firstname", @event.Message.Chat.FirstName);
				Assert.AreEqual("Testusername", @event.Message.Chat.Username);
				Assert.AreEqual("private", @event.Message.Chat.Type);
                
				Assert.IsNotNull(@event.Message.From);
				Assert.AreEqual(@event.Message.Chat.Id, @event.Message.From.Id);
				Assert.AreEqual(@event.Message.Chat.LastName, @event.Message.From.LastName);
				Assert.AreEqual(@event.Message.Chat.FirstName, @event.Message.From.FirstName);
				Assert.AreEqual(@event.Message.Chat.Username, @event.Message.From.Username);

				Assert.IsNotNull(@event.Message.ForwardFrom);
				Assert.AreEqual(222222, @event.Message.ForwardFrom.Id);
				Assert.AreEqual("Forward Lastname", @event.Message.ForwardFrom.LastName);
				Assert.AreEqual("Forward Firstname", @event.Message.ForwardFrom.FirstName);
			}));
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

			TestUpdate(body, new LocalObserver<MessageEvent>(@event =>
			{
				Assert.AreEqual(10000, @event.UpdateId);
				Assert.AreEqual(MessageUpdateType.Message, @event.UpdateType);
				Assert.AreEqual(1365, @event.Message.MessageId);
				Assert.AreEqual("/start", @event.Message.Text);
				Assert.AreEqual(new DateTime(2015, 09, 07, 17, 05, 32, DateTimeKind.Utc), @event.Message.Date.UtcDateTime);

                Assert.IsTrue(@event.Message.ForwardDate.HasValue);
                Assert.AreEqual(new DateTime(2015, 09, 07, 17, 05, 50, DateTimeKind.Utc), @event.Message.ForwardDate.Value.UtcDateTime);

				Assert.IsNotNull(@event.Message.Chat);
				Assert.AreEqual(1111111, @event.Message.Chat.Id);
				Assert.AreEqual("Test Lastname", @event.Message.Chat.LastName);
				Assert.AreEqual("Test Firstname", @event.Message.Chat.FirstName);
				Assert.AreEqual("Testusername", @event.Message.Chat.Username);
				Assert.AreEqual("private", @event.Message.Chat.Type);
                
				Assert.IsNotNull(@event.Message.From);
				Assert.AreEqual(@event.Message.Chat.Id, @event.Message.From.Id);
				Assert.AreEqual(@event.Message.Chat.LastName, @event.Message.From.LastName);
				Assert.AreEqual(@event.Message.Chat.FirstName, @event.Message.From.FirstName);
				Assert.AreEqual(@event.Message.Chat.Username, @event.Message.From.Username);

				Assert.IsNotNull(@event.Message.ForwardFrom);
				Assert.AreEqual(-10000000000, @event.Message.ForwardFrom.Id);
				//Assert.AreEqual("channel", @event.Message.ForwardFrom.Type);
				//Assert.AreEqual("Test channel", @event.Message.ForwardFrom.Title);
			}));
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

			TestUpdate(body, new LocalObserver<MessageEvent>(@event =>
			{
				Assert.AreEqual(10000, @event.UpdateId);
				Assert.AreEqual(MessageUpdateType.Message, @event.UpdateType);
				Assert.AreEqual(1365, @event.Message.MessageId);
				Assert.AreEqual("/start", @event.Message.Text);
				Assert.AreEqual(new DateTime(2015, 09, 07, 17, 05, 32, DateTimeKind.Utc), @event.Message.Date.UtcDateTime);

				Assert.IsNotNull(@event.Message.Chat);
				Assert.AreEqual(1111111, @event.Message.Chat.Id);
				Assert.AreEqual("Test Lastname", @event.Message.Chat.LastName);
				Assert.AreEqual("Test Firstname", @event.Message.Chat.FirstName);
				Assert.AreEqual("Testusername", @event.Message.Chat.Username);
				Assert.AreEqual("private", @event.Message.Chat.Type);
                
				Assert.IsNotNull(@event.Message.From);
				Assert.AreEqual(@event.Message.Chat.Id, @event.Message.From.Id);
				Assert.AreEqual(@event.Message.Chat.LastName, @event.Message.From.LastName);
				Assert.AreEqual(@event.Message.Chat.FirstName, @event.Message.From.FirstName);
				Assert.AreEqual(@event.Message.Chat.Username, @event.Message.From.Username);

				Assert.IsNotNull(@event.Message.ReplyToMessage);
				Assert.AreEqual(1334, @event.Message.ReplyToMessage.MessageId);
				Assert.AreEqual("Original", @event.Message.ReplyToMessage.Text);
				Assert.AreEqual(new DateTime(2015, 09, 07, 16, 56, 40, DateTimeKind.Utc), @event.Message.ReplyToMessage.Date.UtcDateTime);

				Assert.IsNotNull(@event.Message.ReplyToMessage.Chat);
				Assert.AreEqual(1111112, @event.Message.ReplyToMessage.Chat.Id);
				Assert.AreEqual("Reply Lastname", @event.Message.ReplyToMessage.Chat.LastName);
				Assert.AreEqual("Reply Firstname", @event.Message.ReplyToMessage.Chat.FirstName);
				Assert.AreEqual("Testusername", @event.Message.ReplyToMessage.Chat.Username);
				Assert.AreEqual("private", @event.Message.ReplyToMessage.Chat.Type);
			}));
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

			TestUpdate(body, new LocalObserver<MessageEvent>(@event =>
			{
				Assert.AreEqual(10000, @event.UpdateId);
				Assert.AreEqual(MessageUpdateType.EditedMessage, @event.UpdateType);
				Assert.AreEqual(1365, @event.Message.MessageId);
				Assert.AreEqual("Edited text", @event.Message.Text);
				Assert.AreEqual(new DateTime(2015, 09, 07, 17, 05, 32, DateTimeKind.Utc), @event.Message.Date.UtcDateTime);

				Assert.IsTrue(@event.Message.EditDate.HasValue);
                Assert.AreEqual(new DateTime(2015, 09, 07, 17, 23, 20, DateTimeKind.Utc), @event.Message.EditDate.Value.UtcDateTime);

				Assert.IsNotNull(@event.Message.Chat);
				Assert.AreEqual(1111111, @event.Message.Chat.Id);
				Assert.AreEqual("Test Lastname", @event.Message.Chat.LastName);
				Assert.AreEqual("Test Firstname", @event.Message.Chat.FirstName);
				Assert.AreEqual("Testusername", @event.Message.Chat.Username);
				Assert.AreEqual("private", @event.Message.Chat.Type);
                
				Assert.IsNotNull(@event.Message.From);
				Assert.AreEqual(@event.Message.Chat.Id, @event.Message.From.Id);
				Assert.AreEqual(@event.Message.Chat.LastName, @event.Message.From.LastName);
				Assert.AreEqual(@event.Message.Chat.FirstName, @event.Message.From.FirstName);
				Assert.AreEqual(@event.Message.Chat.Username, @event.Message.From.Username);
            }));
		}

		[Test]
		public void MessageWithEntities()
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

			TestUpdate(body, new LocalObserver<MessageEvent>(@event =>
			{
				Assert.AreEqual(10000, @event.UpdateId);
				Assert.AreEqual(MessageUpdateType.Message, @event.UpdateType);
				Assert.AreEqual(1365, @event.Message.MessageId);
				Assert.AreEqual("Bold and italics", @event.Message.Text);
				Assert.AreEqual(new DateTime(2015, 09, 07, 17, 05, 32, DateTimeKind.Utc), @event.Message.Date.UtcDateTime);
                
				Assert.IsNotNull(@event.Message.Chat);
				Assert.AreEqual(1111111, @event.Message.Chat.Id);
				Assert.AreEqual("Test Lastname", @event.Message.Chat.LastName);
				Assert.AreEqual("Test Firstname", @event.Message.Chat.FirstName);
				Assert.AreEqual("Testusername", @event.Message.Chat.Username);
				Assert.AreEqual("private", @event.Message.Chat.Type);
                
				Assert.IsNotNull(@event.Message.From);
				Assert.AreEqual(@event.Message.Chat.Id, @event.Message.From.Id);
				Assert.AreEqual(@event.Message.Chat.LastName, @event.Message.From.LastName);
				Assert.AreEqual(@event.Message.Chat.FirstName, @event.Message.From.FirstName);
				Assert.AreEqual(@event.Message.Chat.Username, @event.Message.From.Username);

				Assert.IsNotNull(@event.Message.Entities);
				Assert.AreEqual(2, @event.Message.Entities.Length);

				Assert.AreEqual("italic", @event.Message.Entities[0].Type);
				Assert.AreEqual(9, @event.Message.Entities[0].Offset);
				Assert.AreEqual(7, @event.Message.Entities[0].Length);

				Assert.AreEqual("bold", @event.Message.Entities[1].Type);
                Assert.AreEqual(0, @event.Message.Entities[1].Offset);
                Assert.AreEqual(4, @event.Message.Entities[1].Length);
			}));
		}

		[Test]
		public void MessageWithAudio()
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

			TestUpdate(body, new LocalObserver<MessageEvent>(@event =>
			{
				Assert.AreEqual(10000, @event.UpdateId);
				Assert.AreEqual(MessageUpdateType.Message, @event.UpdateType);
				Assert.AreEqual(1365, @event.Message.MessageId);
				Assert.AreEqual(new DateTime(2015, 09, 07, 17, 05, 32, DateTimeKind.Utc), @event.Message.Date.UtcDateTime);
                
				Assert.IsNotNull(@event.Message.Chat);
				Assert.AreEqual(1111111, @event.Message.Chat.Id);
				Assert.AreEqual("Test Lastname", @event.Message.Chat.LastName);
				Assert.AreEqual("Test Firstname", @event.Message.Chat.FirstName);
				Assert.AreEqual("Testusername", @event.Message.Chat.Username);
				Assert.AreEqual("private", @event.Message.Chat.Type);
                
				Assert.IsNotNull(@event.Message.From);
				Assert.AreEqual(@event.Message.Chat.Id, @event.Message.From.Id);
				Assert.AreEqual(@event.Message.Chat.LastName, @event.Message.From.LastName);
				Assert.AreEqual(@event.Message.Chat.FirstName, @event.Message.From.FirstName);
				Assert.AreEqual(@event.Message.Chat.Username, @event.Message.From.Username);

				Assert.IsNotNull(@event.Message.Audio);
				Assert.AreEqual("AwADBAADbXXXXXXXXXXXGBdhD2l6_XX", @event.Message.Audio.FileId);
				Assert.AreEqual(243, @event.Message.Audio.Duration);
				Assert.AreEqual("audio/mpeg", @event.Message.Audio.MimeType);
				Assert.AreEqual(3897500, @event.Message.Audio.FileSize);
				Assert.AreEqual("Test music file", @event.Message.Audio.Title);
			}));
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

			TestUpdate(body, new LocalObserver<MessageEvent>(@event =>
			{
				Assert.AreEqual(10000, @event.UpdateId);
				Assert.AreEqual(MessageUpdateType.Message, @event.UpdateType);
				Assert.AreEqual(1365, @event.Message.MessageId);
				Assert.AreEqual(new DateTime(2015, 09, 07, 17, 05, 32, DateTimeKind.Utc), @event.Message.Date.UtcDateTime);
                
				Assert.IsNotNull(@event.Message.Chat);
				Assert.AreEqual(1111111, @event.Message.Chat.Id);
				Assert.AreEqual("Test Lastname", @event.Message.Chat.LastName);
				Assert.AreEqual("Test Firstname", @event.Message.Chat.FirstName);
				Assert.AreEqual("Testusername", @event.Message.Chat.Username);
				Assert.AreEqual("private", @event.Message.Chat.Type);
                
				Assert.IsNotNull(@event.Message.From);
				Assert.AreEqual(@event.Message.Chat.Id, @event.Message.From.Id);
				Assert.AreEqual(@event.Message.Chat.LastName, @event.Message.From.LastName);
				Assert.AreEqual(@event.Message.Chat.FirstName, @event.Message.From.FirstName);
				Assert.AreEqual(@event.Message.Chat.Username, @event.Message.From.Username);

				Assert.IsNotNull(@event.Message.Voice);
				Assert.AreEqual("AwADBAADbXXXXXXXXXXXGBdhD2l6_XX", @event.Message.Voice.FileId);
				Assert.AreEqual(5, @event.Message.Voice.Duration);
				Assert.AreEqual("audio/ogg", @event.Message.Voice.MimeType);
				Assert.AreEqual(23000, @event.Message.Voice.FileSize);
			}));
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

			TestUpdate(body, new LocalObserver<MessageEvent>(@event =>
			{
				Assert.AreEqual(10000, @event.UpdateId);
				Assert.AreEqual(MessageUpdateType.Message, @event.UpdateType);
				Assert.AreEqual(1365, @event.Message.MessageId);
				Assert.AreEqual(new DateTime(2015, 09, 07, 17, 05, 32, DateTimeKind.Utc), @event.Message.Date.UtcDateTime);
                
				Assert.IsNotNull(@event.Message.Chat);
				Assert.AreEqual(1111111, @event.Message.Chat.Id);
				Assert.AreEqual("Test Lastname", @event.Message.Chat.LastName);
				Assert.AreEqual("Test Firstname", @event.Message.Chat.FirstName);
				Assert.AreEqual("Testusername", @event.Message.Chat.Username);
				Assert.AreEqual("private", @event.Message.Chat.Type);
                
				Assert.IsNotNull(@event.Message.From);
				Assert.AreEqual(@event.Message.Chat.Id, @event.Message.From.Id);
				Assert.AreEqual(@event.Message.Chat.LastName, @event.Message.From.LastName);
				Assert.AreEqual(@event.Message.Chat.FirstName, @event.Message.From.FirstName);
				Assert.AreEqual(@event.Message.Chat.Username, @event.Message.From.Username);

				Assert.IsNotNull(@event.Message.Document);
				Assert.AreEqual("AwADBAADbXXXXXXXXXXXGBdhD2l6_XX", @event.Message.Document.FileId);
				Assert.AreEqual("Testfile.pdf", @event.Message.Document.FileName);
				Assert.AreEqual("application/pdf", @event.Message.Document.MimeType);
				Assert.AreEqual(536392, @event.Message.Document.FileSize);
			}));
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

			TestUpdate(body, new LocalObserver<InlineEvent>(@event =>
			{
				Assert.AreEqual(10000, @event.UpdateId);
				Assert.AreEqual(InlineUpdateType.InlineQuery, @event.UpdateType);
				Assert.AreEqual("134567890097", @event.InlineQuery.Id);
				Assert.AreEqual("inline query", @event.InlineQuery.Query);
				Assert.AreEqual(string.Empty, @event.InlineQuery.Offset);
                
				Assert.IsNotNull(@event.InlineQuery.From);
				Assert.AreEqual(1111111, @event.InlineQuery.From.Id);
				Assert.AreEqual("Test Lastname", @event.InlineQuery.From.LastName);
				Assert.AreEqual("Test Firstname", @event.InlineQuery.From.FirstName);
				Assert.AreEqual("Testusername", @event.InlineQuery.From.Username);
				//Assert.AreEqual("private", @event.InlineQuery.From.Type);
			}));
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

			TestUpdate(body, new LocalObserver<InlineEvent>(@event =>
			{
				Assert.AreEqual(10000, @event.UpdateId);
				Assert.AreEqual(InlineUpdateType.ChosenInlineResult, @event.UpdateType);
				Assert.AreEqual("1234csdbsk4839", @event.ChosenInlineResult.InlineMessageId);
				Assert.AreEqual("12", @event.ChosenInlineResult.ResultId);

				Assert.IsNotNull(@event.ChosenInlineResult.From);
				Assert.AreEqual(1111111, @event.ChosenInlineResult.From.Id);
				Assert.AreEqual("Test Lastname", @event.ChosenInlineResult.From.LastName);
				Assert.AreEqual("Test Firstname", @event.ChosenInlineResult.From.FirstName);
				Assert.AreEqual("Testusername", @event.ChosenInlineResult.From.Username);
				//Assert.AreEqual("private", @event.ChosenInlineResult.From.Type);
			}));
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

			TestUpdate(body, new LocalObserver<InlineEvent>(@event =>
			{
				Assert.AreEqual(10000, @event.UpdateId);
				Assert.AreEqual(InlineUpdateType.CallbackQuery, @event.UpdateType);
				Assert.AreEqual("1234csdbsk4839", @event.CallbackQuery.InlineMessageId);
				Assert.AreEqual("4382bfdwdsb323b2d9", @event.CallbackQuery.Id);

				Assert.IsNotNull(@event.CallbackQuery.From);
				Assert.AreEqual(1111111, @event.CallbackQuery.From.Id);
				Assert.AreEqual("Test Lastname", @event.CallbackQuery.From.LastName);
				Assert.AreEqual("Test Firstname", @event.CallbackQuery.From.FirstName);
				Assert.AreEqual("Testusername", @event.CallbackQuery.From.Username);
				//Assert.AreEqual("private", @event.CallbackQuery.From.Type);
			}));
		}

		private static void TestUpdate<T>(string body, IObserver<T> observer)
		{
			var handler = new WebHookHandler();

#pragma warning disable CS1702 // Assuming assembly reference matches identity
			var scheduler = new TestScheduler();

			var inlineObserver = new MockObserver<InlineEvent>(scheduler);
			handler.InlineEvents.Subscribe(inlineObserver);

			if (typeof(T) == typeof(InlineEvent))
			{
				handler.InlineEvents.Subscribe((IObserver<InlineEvent>)observer);
			}

			var messageObserver = new MockObserver<MessageEvent>(scheduler);
			handler.MessageEvents.Subscribe(messageObserver);

			if (typeof(T) == typeof(MessageEvent))
			{
				handler.MessageEvents.Subscribe((IObserver<MessageEvent>)observer);
			}

			var paymentObserver = new MockObserver<PaymentEvent>(scheduler);
			handler.PaymentEvents.Subscribe(paymentObserver);

			if (typeof(T) == typeof(PaymentEvent))
			{
				handler.PaymentEvents.Subscribe((IObserver<PaymentEvent>)observer);
			}

			var response = handler.Process(body);

#pragma warning restore CS1702 // Assuming assembly reference matches identity

			Assert.IsTrue(response.Ok);
			Assert.IsNull(response.ErrorCode);

			if (typeof(T) == typeof(InlineEvent))
			{
				Assert.IsNotEmpty(inlineObserver.Messages);

				var notification = inlineObserver.Messages[0].Value;
                Assert.IsTrue(notification.HasValue);
			}
			else
			{
				Assert.IsEmpty(inlineObserver.Messages);
			}

			if (typeof(T) == typeof(PaymentEvent))
			{
				Assert.IsNotEmpty(paymentObserver.Messages);

				var notification = paymentObserver.Messages[0].Value;
                Assert.IsTrue(notification.HasValue);
			}
			else
			{
				Assert.IsEmpty(paymentObserver.Messages);
			}

			if (typeof(T) == typeof(MessageEvent))
			{
				Assert.IsNotEmpty(messageObserver.Messages);

				var notification = messageObserver.Messages[0].Value;
                Assert.IsTrue(notification.HasValue);
			}
			else
			{
				Assert.IsEmpty(messageObserver.Messages);
			}
		}
	}
}
