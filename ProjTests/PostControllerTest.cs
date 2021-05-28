using CoreServices.Controllers;
using CoreServices.Models;
using CoreServices.Repository;
using CoreServices.ViewModel;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ProjTests
{
    public class PostControllerTest
    {
        private PostRepository repository;
        public static DbContextOptions<BlogDBContext> dbContextOptions { get; }
        public static string connectionString = "Server=DANIIL\\SQLEXPRESS;Database=BlogDB;UID=sa;PWD=sa;";
        static PostControllerTest()
        {
            dbContextOptions = new DbContextOptionsBuilder<BlogDBContext>()
                .UseSqlServer(connectionString)
                .Options;
        }
        public PostControllerTest()
        {
            var context = new BlogDBContext(dbContextOptions);
            dataDb db = new dataDb();
            db.Seed(context);

            repository = new PostRepository(context);

        }
        [Fact]
        public async void GetById_returnOkRes()
        {
            var controller = new PostController(repository);
            var postId = 2;
            var data = await controller.GetPost(postId);       
            Assert.IsType<OkObjectResult>(data);
        }
        [Fact]
        public async void GetById_Post_NotFound()
        {
            var controller = new PostController(repository);
            var postId = 10;
            var data = await controller.GetPost(postId);
 
            Assert.IsType<NotFoundResult>(data);
        }
        [Fact]
        public async void GetById_ReturnPost()
        {

            var controller = new PostController(repository);
            int? postId = 1;


            var data = await controller.GetPost(postId);
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var post = okResult.Value.Should().BeAssignableTo<PostViewModel>().Subject;

            Assert.Equal("Test Title 1", post.Title);
            Assert.Equal("Test Description 1", post.Description);
        }
        [Fact]
        public async void AddData_ReturnokResult()
        {
   
            var controller = new PostController(repository);
            var post = new Post() { Title = "Test Title 3", Description = "Test Description 3", CategoryId = 2, CreatedDate = DateTime.Now };
            var data = await controller.AddPost(post);

            Assert.IsType<OkObjectResult>(data);
        }
        [Fact]
        public async void AddData()
        {
     
            var controller = new PostController(repository);
            var post = new Post() { Title = "Test Title 4", Description = "Test Description 4", CategoryId = 2, CreatedDate = DateTime.Now };

            var data = await controller.AddPost(post);
            Assert.IsType<OkObjectResult>(data);
            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            Assert.Equal(3, okResult.Value);
        }
        [Fact]
        public async void GetPosts()
        { 
            var controller = new PostController(repository);
            var data = await controller.GetPosts();

            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var post = okResult.Value.Should().BeAssignableTo<List<PostViewModel>>().Subject;
            Assert.Equal("Test Title 1", post[0].Title);
            Assert.Equal("Test Description 1", post[0].Description);
            Assert.Equal("Test Title 2", post[1].Title);
            Assert.Equal("Test Description 2", post[1].Description);
        }
        [Fact]
        public async void UpdatedData_ReturnOkResult()
        {

            var controller = new PostController(repository);
            var postId = 2;

            var existingPost = await controller.GetPost(postId);
            var okResult = existingPost.Should().BeOfType<OkObjectResult>().Subject;
            var result = okResult.Value.Should().BeAssignableTo<PostViewModel>().Subject;
            var post = new Post();
            post.Title = "Test Title 2 Updated";
            post.Description = result.Description;
            post.CategoryId = result.CategoryId;
            post.CreatedDate = result.CreatedDate;
            var updatedData = await controller.UpdatePost(post);

            Assert.IsType<OkResult>(updatedData);
        }
        [Fact]
        public async void UpdateData_NotFound()
        {

            var controller = new PostController(repository);
            var postId = 2;

            var existingPost = await controller.GetPost(postId);
            var okResult = existingPost.Should().BeOfType<OkObjectResult>().Subject;
            var result = okResult.Value.Should().BeAssignableTo<PostViewModel>().Subject;

            var post = new Post();
            post.PostId = 5;
            post.Title = "qqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqq";
            post.Description = result.Description;
            post.CategoryId = result.CategoryId;
            post.CreatedDate = result.CreatedDate;

            var data = await controller.UpdatePost(post);

            Assert.IsType<NotFoundResult>(data);
        }
        [Fact]
        public async void Delete_ReturnOkResult()
        {
            var controller = new PostController(repository);
            var postId = 2;

            var data = await controller.DeletePost(postId);
            Assert.IsType<OkResult>(data);
        }

        [Fact]
        public async void Delete_NotFound()
        {
            var controller = new PostController(repository);
            var postId = 15;

            var data = await controller.DeletePost(postId);
  
            Assert.IsType<NotFoundResult>(data);
        }
    }
}
