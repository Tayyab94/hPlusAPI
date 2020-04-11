using CoreApiPractice.Classes;
using CoreApiPractice.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApiPractice.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
  [Authorize]
    [Route("api/[controller]")]

   
        // To Configure the APi Versioning route
    // [Route("v{v:apiversion}/product")] 
 
        
        // when we define the Api-Verion in The Confiugation fine. in StartUp.cs file
 
    
    //[Route("products")]
    public class Productsv1Controller : ControllerBase
    {
        private readonly ShopContext _context;

        public Productsv1Controller(ShopContext context)
        {
            this._context = context;

            _context.Database.EnsureCreated();
        
        }

        [HttpGet]
        public IEnumerable<Product> GetAllProducts([FromQuery] QueryParameters queryParameters)
        {
            return _context.Products.ToArray();
        }


        // QueryParameter class is using for query the data with paging concept
        [HttpGet]
        [Route("ListProduct")]
        public async Task<IActionResult> GetAllProduct([FromQuery] ProductQueryParameters queryParameters)
        {

            
            IQueryable<Product> products = _context.Products.Where(s=>s.IsAvailable==true);


            // thesee to if conditions just check the some filters which we want to apply or not...

            if (queryParameters.MinPrice != null && queryParameters.MaxPrice != null)
                products = products
                    .Where(s => s.Price >= queryParameters.MinPrice && s.Price <= queryParameters.MaxPrice);
            
            if (!string.IsNullOrEmpty(queryParameters.Sku))
            {
                products = products
                     .Where(s => s.Sku.ToLower()
                     .Contains(queryParameters.Sku.ToLower()));
            }

            if (!string.IsNullOrEmpty(queryParameters.Name))
            {
                products = products
                     .Where(s => s.Name.ToLower()
                     .Contains(queryParameters.Name.ToLower()));
            }

            if(!string.IsNullOrEmpty(queryParameters.SortBy))
            {
                if(typeof(Product).GetProperty(queryParameters.SortBy)!=null)
                {
                    products = products.OrderByCustom(queryParameters.SortBy, queryParameters.SortOrder);
                }
            }

            products = products.Skip(queryParameters.Size * (queryParameters.Page - 1)).Take(queryParameters.Size);
            if (products==null){ return NotFound();}    

            return Ok(await products.Include(x=>x.Category).ToListAsync());
        }


        // This Mehtod is Returning new product..... 

        [HttpGet, Route("productid/{id?}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product =await _context.Products.FindAsync(id);

            return Ok(product);
        }


        [HttpPost]

        public async Task<IActionResult> CreateProduct([FromBody] Product model)
        {
            _context.Products.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductById", new { id = model.Id, Controller="Products"}, model);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id,[FromBody]Product product)
        {
            if(id==null)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                if (_context.Products.Find(id) == null)
                    return NotFound();
               
            }

            return NoContent();

        }


        // For deleting the Product.... 
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null) return NotFound();


            _context.Products.Remove(product);

          await  _context.SaveChangesAsync();

            return product;
             
        }

    }




    [ApiVersion("2.0")]
    [ApiController]
    //  [Route("api/[controller]")]
    // To Configure the APi Versioning route
    [Route("v{v:apiversion}/product")]
    public class Productsv2_oController : ControllerBase
    {
        private readonly ShopContext _context;

        public Productsv2_oController(ShopContext context)
        {
            this._context = context;

            _context.Database.EnsureCreated();

        }

        [HttpGet]
        public IEnumerable<Product> GetAllProducts([FromQuery] QueryParameters queryParameters)
        {
            return _context.Products.ToArray();
        }


        // QueryParameter class is using for query the data with paging concept
        [HttpGet]
        [Route("ListProduct")]
        public async Task<IActionResult> GetAllProduct([FromQuery] ProductQueryParameters queryParameters)
        {


            IQueryable<Product> products = _context.Products; 


            // thesee to if conditions just check the some filters which we want to apply or not...

            if (queryParameters.MinPrice != null && queryParameters.MaxPrice != null)
                products = products
                    .Where(s => s.Price >= queryParameters.MinPrice && s.Price <= queryParameters.MaxPrice);

            if (!string.IsNullOrEmpty(queryParameters.Sku))
            {
                products = products
                     .Where(s => s.Sku.ToLower()
                     .Contains(queryParameters.Sku.ToLower()));
            }

            if (!string.IsNullOrEmpty(queryParameters.Name))
            {
                products = products
                     .Where(s => s.Name.ToLower()
                     .Contains(queryParameters.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(queryParameters.SortBy))
            {
                if (typeof(Product).GetProperty(queryParameters.SortBy) != null)
                {
                    products = products.OrderByCustom(queryParameters.SortBy, queryParameters.SortOrder);
                }
            }

            products = products.Skip(queryParameters.Size * (queryParameters.Page - 1)).Take(queryParameters.Size);
            if (products == null) { return NotFound(); }

            return Ok(await products.ToListAsync());
        }


        // This Mehtod is Returning new product..... 

        [HttpGet, Route("productid/{id?}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _context.Products.FindAsync(id);

            return Ok(product);
        }


        [HttpPost]

        public async Task<IActionResult> CreateProduct([FromBody] Product model)
        {
            _context.Products.Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductById", new { id = model.Id, Controller = "Products" }, model);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody]Product product)
        {
            if (id == null)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                if (_context.Products.Find(id) == null)
                    return NotFound();

            }

            return NoContent();

        }


        // For deleting the Product.... 
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null) return NotFound();


            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            return product;

        }

    }
}