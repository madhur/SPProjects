using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.SharePoint.Client;

namespace SPInfoPathList
{
    public static class Util
    {
        public static ClientContext GetContext()
        {
            if (ClientContext.Current != null)
                return ClientContext.Current;
            else
                return new ClientContext("http://sp.madhurmoss.com/sites/nishantverma/portfolios");

        }

    }
}
