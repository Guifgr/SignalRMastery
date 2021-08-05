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
        Task DeleteToDoItem(int listId, int itemId);

    }

    public class InMemoryToDoRepository : IToDoRepository
    {
        private static List<ToDoList> Lists { get; set; } = new List<ToDoList>();
        static InMemoryToDoRepository()
        {
            var item = new ToDoItem()
            {
                Id = 0,
                Text = "Abalo"
            };
            var itemList = new List<ToDoItem> { item };
            Lists.Add(new ToDoList()
            {
                Id = 0,
                Name = "Foo",
                Items = itemList
            });

            Lists.Add(new ToDoList()
            {
                Id = 1,
                Name = "Bar",
                Items = new List<ToDoItem>()
            });

            Lists.Add(new ToDoList()
            {
                Id = 2,
                Name = "Test",
                Items = new List<ToDoItem>()
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
        public async Task DeleteToDoItem(int listId, int itemId)
        {
            var itemList = await GetList(listId);
            var item = itemList.Items.FirstOrDefault(i => i.Id == itemId);
            if (item == null) throw new NullReferenceException();
            itemList.Items.Remove(item);
        }
    }
}