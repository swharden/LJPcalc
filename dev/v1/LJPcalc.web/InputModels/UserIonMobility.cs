using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LJPcalc.web.InputModels
{
    public class UserIonMobility
    {
        public string ErrorMessage;
        public bool IsValid => string.IsNullOrEmpty(ErrorMessage);

        private string input;
        public double Mobility;
        private string PrettyMobility => (Mobility / 1e11).ToString("#.#####");

        public string Input
        {
            get => IsValid ? PrettyMobility : input;
            set
            {
                input = value;
                bool canParse = double.TryParse(input, out double parsedMobility);

                if (canParse)
                    Mobility = parsedMobility * 1e11;

                if (canParse == false)
                    ErrorMessage = "invalid mobility";
                else if (Mobility <= 0)
                    ErrorMessage = "mobility must be >0";
                else
                    ErrorMessage = null;
            }
        }
    }
}
