﻿using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminTagsController : Controller
    {
        private readonly ITagRepository tagRepository;

        public AdminTagsController(ITagRepository tagRepository)
        {
            this.tagRepository = tagRepository;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest)
        {
            if (addTagRequest.DisplayName != null && addTagRequest.Name != null)
            {
                // Mapping AddTagRequest to Tag domain model
                var tag = new Tag
                {
                    Name = addTagRequest.Name,
                    DisplayName = addTagRequest.DisplayName,
                };

                await tagRepository.AddAsync(tag);

                TempData["SuccessMessage"] = "New tag was added...";
                return RedirectToAction("List");
            }
            else
            {
                TempData["ErrorMessage"] = "Please fill out all fields...";
                return RedirectToAction("Add");
            }
            
        }

        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List()
        {
            // use dbContext to read the tags
            var tags = await tagRepository.GetAllAsync();

            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id) 
        {
            var tag = await tagRepository.GetAsync(id);

            if (tag != null)
            {
                var editTagRequest = new EditTagRequest
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    DisplayName = tag.DisplayName
                };

                return View(editTagRequest);
            }

            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest)
        {
            var tag = new Tag
            {
                Id = editTagRequest.Id,
                Name = editTagRequest.Name,
                DisplayName = editTagRequest.DisplayName
            };

            var updatedTag = await tagRepository.UpdateAsync(tag);

            if (updatedTag != null)
            {
                // Show success notification
                TempData["SuccessMessage"] = "Tag was updated...";
            }
            else
            {
                // Show error notification
                TempData["ErrorMessage"] = "Tag was not updated...";
                return RedirectToAction("Edit", new { id = editTagRequest.Id });

            }

            return RedirectToAction("List");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest)
        {
            var deletedTag = await tagRepository.DeleteAsync(editTagRequest.Id);

            if (deletedTag != null)
            {
                // Show success notification
                TempData["SuccessMessage"] = "Tag was deleted...";
                return RedirectToAction("List");
            }

            // Show an error notification
            TempData["ErrorMessage"] = "Tag was not deleted...";
            return RedirectToAction("Edit", new {id = editTagRequest.Id});
            
        }
    }
}