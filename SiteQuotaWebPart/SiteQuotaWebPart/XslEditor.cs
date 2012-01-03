using System.Web.UI.WebControls.WebParts;
using System.Web.UI.WebControls;

namespace SiteQuotaWebPart
{
    /// <summary>
    /// Main class for th Editor Part
    /// </summary>
    public class XslEditor : EditorPart
    {
        #region Private Variables

        private TextBox _xslText;

        #endregion

        #region Public Methods

        /// <summary>
        /// Saves the values in an <see cref="T:System.Web.UI.WebControls.WebParts.EditorPart"/> control to the corresponding properties in the associated <see cref="T:System.Web.UI.WebControls.WebParts.WebPart"/> control.
        /// </summary>
        /// <returns>
        /// true if the action of saving values from the <see cref="T:System.Web.UI.WebControls.WebParts.EditorPart"/> control to the <see cref="T:System.Web.UI.WebControls.WebParts.WebPart"/> control is successful; otherwise (if an error occurs), false.
        /// </returns>
        public override bool ApplyChanges()
        {
            SiteQuotaWebPart quotaWebPart = WebPartToEdit as SiteQuotaWebPart;

            if (quotaWebPart != null)
            {
                EnsureChildControls();
                quotaWebPart.XslText = _xslText.Text;
            }

            return (quotaWebPart != null);
        }

        /// <summary>
        /// Retrieves the property values from a <see cref="T:System.Web.UI.WebControls.WebParts.WebPart"/> control for its associated <see cref="T:System.Web.UI.WebControls.WebParts.EditorPart"/> control.
        /// </summary>
        public override void SyncChanges()
        {
            SiteQuotaWebPart quotaWebPart = WebPartToEdit as SiteQuotaWebPart;
            if (quotaWebPart == null) return;
            EnsureChildControls();
            _xslText.Text = quotaWebPart.XslText;
        } 

        #endregion

        #region Events

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            _xslText = new TextBox {TextMode = TextBoxMode.MultiLine, Height = Unit.Pixel(200)};
            Controls.Add(_xslText);

            
            base.CreateChildControls();
        }

        #endregion



    } 
}
