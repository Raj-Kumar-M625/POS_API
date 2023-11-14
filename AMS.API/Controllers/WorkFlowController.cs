using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectOversight.API.Services.Interface;

namespace ProjectOversight.API.Controllers
{
    [Route("v1/app/[controller]")]
    [ApiController]
    [Authorize]
    public class WorkFlowController : ControllerBase
    {
        private readonly IWorkflowServices _workFlowServices;

        public WorkFlowController(IWorkflowServices workflowServices)
        {
            _workFlowServices = workflowServices;
        }
        [HttpGet("GetEmployeeWorkFlow")]
        public async Task<IActionResult> GetEmployeeWorkFlow(int employeeId,string date)
        {
            try
            {
                var codeTable = await _workFlowServices.GetEmployeeWorkFlow(employeeId,date);
                return Ok(codeTable);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost("CreateEmployeeWorkFlow")]
        public async Task<IActionResult> CreateEmployeeWorkFlow(int employeeId, string Date, string workFlowName)
        {
            try
            {
                var codeTable = await _workFlowServices.CreateEmployeeWorkFlow(employeeId, Date,workFlowName);
                return Ok(codeTable);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet("GetEmployeeTime")]
        public async Task<IActionResult> GetEmployeeTime(int employeeId, string Date)
        {
            try
            {
                var codeTable = await _workFlowServices.GetEmployeeTime(employeeId, Date);
                return Ok(codeTable);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
