using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Comments.API.Dtos;
using Comments.API.Interfaces;
using Comments.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Comments.API.Controllers
{
    [Route("api/v1/[controller]/")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsRepository _repos;
        private readonly IMapper _mapper;
        public CommentsController(ICommentsRepository repos, IMapper mapper)
        {
            _repos = repos;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllComments(Guid id, string productId)
        {
            if(id != Guid.Empty)
            {
                return Ok(await _repos.GetById(id));
            }
            if (!string.IsNullOrEmpty(productId))
            {
                return Ok(await _repos.GetCommentsByProductId(productId));
            }
            var comments = await _repos.GetAll();
            return Ok(comments);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment(CommentDto dto)
        {
            if (!ModelState.IsValid) return BadRequest("Fill all comments field");
            var comment = _mapper.Map<Comment>(dto);
            if (await _repos.Create(comment)) return Ok("Created comment");
            return StatusCode(500);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var comment = await _repos.GetById(id);
            if (comment == null) return BadRequest("No such comment");
            if (await _repos.Delete(comment)) return Ok("Deleted comment");
            return StatusCode(500);
        }

        [HttpPut]
        public async Task<IActionResult> DeleteComment(UpdateCommentDto dto)
        {
            var comment = await _repos.GetById(dto.Id);
            if (comment == null) return BadRequest("No such comment");
            if (!string.IsNullOrEmpty(dto.Description)) comment.Description = dto.Description;
            if (await _repos.Update(comment)) return Ok("Updated comment");
            return StatusCode(500);
        }
    }
}
