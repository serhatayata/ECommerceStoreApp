using AutoMapper;
using CatalogService.Api.Data.Repositories.Base;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.BrandModels;
using CatalogService.Api.Utilities.Results;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CatalogService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BrandController(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost]
        [Route("add")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddAsync(BrandAddModel model)
        {
            var requestModel = _mapper.Map<Brand>(model);
            var result = await _unitOfWork.EfBrandRepository.AddAsync(requestModel);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateAsync(BrandUpdateModel model)
        {
            var requestModel = _mapper.Map<Brand>(model);
            var result = await _unitOfWork.EfBrandRepository.UpdateAsync(requestModel);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteAsync(IntModel model)
        {
            var result = await _unitOfWork.EfBrandRepository.DeleteAsync(model);

            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
