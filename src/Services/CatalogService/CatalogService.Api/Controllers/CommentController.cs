﻿using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Models.CommentModels;
using CatalogService.Api.Services.Base.Abstract;
using CatalogService.Api.Utilities.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CatalogService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : BaseController
    {
        private readonly ICommentService _commentService;

        public CommentController(
            ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        [Route("add")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(Result))]
        public async Task<IActionResult> AddAsync([FromBody] CommentAddModel model)
        {
            var result = await _commentService.AddAsync(model);
            return Ok(result);
        }

        [HttpPut]
        [Route("update")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(Result))]
        public async Task<IActionResult> UpdateAsync([FromBody] CommentUpdateModel model)
        {
            var result = await _commentService.UpdateAsync(model);
            return Ok(result);
        }

        [HttpDelete]
        [Route("delete")]
        [ProducesResponseType(typeof(Result), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(Result))]
        public async Task<IActionResult> DeleteAsync([FromBody] IntModel model)
        {
            var result = await _commentService.DeleteAsync(model);
            return Ok(result);
        }

        [HttpGet]
        [Route("get")]
        [ProducesResponseType(typeof(DataResult<CommentModel>), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(DataResult<CommentModel>))]
        public async Task<IActionResult> GetAsync([FromBody] IntModel model)
        {
            var result = await _commentService.GetAsync(model);
            return Ok(result);
        }

        [HttpGet]
        [Route("get-bycode")]
        [ProducesResponseType(typeof(DataResult<CommentModel>), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(DataResult<CommentModel>))]
        public async Task<IActionResult> GetByCodeAsync([FromBody] StringModel model)
        {
            var result = await _commentService.GetByCodeAsync(model);
            return Ok(result);
        }

        [HttpGet]
        [Route("getall")]
        [ProducesResponseType(typeof(DataResult<IReadOnlyList<CommentModel>>), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(DataResult<IReadOnlyList<CommentModel>>))]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _commentService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet]
        [Route("getall-paged")]
        [ProducesResponseType(typeof(DataResult<IReadOnlyList<CommentModel>>), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(DataResult<IReadOnlyList<CommentModel>>))]
        public async Task<IActionResult> GetAllPagedAsync([FromBody] PagingModel model)
        {
            var result = await _commentService.GetAllPagedAsync(model);
            return Ok(result);
        }

        [HttpGet]
        [Route("getall-byproductid")]
        [ProducesResponseType(typeof(DataResult<IReadOnlyList<CommentModel>>), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(DataResult<IReadOnlyList<CommentModel>>))]
        public async Task<IActionResult> GetAllByProductIdAsync([FromBody] IntModel model)
        {
            var result = await _commentService.GetAllByProductId(model);
            return Ok(result);
        }

        [HttpGet]
        [Route("getall-byproductcode")]
        [ProducesResponseType(typeof(DataResult<IReadOnlyList<CommentModel>>), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(DataResult<IReadOnlyList<CommentModel>>))]
        public async Task<IActionResult> GetAllByProductCodeAsync([FromBody] IntModel model)
        {
            var result = await _commentService.GetAllByProductCode(model);
            return Ok(result);
        }

        [HttpGet]
        [Route("getall-byuserid")]
        [ProducesResponseType(typeof(DataResult<IReadOnlyList<CommentModel>>), (int)HttpStatusCode.OK)]
        [ProducesErrorResponseType(typeof(DataResult<IReadOnlyList<CommentModel>>))]
        public async Task<IActionResult> GetAllByUserIdAsync([FromBody] StringModel model)
        {
            var result = await _commentService.GetAllByUserId(model);
            return Ok(result);
        }
    }
}
