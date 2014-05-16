﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.ModelBinding
{
    using Unic.Flex.Model.Forms;

    public interface IModelConverterService
    {
        FormViewModel ConvertToViewModel(Form form);
    }
}
