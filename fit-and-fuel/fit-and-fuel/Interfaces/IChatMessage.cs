using fit_and_fuel.DTOs;
using fit_and_fuel.Model;
using Microsoft.AspNetCore.Mvc;

namespace fit_and_fuel.Interfaces
{
    public interface IChatMessage
    {
        Task SendMessage(string userId, ChatMessageDto messageDto);
        Task<List<ChatMessage>> ReceiveMessages(string Id);
    }
}
