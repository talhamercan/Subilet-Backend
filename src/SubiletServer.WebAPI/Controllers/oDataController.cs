﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;


namespace SubiletServer.WebAPI.Controllers



{   
    [Route("api/[controller]")]
    [ApiController]
    [EnableQuery]
    public class oDataController : ControllerBase
    {
        public static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new(); // Use ODataConventionModelBuilder
            builder.EnableLowerCamelCase();
            return builder.GetEdmModel();
        }
    }
}
