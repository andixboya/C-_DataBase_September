namespace FastFood.Web.Controllers
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;

    using Data;
    using ViewModels.Items;
    using FastFood.Models;
    using Microsoft.EntityFrameworkCore;

    public class ItemsController : Controller
    {
        private readonly FastFoodContext context;
        private readonly IMapper mapper;

        public ItemsController(FastFoodContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public IActionResult Create()
        {
            var categories = this.context.Categories
                .ProjectTo<CreateItemViewModel>(mapper.ConfigurationProvider)
                .ToList();

            return this.View(categories);
        }

        [HttpPost]
        public IActionResult Create(CreateItemInputModel model)
        {

            if (!ModelState.IsValid)
            {
                return this.RedirectToAction("Error", "Home");
            }

            var itemToAdd = this.mapper.Map<Item>(model);
            this.context.Add(itemToAdd);
            this.context.SaveChanges();

            return this.Redirect("/Items/All");
        }

        public IActionResult All()
        {
            var model = this
                .context
                .Items
                .ProjectTo<ItemsAllViewModels>(this.mapper.ConfigurationProvider)
                .ToList();

                return this.View(model);
        }
    }
}
