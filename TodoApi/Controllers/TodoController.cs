using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {

        private readonly TodoContext _context;
        private readonly WebClient _httpClient = new WebClient();
       
        public TodoController(TodoContext context)
        {
            _context = context;
            
            if ( !_context.TodoItems.Any() )
            {
                _context.TodoItems.Add(new TodoItem { Name = "Buy Batman Costume" });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<TodoItem> GetAll()
        {
            return _context.TodoItems.ToList();
        }

        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(long id)
        {
            var item = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if ( item == null)
            {
                return NotFound();
            }
            
            var task =  PUBG().Result;
            dynamic dude = JsonConvert.DeserializeObject(task);
            
            return new ObjectResult(dude);
        }

        [HttpPost]
        public IActionResult Create([FromBody] TodoItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _context.TodoItems.Add(item);
            _context.SaveChanges();
            
            return CreatedAtRoute("GetTodo", new { id = item.Id }, item);
        }

        [HttpPut]
        public IActionResult Update(long id, [FromBody] TodoItem item)
        {
            if (item == null || item.Id != id )
            {
                return BadRequest();
            }

            var todo = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if ( todo == null)
            {
                return NotFound();
            }

            todo.IsComplete = item.IsComplete;
            todo.Name = item.Name;

            _context.TodoItems.Update(todo);
            _context.SaveChanges();
            
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var todo = _context.TodoItems.FirstOrDefault(t => t.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todo);
            _context.SaveChanges();
            
            return new NoContentResult();
        }

        private async Task<string> PUBG()
        {
            return await _httpClient.DownloadStringTaskAsync("https://jsonplaceholder.typicode.com/posts/1");
            
        }
    }
}