using Microsoft.AspNetCore.Mvc;
using RealtimeToDo.Web.Services;

namespace RealtimeTodo.Web.Api
{
    [Route("teste")]
    public class Notification : Controller
    {
        private readonly INotificationRepository NotificationRepository;

        
        public Notification(INotificationRepository notificationRepository)
        {
            NotificationRepository = notificationRepository;
        }

        [HttpPost("{message}")]
        public void Post(string message)
        {
            Ok(NotificationRepository.SendNotification("Message"));
        }
        [HttpGet("{message}")]
        public void Get(string message)
        {
            Ok(message);
        }
    }
}