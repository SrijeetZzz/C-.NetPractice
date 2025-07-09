// using Microsoft.AspNetCore.Mvc;
// using System.Collections.Generic;
// using System.Linq;
// using MyApi.Models;


// [ApiController]
// [Route("[controller]")]
// public class ProductController : ControllerBase
// {
//     private static List<Product> products = new List<Product>();
//     // {
//     //     new Product { Id = 1, Name = "Laptop" },
//     //     new Product { Id = 2, Name = "Mouse" }
//     // };

//     [HttpGet]
//     public IActionResult GetAll() => Ok(products);

//     [HttpGet("{id}")]
//     public IActionResult GetById(int id)
//     {
//         var product = products.FirstOrDefault(p => p.Id == id);
//         return product == null ? NotFound() : Ok(product);
//     }

//     [HttpPost]
//     public IActionResult Create([FromBody] Product product)
//     {
//         products.Add(product);
//         return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
//     }
// }
// using Microsoft.AspNetCore.Mvc;
// using MyApi.Models;
// using MyApi.Services;

// namespace MyApi.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class ProductController : ControllerBase
//     {
//         private readonly ProductService _service;

//         public ProductController(ProductService service)
//         {
//             _service = service;
//         }

//         [HttpGet]
//         public async Task<IActionResult> GetAll()
//         {
//             var products = await _service.GetAsync();
//             return Ok(products);
//         }

//         [HttpGet("{id:length(24)}")]
//         public async Task<IActionResult> GetById(string id)
//         {
//             var product = await _service.GetAsync(id);
//             if (product == null)
//                 return NotFound();

//             return Ok(product);
//         }

//         [HttpPost]
//         public async Task<IActionResult> Create([FromBody] Product product)
//         {
//             await _service.CreateAsync(product);
//             return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
//         }
//     }
// }
