using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectOversight.API.Data.Model;
using ProjectOversight.API.Services;
using ProjectOversight.API.Services.Interface;

namespace ProjectOversight.API.Controllers
{
    [Route("v1/app/[controller]")]
    [ApiController]
    //[Authorize]
    public class CommonMasterController : ControllerBase
    {
        private readonly ICommonMasterService _commonMasterService;
        private readonly UserManager<User> _userManager;

        public CommonMasterController(ICommonMasterService commonMasterService, UserManager<User> userManager)
        {
            _commonMasterService = commonMasterService;
            _userManager = userManager;
        }

        [HttpGet("GetCodeTableList")]
        public async Task<IActionResult> GetCodeTableList()
        {
            try
            {
                var codeTable = await _commonMasterService.GetCodeTableList();
                return Ok(codeTable);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost("UpdateCommonMaster")]

        public async Task<ActionResult<bool>> UpdateCommonMaster(CommonMaster commonMaster)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _commonMasterService.UpdateCommonMaster(user,commonMaster);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost("AddCommonmaster")]
        public async Task<ActionResult<bool>> AddCommonmaster(CommonMaster commonMaster)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(User.Identity?.Name);
                var result = await _commonMasterService.AddCommonmaster(user,commonMaster);
                return Ok("Commonmaster Created");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
