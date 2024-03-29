﻿using Application.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public static class HttpVerbHelper
    {
        public static HttpVerb Create(string httpMethod)
        {
            switch (httpMethod.ToUpperInvariant())
            {
                case "GET":
                    return HttpVerb.Get;
                case "POST":
                    return HttpVerb.Post;
                case "PUT":
                    return HttpVerb.Put;
                case "DELETE":
                    return HttpVerb.Delete;
                case "OPTIONS":
                    return HttpVerb.Options;
                case "TRACE":
                    return HttpVerb.Trace;
                case "HEAD":
                    return HttpVerb.Head;
                case "PATCH":
                    return HttpVerb.Patch;
                default:
                    throw new Exception("Unknown HTTP METHOD: " + httpMethod);
            }
        }
    }
}
