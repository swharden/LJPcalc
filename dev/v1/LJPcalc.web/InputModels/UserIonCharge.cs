using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LJPcalc.web.InputModels
{
    public class UserIonCharge
    {
        public string ErrorMessage;
        public bool IsValid => string.IsNullOrEmpty(ErrorMessage);

        private string input;
        public int Charge;
        private string PrettyCharge => ((Charge < 0) ? "" : "+") + Charge.ToString();

        public string Input
        {
            get => IsValid ? PrettyCharge : input;
            set
            {
                input = value;
                bool canParse = int.TryParse(input, out Charge);

                if (canParse == false)
                    ErrorMessage = "invalid charge";
                else if (Charge == 0)
                    ErrorMessage = "ions must have charge";
                else
                    ErrorMessage = null;
            }
        }
    }
}
