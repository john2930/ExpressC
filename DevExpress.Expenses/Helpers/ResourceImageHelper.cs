using System;
using System.Windows.Media.Imaging;

namespace Expenses.Wpf {
    public static class ResourceImageHelper {
        public static BitmapImage GetResourceImage(string image) {
            BitmapImage res = new BitmapImage();
            res.BeginInit();
            res.UriSource = new Uri(@"/DevExpress.Expenses;component/Views/Images/" + image, UriKind.Relative);
            res.EndInit();
            //res.Freeze();
            return res;
        }
    }
}
