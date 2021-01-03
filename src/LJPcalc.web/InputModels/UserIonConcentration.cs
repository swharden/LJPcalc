using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LJPcalc.web.InputModels
{
    public class UserIonConcentration
    {
        public string ErrorMessage;
        public bool IsValid => string.IsNullOrEmpty(ErrorMessage);

        private string input;
        public double Concentration;

        public string Input
        {
            get => input;
            set
            {
                input = value;
                bool canParse = double.TryParse(input, out Concentration);

                if (canParse == false)
                    ErrorMessage = "invalid concentration";
                else if (Concentration < 0)
                    ErrorMessage = "concentration cannot be negative";
                else
                    ErrorMessage = null;
            }
        }
    }
}
