using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;

namespace FormattedTextTestCommon
{
    public static class NET35CompatibilityPreferences
    {
        private static readonly FieldInfo coreCompatibilityPreferencesIsSealed;
        private static readonly PropertyInfo coreCompatibilityPreferencesIncludeAllInkInBoundingBox;

        static NET35CompatibilityPreferences()
        {
            try
            {
                Type coreCompatibilityPreferencesType = typeof(CoreCompatibilityPreferences);
                coreCompatibilityPreferencesIsSealed =
                    coreCompatibilityPreferencesType.GetField("_isSealed",
                        BindingFlags.NonPublic | BindingFlags.Static);
                coreCompatibilityPreferencesIncludeAllInkInBoundingBox = coreCompatibilityPreferencesType.GetProperty(
                    "IncludeAllInkInBoundingBox",
                    BindingFlags.NonPublic | BindingFlags.Static);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to access CoreCompatibilityPreferences. " + ex);
            }
        }

        public static bool IncludeAllInkInBoundingBox
        {
            set
            {
                try
                {
                    if ((bool) coreCompatibilityPreferencesIncludeAllInkInBoundingBox.GetValue(null) != value)
                    {
                        coreCompatibilityPreferencesIsSealed.SetValue(null, false);
                        coreCompatibilityPreferencesIncludeAllInkInBoundingBox.SetValue(null, value);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Failed to set CoreCompatibilityPreferences.IncludeAllInkInBoundingBox. " + ex);
                }
            }
            get
            {
                try
                {
                    return (bool) coreCompatibilityPreferencesIncludeAllInkInBoundingBox.GetValue(null);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Failed to read CoreCompatibilityPreferences.IncludeAllInkInBoundingBox. " + ex);
                }

                return true;
            }
        }
    }
}
