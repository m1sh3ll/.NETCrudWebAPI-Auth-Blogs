﻿using DotNetAPI2.Models;

namespace DotNetAPI2.Dtos
{
  public class CategoryUpdateDto
  {

    public string Name { get; set; }

    public string UrlHandle { get; set; }

    public List<BlogPost> BlogPosts { get; set; }
  }
}
