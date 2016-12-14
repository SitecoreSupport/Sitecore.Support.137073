using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.RenderField;
using Sitecore.WordOCX;
using System.Reflection;
using Sitecore.Support.Data.Items;

namespace Sitecore.Support.Pipelines.RenderField
{
    public class GetFieldValue
    {
        public void Process(RenderFieldArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            // Sitecore.Support +++
            if (Sitecore.Context.PageMode.IsPreview || Sitecore.Context.PageMode.IsPageEditor)
            {
                PropertyInfo fieldValuePropertyInfo = args.GetType().GetProperty("FieldValue");
                if (fieldValuePropertyInfo != null)
                    fieldValuePropertyInfo.SetValue(args, args.Item.GetFieldValue(args.FieldName));
                else
                    Log.Error("The 'fieldValuePropertyInfo' instance cannot be null.", this);
            }
            args.Result.FirstPart = args.FieldValue;
            // Sitecore.Support ---
            if (args.FieldTypeKey == "rich text")
            {
                WordFieldValue value2 = WordFieldValue.Parse(args.Result.FirstPart);
                if (value2.BlobId != ID.Null)
                {
                    args.Result.FirstPart = value2.GetHtmlWithStyles();
                }
            }
        }
    }
}