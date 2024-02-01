using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TVManagmentSystem.Services;

namespace TVManagmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IexcellService ser;

        public ValuesController(IexcellService se)
        {
            ser = se;
        }

        [HttpGet("Print")]
        public void print()
        {
            ser.UpdateExcell();
        }

        [HttpGet("Comments")]
        public void com()
        {
            ser.DisplayComments();
        }

    }
}
