using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseAPI.Classes
{
    public class WebProperties
    {
        public string Name { get; set; }
        public List<string> ConnectionStrings { get; set; } = new List<string>();
        public string ConnectionString { get; set; } = string.Empty;
        public WebApplicationBuilder Builder { get; set; }

        public WebProperties(string name, WebApplicationBuilder builder)
        {
            Name = name;
            Builder = builder;

        }
    }
}
