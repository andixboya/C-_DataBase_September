﻿namespace FastFood.Web.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using System;

    using Data;
    using ViewModels.Categories;
    using FastFood.Models;
    using AutoMapper.QueryableExtensions;
    using System.Linq;

    public class CategoriesController : Controller
    {
        private readonly FastFoodContext context;
        private readonly IMapper mapper;

        public CategoriesController(FastFoodContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateCategoryInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.RedirectToAction("Error", "Home");
            }

            var categoryToAdd = this.mapper.Map<Category>(model);

            context.Categories.Add(categoryToAdd);
            context.SaveChanges();

            return this.Redirect("/Categories/All");
        }

        [HttpGet]
        public IActionResult All()
        {
            var viewModel = this
                .context
                .Categories
                .ProjectTo<CategoryAllViewModel>(this.mapper.ConfigurationProvider)
                .ToList();


            return this.View(viewModel);
        }
    }
}
