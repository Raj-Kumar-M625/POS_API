using Comments.API.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Comments.API.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Comments.API.Controllers
{
    [Route("v1/app/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentsController : Controller
    {
        public readonly ICommentService _commentService;
        private readonly UserManager<User> _userManager;
        public CommentsController(ICommentService commentService, UserManager<User> userManager)
        {
            _commentService = commentService;
            _userManager = userManager;
        }

        [HttpGet("GetComments")]
        public async Task<ActionResult<CommentHierarchy>> GetComments()
        {
            try
            {
                var comments = await _commentService.GetComments();
                return Ok(comments);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("AddComment")]
        public async Task<ActionResult<bool>> AddComment(CommentHierarchy commentHierarchy)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var comments = await _commentService.AddComment(user,commentHierarchy);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost("UpdateComment")]
        public async Task<ActionResult<bool>> UpdateComment(CommentHierarchy commentHierarchy)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var comments = await _commentService.UpdateComment(user,commentHierarchy);
                return Ok(comments);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}