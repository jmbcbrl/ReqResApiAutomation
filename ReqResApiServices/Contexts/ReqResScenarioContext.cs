using ReqResApiServices.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ReqResApiServices.Contexts
{
    public class ReqResScenarioContext
    {
        public ReqResScenarioContext()
        {
            GetResponseUserDTO usersResponseData = new GetResponseUserDTO();
            GetResponseColorDTO colorsResponseData = new GetResponseColorDTO();
            List<UserDTO> userDataList = new List<UserDTO>();
            List<GetResponseUserDTO> responseList = new List<GetResponseUserDTO>();
            HttpResponseMessage responseMessage= new HttpResponseMessage();
        }
        public GetResponseUserDTO usersResponseData { get; set; }
        public GetResponseColorDTO colorsResponseData { get; set; }
        public List<UserDTO> userDataList { get; set; }
        public List<ColorDTO> colorDataList { get; set; }
        public string responseStatusCode { get; set; }
    }
}
