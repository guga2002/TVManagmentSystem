using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TVManagmentSystem.Models;
using TVManagmentSystem.ResponseRequest;
using TVManagmentSystem.Services;
using TVManagmentSystem.Sources.Enums;

namespace TVManagmentSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChanellController : ControllerBase
    {
        private readonly IUserService ser;

        public ChanellController(IUserService se)
        {
            ser = se;
        }

        [HttpGet("GetChanells")]
        public void LoadData()
        {
            ser.LoadChanels();
        }
        [HttpGet("UpdateRecords")]
        public void info()
        {
            ser.GETAREPORT();
        }

        [HttpGet("Search")]
        public List<InfoResponsee> GetInfoByCHanellName(string Name)
        {
            return ser.GetInfoByCHanellName(Name);
        }

        [HttpGet("EMR")]
        public List<InfoResponsee> GetChanellsByTranscoder(EnumTranscoders choice)
        {
            return ser.GetChanellsByTranscoder(choice);
        }

        [HttpDelete("CleareHistory")]
        public void DeleteHistory()
        {
          ser.DeleteHistory();
        }
    }
}
