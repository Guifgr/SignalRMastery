using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RealtimeTodo.Web.Models;

namespace RealtimeToDo.Web.Services
{
    public interface IToDoRepository
    {
        Task<List<ToDoList>> GetLists();
        Task<ToDoList> GetList(int id);
        Task AddToDoItem(int listId, string text);
        Task ToggleToDoItem(int listId, int itemId);
        
    }

    public class InMemoryToDoRepository : IToDoRepository
    {
        private static List<ToDoList> Lists { get; set; } = new List<ToDoList>();

        static InMemoryToDoRepository()
        {
            Lists.Add(new ToDoList()
            {
                Id = 0,
                Name = "Foo"
            });

            Lists.Add(new ToDoList()
            {
                Id = 1,
                Name = "Bar"
            });

            Lists.Add(new ToDoList()
            {
                Id = 2,
                Name = "Test"
            });
        }

        public async Task<List<ToDoList>> GetLists()
        {
            return await Task.FromResult(Lists);
        }

        public async Task<ToDoList> GetList(int id)
        {
            var result = await Task.FromResult(Lists.FirstOrDefault(list => list.Id == id));
            if (result == null) throw new NullReferenceException();
            return result;
        }

        public async Task AddToDoItem(int listId, string text)
        {
            var list = await GetList(listId);
            list.AddItem(text);
        }

        public async Task ToggleToDoItem(int listId, int itemId)
        {
            var itemList = await GetList(listId);
            itemList.Toggle(itemId);
        }
    }
}