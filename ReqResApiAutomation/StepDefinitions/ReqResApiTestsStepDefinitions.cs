using APIServices;
using Newtonsoft.Json;
using ReqResApiServices.Constants;
using ReqResApiServices.DTO;
using ReqResApiServices.Contexts;
using RestSharp;
using System.Net;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using System.Runtime.CompilerServices;
using ReqResApiServices.Helpers;
using System.Text;
using System.Net.Http.Headers;
using System.Security.Policy;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace ReqResApiAutomation.StepDefinitions
{
    [Binding]
    
    public class ReqResApiStepDefinitions
    {
        protected ReqResScenarioContext _scenarioContext;
        HttpClient httpClient;
        HttpWebRequest request;
        GetResponseUserListDTO GetUsersResponseResult = new GetResponseUserListDTO();
        GetResponseColorListDTO GetColorsResponseResult = new GetResponseColorListDTO();
        List<HttpResponseMessage> GetResponseStatusCodes = new List<HttpResponseMessage>();

        public ReqResApiStepDefinitions(ReqResScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext; 
            httpClient = new HttpClient();
        }

        [Given(@"the system has a list of users")]
        public void GivenTheSystemHasAListOfUsers()
        {

        }

        [Given(@"the system has a list of colors")]
        public void GivenTheSystemHasAListOfColors()
        {
            
        }

        [When(@"the application sends a GET request for a user with ID '([^']*)'")]
        public async Task WhenTheApplicationSendsAGETRequestForAUserWithID(string id)
        {
            _scenarioContext.usersResponseData = await(GetRequestByIdAsync<GetResponseUserDTO>(Enums.UsersController, id));
        }

        [When(@"the application sends a GET request for a color with ID '([^']*)'")]
        public async Task WhenTheApplicationSendsAGETRequestForAColorWithID(string id)
        {
            _scenarioContext.colorsResponseData = await (GetRequestByIdAsync<GetResponseColorDTO>(Enums.ColorsController, id));
        }


        [When(@"the application sends a GET request for list of users")]
        public async Task WhenTheApplicationSendsAGETRequestForListOfUsers()
        {
            GetUsersResponseResult = await GetRequestAsync<GetResponseUserListDTO>(Enums.UsersController,1);
            _scenarioContext.userDataList = GetUsersResponseResult.Data;
            var totalPages = Int32.Parse(GetUsersResponseResult.Total_Pages.ToString());
            if (totalPages > 1) 
            {
                for (int page = 2; page <= totalPages; page++)
                {
                    GetUsersResponseResult = await GetRequestAsync<GetResponseUserListDTO>(Enums.UsersController, page);
                    _scenarioContext.userDataList.AddRange(GetUsersResponseResult.Data);
                }
            }
        }

        [When(@"the application sends a GET request for list of colors")]
        public async Task WhenTheApplicationSendsAGETRequestForListOfColors()
        {
            GetColorsResponseResult = await GetRequestAsync<GetResponseColorListDTO>(Enums.ColorsController, 1);
            _scenarioContext.colorDataList = GetColorsResponseResult.Data;
            var totalPages = Int32.Parse(GetColorsResponseResult.Total_Pages.ToString());
            if (totalPages > 1)
            {
                for (int page = 2; page <= totalPages; page++)
                {
                    GetColorsResponseResult = await GetRequestAsync<GetResponseColorListDTO>(Enums.ColorsController, page);
                    _scenarioContext.colorDataList.AddRange(GetColorsResponseResult.Data);
                }
            }
        }


        [When(@"the application sends a POST request to add a new user with name '([^']*)' and job '([^']*)'")]
        public void WhenTheApplicationSendsAPOSTRequestToAddANewUserWithNameAndJob(string nameInput, string jobInput)
        {
            if (nameInput != null && jobInput != null) {
                CreateNewUser(nameInput, jobInput);
            }
        }

        [When(@"the application sends a PUT request to update a user")]
        public void WhenTheApplicationSendsAPUTRequestToUpdateAUser(Table table)
        {
            UpdateUserDTO user = new UpdateUserDTO();
            user = table.CreateInstance<UpdateUserDTO>();
            UpdateUser(user.Name, user.Job, user.Id, Enums.GetEnumDescription(typeof(Enums.RequestMethod), Enums.RequestMethod.Put.ToString()));
        }

        [When(@"the application sends a PATCH request to update a user")]
        public void WhenTheApplicationSendsAPATCHRequestToUpdateAUser(Table table)
        {
            UpdateUserDTO user = new UpdateUserDTO();
            user = table.CreateInstance<UpdateUserDTO>();
            UpdateUser(user.Name, user.Job, user.Id, Enums.GetEnumDescription(typeof(Enums.RequestMethod), Enums.RequestMethod.Patch.ToString()));
        }

        [When(@"the application sends a DELETE request to delete a user with Id '([^']*)'")]
        public void WhenTheApplicationSendsADELETERequestToDeleteAUserWithId(string id)
        {
            DeleteUser(id);
        }


        [Then(@"the following user should be returned")]
        public void ThenTheFollowingUserShouldBeReturned(Table table)
        {
            var user = table.CreateInstance<UserDTO>();
            var response = _scenarioContext.userDataList;
            Assert.True(response.Any(u => u.First_Name == user.First_Name && u.Last_Name == user.Last_Name));
        }

        [Then(@"the user information should be returned with the corresponding error code '([^']*)'")]
        [Then(@"the color information should be returned with the corresponding error code '([^']*)'")]
        public void ThenTheUserInformationShouldBeReturnedWithTheCorrespondingErrorCode(string code)
        {
            var errorCode = code;
            var actualErrorCode = _scenarioContext.responseStatusCode;
            Assert.Equal(errorCode, actualErrorCode);
            
        }


        [Then(@"the user should be created successfully")]
        public void ThenTheUserShouldBeCreatedSuccessfully()
        {
            Assert.Equal(HttpStatusCode.Created.ToString(), _scenarioContext.responseStatusCode);
        }

        [Then(@"the user should be updated successfully")]
        public void ThenTheUserShouldBeUpdatedSuccessfully()
        {
            Assert.Equal(HttpStatusCode.OK.ToString(), _scenarioContext.responseStatusCode);
        }

        [Then(@"the user with Id '([^']*)' should be deleted successfully")]
        public void ThenTheUserWithIdShouldBeDeletedSuccessfully(string p0)
        {
         
        }


        [Then(@"the API should return a list of users")]
        public void ThenTheAPIShouldReturnAListOfUsers()
        {
            Assert.True(_scenarioContext.userDataList.Count > 0);
        }

        [Then(@"the API should return a list of colors")]
        public void ThenTheAPIShouldReturnAListOfColors()
        {
            Assert.True(_scenarioContext.colorDataList.Count > 0);
        }


        public async Task<T> GetRequestByIdAsync<T>(string controller, string id)
        {
            var request = buildRequest(controller, $"/{id}");
            var responseBody = sendRequest(request);
            return (T)JsonConvert.DeserializeObject<T>(responseBody);
        }
        public async Task<T> GetRequestAsync<T>(string controller, int? page = 1)
        {
            var pageApi = $"?page={page}";
            var request = buildRequest(controller, pageApi);
            var responseBody = sendRequest(request);
            Assert.Equal(_scenarioContext.responseStatusCode, HttpStatusCode.OK.ToString());
            return (T)JsonConvert.DeserializeObject<T>(responseBody);
        }

        public async Task CreateNewUser(string name, string job)
        {
            var request = buildRequest(Enums.UsersController);
            request.Method = "POST";
            var stringData = JsonConvert.SerializeObject(new CreateUserDTO()
            {
                Job = job,
                Name = name
            });
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(stringData);
            }

            sendRequest(request);
        }
        public async Task UpdateUser(string name, string job, string id, string method)
        {
            var request = buildRequest(Enums.UsersController, $"/{id}");
            request.Method = method;
            var stringData = JsonConvert.SerializeObject(new UpdateUserDTO()
            {
                Job = job,
                Name = name,
                Id = id
            });
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(stringData);
            }

            sendRequest(request);
        }

        public void DeleteUser(string id)
        {
            var request = buildRequest(Enums.UsersController, $"/{id}");
            request.Method = Enums.GetEnumDescription(typeof(Enums.RequestMethod), Enums.RequestMethod.Delete.ToString());

            sendRequest(request);
        }

        public static HttpWebRequest buildRequest(string controller, string? parameter = null)
        {
            var uri = String.Join("/", Enums.BaseUri, controller) + parameter;
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/109.0.0.0 Safari/537.36";
            request.ContentType = "application/json;charset=UTF-8";
            return request;
        }


        public string sendRequest(HttpWebRequest request)
        {
            string result = null;
            try
            {
                using (HttpWebResponse httpResponse = (HttpWebResponse)request.GetResponse())
                {
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                    }
                    _scenarioContext.responseStatusCode = httpResponse.StatusCode.ToString();
                    return result;
                }
            }
            catch (WebException e)
            {
                using (HttpWebResponse response = (HttpWebResponse)e.Response)
                {
                    using (var streamReader = new StreamReader(response.GetResponseStream()))
                    {
                        result = streamReader.ReadToEnd();
                    }
                    _scenarioContext.responseStatusCode = response.StatusCode.ToString();
                    return result;
                }
            }
            
        }

        
    }
}
