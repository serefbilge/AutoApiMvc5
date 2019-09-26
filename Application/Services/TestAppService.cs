using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TestAppService: ITestAppService
    {
        public string GetTestResult()
        {
            return "OK";
        }
    }
}
