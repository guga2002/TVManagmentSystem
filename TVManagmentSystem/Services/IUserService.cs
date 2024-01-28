using TVManagmentSystem.Models;
using TVManagmentSystem.ResponseRequest;
using TVManagmentSystem.Sources.Enums;

namespace TVManagmentSystem.Services
{
    public interface IUserService
    {
        void LoadChanels();
        void GETAREPORT();
        List<InfoResponsee> GetInfoByCHanellName(string Name);
        List<InfoResponsee> GetChanellsByTranscoder(EnumTranscoders choice);
        void DeleteHistory();
    }
}
