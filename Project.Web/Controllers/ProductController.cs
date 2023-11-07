using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project.Web.Models;
using Project.Web.Service.IService;

namespace Project.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto> list = new();

            ResponseDto? response = await _productService.GetAllProductsAsync();
            if (response != null && response.IsSucess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(list);
        }

        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _productService.CreateProductsAsync(productDto);
                if (response != null && response.IsSucess)
                {
                    TempData["success"] = "Product created successfully";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }
            return View(productDto);
        }

        public async Task<IActionResult> ProductDelete(int ProductId)
        {
            ResponseDto? response = await _productService.GetProductByIdAsync(ProductId);

            if (response != null && response.IsSucess)
            {
                ProductDto? model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDto productDto)
        {
            ResponseDto? response = await _productService.DeleteProductsAsync(productDto.ProductId);

            if (response != null && response.IsSucess)
            {
                TempData["success"] = "Product deleted successfully";
                return RedirectToAction(nameof(ProductIndex));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(productDto);
        }



        public async Task<IActionResult> ProductEdit(int ProductId)
        {
            ResponseDto? response = await _productService.GetProductByIdAsync(ProductId);

            if (response != null && response.IsSucess)
            {
                ProductDto? model = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
     


                return View(model);
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductDto productDto)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _productService.UpdateProductsAsync(productDto);

                if (response.IsSucess)
                {
                    // Update ImageUrl and ImageLocalPath based on the response
                  
                    //productDto.ImageLocalPath = response.Result.;

                    TempData["success"] = "Product updated successfully";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response.Message;
                }
            }

            return View(productDto);
        }

    }
}
