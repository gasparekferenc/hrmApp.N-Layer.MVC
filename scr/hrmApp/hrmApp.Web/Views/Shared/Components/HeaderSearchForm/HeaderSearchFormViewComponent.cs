﻿using Microsoft.AspNetCore.Mvc;

namespace hrmApp.Web.Views.Shared.Components.HeaderSearchForm
{
    public class HeaderSearchFormViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke() => View();
    }
}