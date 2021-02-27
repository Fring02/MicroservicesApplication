using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping.ApiCollection.Interfaces;
using Shopping.DTOs;
using Shopping.Entities;

namespace Shopping.Pages
{
    public class AddProductModel : PageModel
    {
        private IProductApi _productApi;
        private IMapper _mapper;
        private IWebHostEnvironment _env;
        public AddProductModel(IApiFactory factory, IMapper mapper, IWebHostEnvironment env)
        {
            _productApi = factory.ProductApi ?? throw new ArgumentNullException(nameof(_productApi));
            _mapper = mapper;
            _env = env;
        }
        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAddProductAsync(ProductDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewData["productError"] = "Fill all fields";
                return Page();
            }
            Product product = _mapper.Map<Product>(dto);
            if (dto.File != null)
            {
                string path = "/images/product/" + dto.File.FileName;
                using(var stream = new FileStream(_env.WebRootPath + path, FileMode.OpenOrCreate))
                {
                    await dto.File.CopyToAsync(stream);
                }
                product.ImageFile = dto.File.FileName;
            }
            else
            {
                product.ImageFile = "https://piotrkowalski.pw/assets/camaleon_cms/image-not-found-4a963b95bf081c3ea02923dceaeb3f8085e1a654fc54840aac61a57a60903fef.png";
            }
            await _productApi.CreateProduct(product);
            return RedirectToPage("Index");
        }
    }
}
