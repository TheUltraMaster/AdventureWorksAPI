﻿using Microsoft.AspNetCore.Mvc;
using AdventureWorksNS.Data;
using AdventureWorksAPI.Repositories;


namespace AdventureWorksAPI.Controllers
{
    [Route("ProductCategory/[controller]")]
    [ApiController]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IProductCategoryRepository repo;

        public ProductCategoryController(IProductCategoryRepository repo)
        {
            this.repo = repo;
        }




        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProductCategory>))]
        public async Task<IEnumerable<ProductCategory>> ProductCategories(string? Name)
        {
            if (string.IsNullOrEmpty(Name))
            {
                return (IEnumerable<ProductCategory>)await repo.RetrieveAllAsync();
            }
            else
            {
                return (IEnumerable<ProductCategory>)(await repo.RetrieveAllAsync())
                        .Where(ProductCategory => ProductCategory.Name == Name);
            }
        }

        [HttpGet("{id}", Name = nameof(GetProductCategory))] //Ruta
        [ProducesResponseType(200, Type = typeof(ProductCategory))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetProductCategory(int id)
        {
           ProductCategory? c = await repo.RetrieveAsync(id);
            if (c == null)
            {
                return NotFound(); //Error 404
            }
            return Ok(c);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ProductCategory))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] ProductCategory c)
        {
            if (c == null)
            {
                return BadRequest(); //400
            }
            ProductCategory? addProductCategory = await repo.CreateAsync(c);
            if (addProductCategory == null)
            {
                return BadRequest("El repositorio fallo al crear la cateogria de producto");
            }
            else
            {
                return CreatedAtRoute(
                        routeName: nameof(GetProductCategory),
                        routeValues: new { id = addProductCategory.ProductCategoryId },
                        value: addProductCategory);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] ProductCategory c)
        {
            if (c == null || c.ProductCategoryId != id)
            {
                return BadRequest(); //400
            }
            ProductCategory? existe = await repo.RetrieveAsync(id);
            if (existe == null)
            {
                return NotFound(); //404
            }
            await repo.UpdateAsync(id, c);
            return new NoContentResult(); //204
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            ProductCategory? existe = await repo.RetrieveAsync(id);
            if (existe == null)
            {
                return NotFound(); //404
            }
            bool? deleted = await repo.DeleteAsync(id);
            if (deleted.HasValue && deleted.Value)
            {
                return new NoContentResult(); //201
            }
            return BadRequest($"Cliente con el id {id} no se pudo borrar");
        }

    }







}

