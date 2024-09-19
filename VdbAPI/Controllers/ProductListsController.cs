using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using VdbAPI.Models;
using VdbAPI.ShoppingCartDTO;

namespace VdbAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductListsController : ControllerBase
    {
        private readonly VideoDBContext _context;

        public ProductListsController(VideoDBContext context)
        {
            _context = context;
        }

        // GET: api/ProductLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductList>>> GetProductLists()
        {
            var result = from P in _context.ProductLists
                         select new getProductDTO
                         {
                             ProductId = P.ProductId,
                             ProductName = P.ProductName,
                             ProductPrice = P.ProductPrice,
                             ProductDescription = P.ProductDescription,
                             ProductImg = P.ProductImg,
                         };
            return Ok(await result.ToListAsync());
        }

        // GET: api/ProductLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductList>> GetProductList(int id)
        {
            var productList = await _context.ProductLists.FindAsync(id);

            if (productList == null)
            {
                return NotFound();
            }

            return productList;
        }

        // PUT: api/ProductLists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductList(int id, ProductList productList)
        {
            if (id != productList.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(productList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductListExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ProductLists
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProductList>> PostProductList(ProductList productList)
        {
            _context.ProductLists.Add(productList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProductList", new { id = productList.ProductId }, productList);
        }

        // DELETE: api/ProductLists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductList(int id)
        {
            var productList = await _context.ProductLists.FindAsync(id);
            if (productList == null)
            {
                return NotFound();
            }

            _context.ProductLists.Remove(productList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductListExists(int id)
        {
            return _context.ProductLists.Any(e => e.ProductId == id);
        }
    }
}
