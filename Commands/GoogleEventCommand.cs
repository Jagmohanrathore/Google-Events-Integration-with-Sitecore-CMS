using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web;
using Sitecore.Web.UI.Sheer;
using Sitecore;
using Sitecore.Diagnostics;
using Sitecore.Shell.Applications.ContentEditor;
using System.Text.RegularExpressions;
using Sitecore.Text;
using Sitecore.Data.Items;
using Sitecore.Shell.Applications.WebEdit.Commands;
using Sitecore.IO;

namespace Oasis.GoogleEventTracking.Commands
{
    public class GoogleEventCommand : Link
    {
        #region IContentField
        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public void SetValue(string value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <returns>The value of the field.</returns>
        public string GetValue()
        {
            return Value;
        }
        #endregion

        #region overrided
        /// <summary>
        /// Handles the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public override void HandleMessage(Sitecore.Web.UI.Sheer.Message message)
        {
            base.HandleMessage(message);
            if (message["id"] != ID || String.IsNullOrEmpty(message.Name))
            {
                return;
            }

            switch (message.Name)
            {
                case "contentlink:GoogleEvent":
                    var url = new UrlString(UIUtil.GetUri("control:GoogleTrackinglink"));
                    base.Insert(url.ToString());
                    return;
            }

            if (Value.Length > 0)
            {
                SetModified();
            }

            Value = String.Empty;
        }

        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="output">The <see cref="T:System.Web.UI.HtmlTextWriter"></see> object that receives the control content.</param>
        /// <remarks>When developing custom server controls, you can override this method to generate content for an ASP.NET page.</remarks>
        protected override void Render(System.Web.UI.HtmlTextWriter output)
        {
            base.Render(output);
            output.Write("<object id=\"dlgHelper\" classid=\"clsid:3050f819-98b5-11cf-bb82-00aa00bdce0b\" width=\"0px\" height=\"0px\"></object>");
        }

        #endregion
    }
}
