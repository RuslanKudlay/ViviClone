using System;
using System.Collections.Generic;
using System.Text;

namespace Application.EntitiesModels.Models
{
    public class OperationResult<T>
    {
        public bool IsSucceded { get; set; }

        public T Result { get; set; }

        public string OperationMessage { get; set; }

        public string ErrorMessage { get; set; }

    }
}
