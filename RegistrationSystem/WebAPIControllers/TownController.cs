using System.Web.Http;
using RegistrationSystem.Models;

namespace RegistrationSystem.WebAPIControllers
{
    public class TownController : ApiController
    {
        [Route("api/Town/{_CityID:int}")]
        public APIResult Get(int _CityID)
        {
            bool bo = true;
            APIResult re = new APIResult();
            re.Payload = new AreaData().GetTownDataToDB(out bo, (byte)_CityID);
            re.Success = bo;
            re.Message = bo == true ? "OK" : "Error";

            return re;
        }

    }
}