﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unic.Flex.Model.ViewModel.Fields.InputFields
{
    public class MultilineTextFieldViewModel : InputFieldViewModel<string>
    {
        public virtual string Rows { get; set; }
    }
}
