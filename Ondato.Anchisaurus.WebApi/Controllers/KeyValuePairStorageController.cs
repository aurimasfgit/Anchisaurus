using Microsoft.AspNetCore.Mvc;
using Ondato.Anchisaurus.Core.Interfaces.Services;
using Ondato.Anchisaurus.WebApi.Controllers.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ondato.Anchisaurus.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class KeyValuePairStorageController : BaseController
    {
        private readonly IKeyValuePairWithExpirationService keyValuePairWithExpirationService;

        public KeyValuePairStorageController(IKeyValuePairWithExpirationService keyValuePairWithExpirationService)
        {
            if (keyValuePairWithExpirationService == null)
                throw new ArgumentNullException(nameof(keyValuePairWithExpirationService));

            this.keyValuePairWithExpirationService = keyValuePairWithExpirationService;
        }

        [HttpPost("{key}")]
        public ActionResult Create(
            [Required] string key, IList<object> value,
            [Range(0, int.MaxValue)] int? expirationTimeInSeconds = null)
        {
            keyValuePairWithExpirationService.AddOrUpdate(key, value, expirationTimeInSeconds);

            return Ok();
        }

        [HttpPut("{key}")]
        public ActionResult Append([Required] string key, IList<object> value)
        {
            keyValuePairWithExpirationService.AddOrAppend(key, value);

            return Ok();
        }

        [HttpDelete("{key}")]
        public ActionResult Delete([Required] string key)
        {
            keyValuePairWithExpirationService.Remove(key);

            return Ok();
        }

        [HttpGet("{key}")]
        public ActionResult<IList<object>> Get([Required] string key)
        {
            return Ok(keyValuePairWithExpirationService.Get(key));
        }
    }
}