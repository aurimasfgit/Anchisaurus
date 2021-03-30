using Ondato.Anchisaurus.Core.Models.Entities.Base;
using System;
using System.Collections.Generic;

namespace Ondato.Anchisaurus.Core.Models.Entities
{
    public class KeyValuePairWithExpiration : Entity
    {
        public string Key { get; set; }

        public IList<object> Value { get; set; }

        public DateTime ExpiresAt { get; set; }
    }
}