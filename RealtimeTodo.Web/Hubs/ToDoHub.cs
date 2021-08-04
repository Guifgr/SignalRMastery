using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using RealtimeToDo.Web.Services;

namespace RealtimeTodo.Web.Hubs
{
    public class ToDoHub : Hub
    {
        private readonly IToDoRepository _todoRepository;
        public ToDoHub(IToDoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        public async Task GetLists()
        {
            var results = await _todoRepository.GetLists();
            await Clients.Caller.SendAsync("updatedToDoList", results);
        }
    }
}