using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using FastReport.Olap.Web;
using FastReport.Olap.Cube;
using FastReport.Olap.Slice;

namespace KnockWebReport.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private IHostingEnvironment _env;
        public SampleDataController(IHostingEnvironment env)
        {
            _env = env;
        }

        [HttpGet("[action]")]
        public IActionResult ShowCube(string name)
        {
            Cube cube = new Cube();
            Slice slice = new Slice()
            {
                Cube = cube
            };
            FilterManager filterManager = new FilterManager()
            {
                Cube = cube
            };
            WebGrid grid;
            grid = new WebSliceGrid()
            {
                Slice = slice

            };

            ViewBag.WebGrid = grid;

            cube.SourceType = SourceType.File;
            cube.Load(Path.Combine(_env.WebRootPath,(String.Format("App_Data/{0}",name))));
            return View(cube);

        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);
            var webRoot = _env.WebRootPath;
            var filePath = System.IO.Path.Combine(webRoot, (String.Format("App_Data/{0}", files[0].FileName)));

            if (files[0].Length > 0)
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await files[0].CopyToAsync(stream);
                    stream.Close();
                }
            }
            Task.WaitAll();
            return Content(Path.GetFileName(filePath));
        }
    }
}