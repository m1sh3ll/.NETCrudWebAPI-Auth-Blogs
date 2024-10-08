﻿using DotNetAPI2.Dtos;
using DotNetAPI2.Models;
using DotNetAPI2.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetAPI2.Controllers
{
  //http://localhost:xxxx/api/categories

  [Route("api/[controller]")]
  [ApiController]
  public class CategoriesController : ControllerBase
  {

    private readonly ICategoryRepository _categoryRepository;


    public CategoriesController(ICategoryRepository categoryRepository)
    {
      this._categoryRepository = categoryRepository;
    }



    // GET: https://localhost:7226/api/Categories?query=html&sortBy=name&sortDirection=desc
    [HttpGet]    
    public async Task<IActionResult> GetAllCategories(
    [FromQuery] string? query, 
    [FromQuery] string? sortBy, 
    [FromQuery] string sortDirection,
    [FromQuery] int? pageNumber,
    [FromQuery] int? pageSize)  
    {
      var categories = await _categoryRepository.GetAllAsync(
      query, 
      sortBy, 
      sortDirection,
      pageNumber,
      pageSize);

      var response = new List<CategoryDto>();
      // map domain to dto
      foreach (var category in categories)
      {
        response.Add(new CategoryDto
        {
          Id = category.Id,
          Name = category.Name,
          UrlHandle = category.UrlHandle
        });
      }
      return Ok(response);
    }



    //api/categories/{guid}}
    [HttpGet]
    [Route("{id:Guid}")]   
    public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
    {
      var category = await _categoryRepository.GetById(id);

      if (category is null)
      {
        return NotFound();
      }

      var response = new CategoryDto
      {
        Id = category.Id,
        Name = category.Name,
        UrlHandle = category.UrlHandle
      };
      return Ok(response);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto categoryCreateDto)
    {
      // Map DTO to Domain Model
      Category category = new Category
      {
        Name = categoryCreateDto.Name,
        UrlHandle = categoryCreateDto.UrlHandle
      };

      category = await _categoryRepository.CreateAsync(category);

      //Domain model to Dto
      var response = new CategoryDto
      {
        Id = category.Id,
        Name = category.Name,
        UrlHandle = category.UrlHandle
      };
      return Ok(response);
    }

    //api/categories/{id}
    [HttpDelete("{id:Guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteCategory([FromRoute] Guid id){
      var category = await _categoryRepository.DeleteAsync(id);
      if(category is null) {
        return NotFound();
      }
      //convert domain to dto
      var response = new CategoryDto
      {
        Id = category.Id,
        Name = category.Name,
        UrlHandle = category.UrlHandle

      };

      return Ok(response);

    }


    //api/categories/{id}
    [HttpPut("{id:Guid}")]
    [Authorize]
    public async Task<IActionResult> EditCategory([FromRoute] Guid id, CategoryUpdateDto categoryUpdateDto)
    {
      //convert dto to Domain model
      var category = new Category
      {
        Id = id,
        Name = categoryUpdateDto.Name,
        UrlHandle = categoryUpdateDto.UrlHandle
      };

      category = await _categoryRepository.UpdateAsync(category);

      if (category is null)
      {
        return NotFound();
      }

      //convert domain model to DTO
      var response = new CategoryDto
      {
        Id = id,
        Name = categoryUpdateDto.Name,
        UrlHandle = categoryUpdateDto.UrlHandle
      };

      return Ok(response);
    }



    //GET: https://domain/api/categories/count
    [HttpGet]
    [Route("count")]
    public async Task<IActionResult> GetCategoriesTotal() {
      var count = await _categoryRepository.GetCount();
      return Ok(count);

    }

  }
}
