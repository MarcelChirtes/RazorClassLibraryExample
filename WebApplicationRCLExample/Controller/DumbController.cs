using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RCL.MyFeature.Pages;

namespace WebApplicationRCLExample.Controller
{
    public class DumbController : ControllerBase
    {
        private ExpandoObject ViewBag;
        private readonly RazorLight.RazorLightEngine Engine;

        public DumbController()
        {
            // this is a stupid example of razor rendering, razor light is a limited tool in beta, but for the sake of example
            // if you have to use your own razor builder, a good example to follow would be: https://scottsauber.com/2018/07/07/walkthrough-creating-an-html-email-template-with-razor-and-razor-class-libraries-and-rendering-it-from-a-net-standard-class-library/
            // or any other google search for custom  razor builder
            var builder = new RazorLight.RazorLightEngineBuilder();
                builder.UseMemoryCachingProvider();
            dynamic viewBag = new ExpandoObject();
            ViewBag = viewBag;
            Engine = builder.Build();
        }

        [Route("test")]
        [Produces("text/html")]
        [HttpGet]
        public ContentResult GetCompiled()
        {
            Page1Model page1Model = new Page1Model { MyName = "John"};

            var result = Generate(page1Model, @"Areas\MyFeature\Pages\Page1.cshtml");
            return new ContentResult
            {
                ContentType = "text/html",
                Content = result
            };
        }

        private string Generate<T>(T model, string templateRelativePath)
        {
            var t = GenerateAsync(model, templateRelativePath);
            t.Wait();
            return t.Result;
        }

        private async Task<string> GenerateAsync<T>(T model, string templateRelativePath)
        {
            templateRelativePath = GetOsRuntimeBaseDirectoryCombinedPath(templateRelativePath);

            // Try to find template.
            var found = Engine.TemplateCache.RetrieveTemplate(templateRelativePath);
            if (found.Success)
            {
                // If template exists render template
                return await Engine.RenderTemplateAsync(found.Template.TemplatePageFactory(), (object)model, Type.GetTypeFromHandle(typeof(T).TypeHandle), ViewBag);
            }
           
            var template = System.IO.File.ReadAllText(templateRelativePath);
            // Compile and generate template
            return await Engine.CompileRenderAsync(templateRelativePath, template, model, ViewBag);
        }


        private string ToRuntimeOSPath(string path)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                path = path.Replace("/", @"\");

            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                path = path.Replace(@"\", "/");
            }
            else
            {
                throw new Exception("Operating System not supported");
            }
            return path;
        }

        private string GetOsRuntimeBaseDirectoryCombinedPath(string path)
        {
            var runtimePath = ToRuntimeOSPath(AppContext.BaseDirectory);
            path = path.TrimStart('/').TrimStart('\\');
            return Path.Combine(runtimePath, ToRuntimeOSPath(path));
        }
    }
}