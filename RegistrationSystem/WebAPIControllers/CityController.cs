using System.Web.Http;
using RegistrationSystem.Models;

namespace RegistrationSystem.WebAPIControllers
{
    public class CityController : ApiController
    {
        [Route("api/City")]
        public APIResult Get()
        {
            bool bo = true;
            APIResult re = new APIResult();
            re.Payload = new AreaData().GetCityDataToDB(out bo);
            re.Success = bo;
            re.Message = bo== true ? "OK" : "Error";

            return re;
        }
        [Route("api/City/{_CityID:int}")]
        public APIResult Get(int _CityID)
        {
            bool bo = true;
            APIResult re = new APIResult();
            re.Payload = new AreaData().GetCityDataToDB(out bo, (byte)_CityID);
            re.Success = bo;
            re.Message = bo == true ? "OK" : "Error";

            return re;
        }

        

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}