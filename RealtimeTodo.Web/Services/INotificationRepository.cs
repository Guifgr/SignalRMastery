using System.Threading.Tasks;
using RealtimeTodo.Web.Hubs;

namespace RealtimeToDo.Web.Services
{
    public interface INotificationRepository
    {
        Task SendNotification(string message);
    }

    public class NotificationRepository : INotificationRepository
    {
        private readonly ToDoHub _toDoHub;

        public NotificationRepository(ToDoHub toDoHub)
        {
            _toDoHub = toDoHub;
        }

        public async Task SendNotification(string message)
        {
             await _toDoHub.Notification(message);
        }
    }
}