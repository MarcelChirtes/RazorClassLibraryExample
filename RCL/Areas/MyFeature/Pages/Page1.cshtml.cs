﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RCL.MyFeature.Pages
{
    public class Page1Model : PageModel
    {
        public string MyName { get; set; }

        public void OnGet()
        {

        }
    }
}