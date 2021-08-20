using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using RealtimeToDo.Web.Services;

namespace RealtimeTodo.Web.Hubs
{
    public class ToDoHub : Hub
    {
        private readonly IToDoRepository _toDoRepository;

        public ToDoHub(IToDoRepository toDoRepository)
        {
            _toDoRepository = toDoRepository;
        }

        private async Task NotificationAdd(string message)
        {
            await Clients.All.SendAsync("NotificationAdd", message);
        }

        private async Task NotificationDelete(string message)
        {
            await Clients.All.SendAsync("NotificationDelete", message);
        }
        
        private async Task NotificationUpdate(string message)
        {
            await Clients.All.SendAsync("NotificationUpdate", message);
        }
        
        public async Task GetLists()
        {
            var results = await _toDoRepository.GetLists();

            await Clients.Caller.SendAsync("UpdatedToDoList", results);
        }

        public async Task GetList(int listId)
        {
            var result = await _toDoRepository.GetList(listId);

            await Clients.Caller.SendAsync("UpdatedListData", result);
        }

        public async Task SubscribeToCountUpdates()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Counts");
        }

        public async Task UnsubscribeFromCountUpdates()
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Counts");
        }

        public async Task SubscribeToListUpdates(int listId)
        {
            var groupName = ListIdToGroupName(listId);
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task UnsubscribeFromListUpdates(int listId)
        {
            var groupName = ListIdToGroupName(listId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task AddToDoItem(int listId, string text)
        {
            await NotificationAdd("Nova task " + text);
            await _toDoRepository.AddToDoItem(listId, text);

            var allLists = await _toDoRepository.GetLists();
            var listUpdate = await _toDoRepository.GetList(listId);

            var groupName = ListIdToGroupName(listId);
            await Clients.Group("Counts").SendAsync("UpdatedToDoList", allLists);
            await Clients.Group(groupName).SendAsync("UpdatedListData", listUpdate);
        }

        public async Task ToggleToDoItem(int listId, int itemId)
        {
            await NotificationAdd("Mudou status da task " + itemId);
            await _toDoRepository.ToggleToDoItem(listId, itemId);

            var allLists = await _toDoRepository.GetLists();
            var listUpdate = await _toDoRepository.GetList(listId);


            var groupName = ListIdToGroupName(listId);
            await Clients.Group("Counts").SendAsync("UpdatedToDoList", allLists);
            await Clients.Group(groupName).SendAsync("UpdatedListData", listUpdate);
        }
        
        public async Task DeleteToDoItem(int listId, int itemId)
        {
            await NotificationDelete("Deletou a task " + itemId);
            await _toDoRepository.DeleteToDoItem(listId, itemId);

            var allLists = await _toDoRepository.GetLists();
            var listUpdate = await _toDoRepository.GetList(listId);


            var groupName = ListIdToGroupName(listId);
            await Clients.Group("Counts").SendAsync("UpdatedToDoList", allLists);
            await Clients.Group(groupName).SendAsync("UpdatedListData", listUpdate);
        }


        private string ListIdToGroupName(int listId) => $"list-updates-{listId}";
    }
}