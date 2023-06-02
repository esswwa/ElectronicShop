namespace ElectronicShop.Assets.Converters
{
    public class ValueToTextDecorationsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value as int?;
            if (str == null) { return default; }
            else if (str == 0) { return default; }
            else { return TextDecorations.Strikethrough; }
            //var str = value as double?;
            //string str1 = str.ToString();
            //int len = str1.Length; 
            //if (len <=3) { return default; }
            //else if (str == 0) { return default; }
            //else { return TextDecorations.Strikethrough; }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
