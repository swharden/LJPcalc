using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LJPcalc.web.InputModels
{
    public class UserTemperature
    {
        public string ErrorMessage;
        public bool IsValid => string.IsNullOrEmpty(ErrorMessage);

        private string input;
        public double TemperatureC = 25;

        public string Input
        {
            get => IsValid ? TemperatureC.ToString() : input;
            set
            {
                input = value;
                bool canParse = double.TryParse(input, out TemperatureC);
                ErrorMessage = canParse ? "" : "invalid temperature";
            }
        }
    }
}
