namespace MySiteLib
{
    /// <summary>
    /// Represents the webpart 
    /// </summary>
    public class WebPartItem
    {
        #region Private Variables

        private string _title;
        private string _id;
        private string _typeName;

        #endregion

        #region Properties

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
            }
        }

        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }


        }

        public string TypeName
        {
            get
            {
                return _typeName;
            }
            set
            {
                _typeName = value;
            }
        }

        #endregion 

        #region Public Methods

        public WebPartItem(string title, string id,string typename)
        {
            Title = title;
            Id = id;
            TypeName = typename;
        }

        public override string ToString()
        {
            return Title;
        }

        #endregion

    }
}
