using Intro_To_Web_Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Intro_To_Web_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ILogger<CartController> _logger;
        private List<Item> _items = new List<Item> { new Item { Id = 1 ,Name = "Iphone", Price = 300.0},
                                    new Item { Id = 2, Name = "Airpods", Price = 100.0 },
                                    new Item { Id = 3, Name = "Beats", Price = 130.0 }};
        public CartController(ILogger<CartController> logger)
        {
            _logger = logger;
        }

        // GET: api/<CartController>
        [HttpGet]
        public IEnumerable<Item> Get()
        {
            return _items;
        }

        // GET api/<CartController>/5
        [HttpGet("{id}")]
        public Item Get(long id)
        {
            return _items.Where(x => x.Id.Equals(id)).Single();
        }

        // POST api/<CartController>
        [HttpPost]
        public ActionResult Post([FromBody] Item item)
        {
            // validate the object and its properties
            if (item.IsValidItem())
            {
                return BadRequest();
            }

            // Generate id
            item.Id = _items.Count + 1;

            // Add new item to list
            _items.Add(item);

            return Ok(_items);
        }

        // PATCH api/<CartController>/5
        [HttpPatch]
        public ActionResult Patch([FromBody] Item item)
        {
            // validate the item and its properties
            if (item.IsValidItem())
            {
                return BadRequest();
            }

            var existingItem = _items.Where(x => x.Id.Equals(item.Id));

            // confirm the item exists in cart
            if (existingItem is null)
            {
                return NotFound();
            }

            // update item in cart
            _items = _items.Where(x => x.Id.Equals(item.Id))
                .Select(i => {
                    i.Name = item.Name; i.Price = item.Price; return i;
                })
                .ToList();

            // return updated item
            return Ok(_items.Where(x => x.Id.Equals(item.Id)));
        }

        // DELETE api/<CartController>/5
        [HttpDelete("{id}")]
        public ActionResult Delete(long id)
        {
            // check if id is valid
            if (id <= 0)
            {
                return BadRequest();
            }

            // find the ietm to delete with id
            var item = _items.Where(x => x.Id.Equals(id));

            // If item does not exist, return not found 
            if (item is null)
            {
                return NotFound();
            }

            // remove it from list
            _items = _items.Where(x => !x.Id.Equals(id)).ToList();

            return Ok(_items);
        }
    }
}
