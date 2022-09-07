

using Microsoft.AspNetCore.Mvc;

namespace MyWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        //localhost/product
        [HttpGet]
        public ActionResult<string> GetProducts()
        {
            return "tanakorn";
        }

        //localhost/product/1234 => validation
        //localhost/product/{id}
        [HttpGet("{id}")]
        public ActionResult<string> GetProductByID(int id)
        {
            return $"id {id}";
        }

        //localhost/product/search?name=123
        [HttpGet("search")]
        public ActionResult<string> SearchProduct([FromQuery]string name)
        {
            return $"name {name}";
        }
    }
}

