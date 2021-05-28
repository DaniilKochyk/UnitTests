using CoreServices.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjTests
{
    public class dataDb
    {
        public dataDb()
        {
        }
        public void Seed(BlogDBContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            context.Category.AddRange(
                new Category() { Name = "name1", Slug = "slug1" },
                new Category() { Name = "name2", Slug = "slug2" },
                new Category() { Name = "name3", Slug = "slug3" },
                new Category() { Name = "name4", Slug = "slug4" }
            );

            context.Post.AddRange(
                new Post() { Title = "Test Title 1", Description = "Test Description 1", CategoryId = 2, CreatedDate = DateTime.Now },
                new Post() { Title = "Test Title 2", Description = "Test Description 2", CategoryId = 3, CreatedDate = DateTime.Now }
            );
            context.SaveChanges();
        }
    }
}

