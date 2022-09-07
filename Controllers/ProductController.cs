

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
            if (id == 0)
            {
                return NotFound();
            }
            return $"id {id}";
        }

        //localhost/product/search?name=123
        [HttpGet("search")]
        public ActionResult<string> SearchProduct([FromQuery]string name)
        {
            return $"name {name}";
        }

        //localhost/product
        [HttpPost]
        public ActionResult<string> AddProduct ()
        {
            return "post";
        }

        //localhost/product
        [HttpPut]
        public ActionResult<string> EditProduct()
        {
            return "put";
        }

        //localhost/product
        [HttpDelete]
        public ActionResult<string> DeleteProduct()
        {
            return "delete";
        }
    }
}

