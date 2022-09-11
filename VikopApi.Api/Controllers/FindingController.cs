using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VikopApi.Api.DTO;
using VikopApi.Api.Infrastructure.AuthManager;
using VikopApi.Application.Findings;

namespace VikopApi.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class FindingController : ControllerBase
    {
        /// <summary>
        /// Gets all findings
        /// </summary>
        /// <response code="200">
        /// List of findings containing:
        /// * id
        /// * title
        /// * description
        /// * creatorName
        /// * creatorId
        /// * link
        /// * commentCount
        /// * created - creation date
        /// </response>
        [HttpGet]
        public IActionResult GetAll([FromServices] GetFindings getFindings)
            => Ok(getFindings.Execute());

        /// <summary>
        /// Gets finding with given id
        /// </summary>
        /// <response code="200">
        /// Finding model:
        /// * id
        /// * title
        /// * description
        /// * creatorName
        /// * creatorId
        /// * link
        /// * commentCount
        /// * created - creation date
        /// * comments: 
        ///     - content
        ///     - created
        ///     - id
        ///     - creatorName
        ///     - creatorId
        /// </response>
        [HttpGet("{id}")]
        public IActionResult Get(int id, [FromServices] GetFinding getFinding)
            => Ok(getFinding.Execute(id));

        /// <summary>
        /// Adds new finding
        /// </summary>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(
            AddFindingModel request,
            [FromServices] AddFinding addFinding,
            [FromServices] IAuthManager authManager) 
            => Ok(await addFinding.Execute(new AddFinding.Request
                {
                    Title = request.Title,
                    CreatorId = authManager.GetCurrentUserId(),
                    Link = request.Link,
                    Description = request.Description,
                    Picture = request.Picture
                }));
    }
}
