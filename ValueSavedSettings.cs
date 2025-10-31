using Playnite.SDK;
using System.Collections.Generic;

namespace ValueSavedPlugin
{
    public class ValueSavedSettings : ISettings
    {
        public double TaxRate { get; set; } = 0.0;

        public void BeginEdit()
        {
            // No action needed
        }

        public void CancelEdit()
        {
            // No action needed
        }

        public void EndEdit()
        {
            // No action needed
        }

        public bool VerifySettings(out List<string> errors)
        {
            errors = new List<string>();
            if (TaxRate < 0 || TaxRate > 100)
            {
                errors.Add("Tax rate must be between 0 and 100.");
                return false;
            }
            return true;
        }
    }
}