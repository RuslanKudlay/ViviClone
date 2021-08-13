using System;
using System.Collections.Generic;
using System.Text;

namespace Application.BBLInterfaces.BusinessServicesInterfaces
{
    public interface IModelMapper
    {
        OUT MapTo<IN, OUT>(IN innerModel, OUT outModel);

        OUT MapTo<IN, OUT>(IN innerModel);
    }
}
