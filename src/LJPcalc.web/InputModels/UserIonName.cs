using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LJPcalc.web.InputModels
{
    public class UserIonName
    {
        public string ErrorMessage => string.IsNullOrWhiteSpace(Input) ? "name required" : null;
        public bool IsValid => string.IsNullOrEmpty(ErrorMessage);
        public int Charge;
        public string Input;
    }
}
