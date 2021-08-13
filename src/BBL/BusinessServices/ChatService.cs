using Application.BBL.Common;
using Application.BBLInterfaces.BusinessServicesInterfaces;
using Application.DAL;
using Application.EntitiesModels.Entities;
using Application.EntitiesModels.Entities.Chat;
using Application.EntitiesModels.Models;
using Application.EntitiesModels.Models.ChatModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BBL.BusinessServices
{
    public class ChatService : IChatService
    {
        private readonly IDbContextFactory dbContextFactory;

        private readonly IModelMapper modelMapper;

        private readonly UserManager<ApplicationUser> userManager;

        private const int CHAT_EXPIRES = 3;

        public ChatService(IDbContextFactory dbContextFactory, IModelMapper modelMapper, UserManager<ApplicationUser> _userManager)
        {
            this.dbContextFactory = dbContextFactory;

            this.modelMapper = modelMapper;

            this.userManager = _userManager;
        }

        public bool CloseChat(int id)
        {
            using (var context = dbContextFactory.Create())
            {
                var chat = context.Chats.FirstOrDefault(ch => ch.Id == id);

                if (chat != null)
                {
                    chat.IsChatEnded = true;

                    context.SaveChanges();

                    return true;
                }
                else
                    return false;

            }
        }

        public ChatModel CreateChat(string UserOrSessionId)
        {
            Chat newChat, createdChat;

            DateTime createdDate = DateTime.UtcNow;


            using (var context = dbContextFactory.Create())
            {
                var result = context.Chats.Include(ch => ch.ChatMessages).FirstOrDefault(chat => chat.SessionOrUserId == UserOrSessionId);
                if(result != null)
                {
                    result.IsChatEnded = false;
                    context.Chats.Update(result);
                    context.SaveChanges();
                    return new ChatModel()
                    {
                        Id = result.Id,
                        SessionOrUserId = result.SessionOrUserId,
                        CreatedDate = result.CreatedDate,
                        IsUserAuthorized = result.IsUserAuthorized,
                        UserNameOrEmail = result.UserNameOrEmail,
                        ChatGUID = result.ChatGUID,
                        ChatMessages = result.ChatMessages.Select(ms => modelMapper.MapTo<ChatMessage, ChatMessageModel>(ms)).ToList()
                    };
                }
                newChat = context.Chats.Add(new Chat()
                {
                    CreatedDate = createdDate,
                    TimeToCloseChat = createdDate.AddMinutes(CHAT_EXPIRES),
                    SessionOrUserId = UserOrSessionId,
                    ChatGUID = Guid.NewGuid()
                }).Entity;

                context.SaveChanges();

                createdChat = context.Chats.Where(ch => ch.ChatGUID == newChat.ChatGUID).First();
            }

            if (createdChat != null)
            {
                return new ChatModel()
                {
                    Id = createdChat.Id,
                    SessionOrUserId = createdChat.SessionOrUserId,
                    CreatedDate = createdChat.CreatedDate,
                    IsUserAuthorized = createdChat.IsUserAuthorized,
                    UserNameOrEmail = createdChat.UserNameOrEmail,
                    ChatGUID = createdChat.ChatGUID
                };
            }

            return null;
        }

        public ChatModel GetChatByChatGuid(Guid chatGUID)
        {
            using (var context = dbContextFactory.Create())
            {
                var chat = context.Chats.Where(ch => ch.ChatGUID == chatGUID).Include(ch => ch.ChatMessages).First();

                return modelMapper.MapTo<Chat, ChatModel>(chat);
            }
        }

        public ChatModel GetChatById(int id)
        {
            using (var context = dbContextFactory.Create())
            {
                var chat = context.Chats.First(ch => ch.Id == id);

                return modelMapper.MapTo<Chat, ChatModel>(chat);
            }
        }

        public ChatMessageModel AddMessage(ChatMessageModel message)
        {
            message.CreatedDate = DateTime.UtcNow;

            using (var context = dbContextFactory.Create())
            {
                context.ChatMessages.Add(modelMapper.MapTo<ChatMessageModel, ChatMessage>(message));

                var chat = context.Chats.SingleOrDefault(ch => ch.Id == message.ChatId);

                if(chat != null && !message.IsMessageFromOperator)
                {
                    chat.TimeToCloseChat = message.CreatedDate.Value.AddMinutes(CHAT_EXPIRES);
                }

                context.SaveChanges();

                return message;

            }
        }

        public List<ChatModel> GetNewestChat(int[] alreadyReceived)
        {
            using (var context = dbContextFactory.Create())
            {
                var newestChats = context.Chats.Where(ch => ch.IsChatEnded == false)
                    .Where(ch => alreadyReceived.All(m => m != ch.Id)).Include(ch => ch.ChatMessages).ToList();
                //Check ended chats
                if (alreadyReceived.Length > 0)
                {
                    var endedChats = context.Chats.Where(ch => ch.IsChatEnded == true)
                       .Where(ch => alreadyReceived.Any(m => m == ch.Id)).Include(ch => ch.ChatMessages).ToList();

                    if (endedChats.Count > 0)
                        newestChats.AddRange(endedChats);
                }
                return newestChats.Count > 0
                    ? modelMapper.MapTo<List<Chat>, List<ChatModel>>(newestChats)
                    : new List<ChatModel>();
            }
        }

        public List<ChatMessageModel> GetNewestMsg(NewestMsgQueryModel[] msgQueryModel)
        {
            var result = new List<ChatMessageModel>();

            if (msgQueryModel != null)
            {
                using (var context = dbContextFactory.Create())
                {
                    var newestChats = context.Chats.Where(x => msgQueryModel.Any(p => p.ChatId == x.Id)).Include(c => c.ChatMessages).ToList();

                    if (newestChats.Count > 0)
                    {
                        foreach (var query in msgQueryModel)
                        {
                            var currentChat = newestChats.Find(ch => ch.Id == query.ChatId);

                            if (IsChatShouldBeClosed(currentChat))
                            {
                                var closeResponse = new List<ChatMessageModel>
                            {
                                new ChatSouldBeClosed() {ChatId = currentChat.Id, ChatShouldBeClosed = true }
                            };

                                return closeResponse;
                            }

                            var newsetMsgs = currentChat.ChatMessages.Where(msg => msg.CreatedDate > query.LastMsgDate).ToList();
                            if (currentChat.IsChatEnded)
                            {
                                newsetMsgs = new List<ChatMessage>
                            {
                                new ChatMessage() { EndedByOperator = true }
                            };
                            }

                            result.AddRange(modelMapper.MapTo<List<ChatMessage>, List<ChatMessageModel>>(newsetMsgs));
                        }
                    }
                }
            }

            return result.Count > 0 ? result : null;
        }

        private bool IsChatShouldBeClosed(Chat chat)
        {
            return DateTime.UtcNow >= chat.TimeToCloseChat;
        }
    }
}