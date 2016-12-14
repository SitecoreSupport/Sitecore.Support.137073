using Sitecore.Diagnostics;
using System;
using System.Reflection;

namespace Sitecore.Support.Data.Items
{
    public static class ItemExtensions
    {
        public static string GetFieldValue(this Sitecore.Data.Items.Item item, string fieldName)
        {
            Sitecore.Data.Fields.Field field = item.Fields[fieldName];

            if (item.Versions.Count > 0 || field.Shared || !item.IsClone)
                return field.ToString();
            else
                return ItemExtensions.GetFieldValue(field);
        }
        private static string GetFieldValue(Sitecore.Data.Fields.Field field)
        {
            string fieldValue = field.GetValue(true, true);
            if (!field.InheritsValueFromOtherItem)
                return fieldValue ?? String.Empty;

            fieldValue = field.GetStandardValue();
            if (fieldValue != null)
            {
                PropertyInfo propertyInfo = field.GetType().GetProperty("containsStandardValue");
                if (propertyInfo != null)
                    propertyInfo.SetValue(field, 1);
                else
                    Log.Error("The 'containsStandardValue' cannot be null.", field);
                return fieldValue;
            }
            return field.GetValue(false, true) ?? String.Empty;
        }
    }
}