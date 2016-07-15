using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using WhamOnline.Controllers;
using WhamOnline.Models;
using System.Reflection;

namespace WhamOnline.Controllers.Tests
{
    [TestClass()]
    public class WhamGeneratorControllerTests
    {
        private TestWhamGeneratorController m_whamGeneratorController;

        [TestInitialize]
        public void TestSetup()
        { 
            string templatesPath = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\WhamOnline\App_Data\Templates");
            templatesPath = Path.Combine(Directory.GetParent(templatesPath).FullName, "Templates"); // this is to remove the ..\ parts of the path
            WhamOnline.Global.InitWham(templatesPath);
            m_whamGeneratorController = new TestWhamGeneratorController();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (m_whamGeneratorController != null)
            {
                if (m_whamGeneratorController.TaskFolder != null)
                {
                    try
                    {
                        Console.WriteLine("[WGCTBNGNHNB] Clean up, removing task folder: " + m_whamGeneratorController.TaskFolder);
                       // Directory.Delete(m_whamGeneratorController.TaskFolder, true);
                    }
                    catch (Exception x)
                    {
                        Console.WriteLine("[WGCTFZZTCRB] Unable to clean up after test: " + x);
                    }
                }
                else
                {
                    Console.WriteLine("[WGCTXBSZCXB] No task folder to clean up");
                }
            }
            else
            {
                Console.WriteLine("[WGCTGNRCXCC] no WhamGeneratorController to clean up");
            }
        }

        [TestMethod()]
        public async Task Generator_OKResponse_Test()
        {
            var appGen = new TestAppGenConfig();
            await AssertOKResponse(appGen);
        }
         
        [TestMethod()]
        public async Task Generator_ValidationError_Test()
        {
            var appGen = new TestAppGenConfig {AppOptions = {AppName = null}};

            await AssertErrorResponse(appGen);
        } 

        private async Task<string> AssertOKResponse(TestAppGenConfig appGen)
        {
            var responseMessage = await m_whamGeneratorController.PostJsonSchema(appGen);

            Assert.IsNotNull(responseMessage);

            var responseContents = await responseMessage.Content.ReadAsStringAsync();
            dynamic response = JsonConvert.DeserializeObject(responseContents);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.taskId);

            Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode, $"Error [{responseMessage.StatusCode}]: {responseMessage.Content.ReadAsStringAsync().Result}");
            return response.taskId.ToString();
        }

        private async Task AssertErrorResponse(TestAppGenConfig appGen)
        {
            var responseMessage = await m_whamGeneratorController.PostJsonSchema(appGen);

            Assert.IsNotNull(responseMessage);

            var responseContents = await responseMessage.Content.ReadAsStringAsync();
            dynamic response = JsonConvert.DeserializeObject(responseContents);

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.taskId);
            Assert.IsNotNull(response.errors);

            Assert.AreNotEqual(HttpStatusCode.OK, responseMessage.StatusCode, $"Expected an error [{responseMessage.StatusCode}]: {responseMessage.Content.ReadAsStringAsync().Result}");
        }
    }

    internal class TestWhamGeneratorController : WhamGeneratorController
    {
        public string TaskFolder { get; protected set; }
        public string TaskZip { get; protected set; }

        public TestWhamGeneratorController() : base()
        {
            Request = new HttpRequestMessage();
            Request.SetConfiguration(new HttpConfiguration());
        }

        protected override void TaskFolderCreated(string taskFolder)
        {
            TaskFolder = taskFolder;
        }

        protected override void TaskZipFileCreated(string zipFileName)
        {
            TaskZip = zipFileName;
        }
    }

    internal class TestAppGenConfig : AppGenConfig
    {
        public TestAppGenConfig()
        {
            AppOptions = new AppOptions
            {
                AppName = "TestAppName",
                Platform = ".NET",
                Theme = "Basic",
            };

            Authentication = new Authentication()
            {
                ClientId = "test-auth-app-id",
                Type = "AzureAD",
            };

            Database = new Database
            {
                ConnectionString = "http://some-test-conn-string|aaaa-bbbb,ccc\"ddd\"",
                Type = "SQL",
            };

            DataModel = new[] {
                new DataModel
                {
                    IsVisible = true,
                    TableName = "Customers",
                    Fields = new []
                    {
                        new Field
                        {
                            Name = "CustomerNameField",
                            Type = Constants.DataTypes.TString,
                        },
                        new Field
                        {
                            Name = "CustomerEmailField",
                            Type = Constants.DataTypes.TString,
                            Regex = @"\S+@\S+\.\S+",
                        },
                        new Field
                        {
                            Name = "Address",
                            Type = Constants.DataTypes.TRef,
                            RefList = new [] {"Addresses"},
                        },
                    }
                },

                new DataModel
                {
                    IsVisible = true,
                    TableName = "Addresses",
                    Fields = new []
                    {
                        new Field
                        {
                            Name = "AddressNameField",
                            Type = Constants.DataTypes.TString,
                        },
                        new Field
                        {
                            Name = "AddressStreetField",
                            Type = Constants.DataTypes.TString,
                        },
                        new Field
                        {
                            Name = "AddressCityField",
                            Type = Constants.DataTypes.TString,
                        },
                        new Field
                        {
                            Name = "AddressStateField",
                            Type = Constants.DataTypes.TString,
                        },
                        new Field
                        {
                            Name = "AddressZipField",
                            Type = Constants.DataTypes.TString,
                            Regex = @"\d{4,6}(-\d{3,5})?",
                        },
                    }
                }
            };
        }
    }
}