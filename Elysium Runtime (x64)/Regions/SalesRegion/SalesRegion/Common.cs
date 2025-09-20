using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using RMSDataAccessLayer;

namespace SalesRegion
{

    public class ArithmeticConverter : IValueConverter
    {
        private const string ArithmeticParseExpression = "([+\\-*/]{1,1})\\s{0,}(\\-?[\\d\\.]+)";
        private Regex arithmeticRegex = new Regex(ArithmeticParseExpression);

        #region IValueConverter Members

        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            if (value is double && parameter != null)
            {
                string param = parameter.ToString();

                if (param.Length > 0)
                {
                    Match match = arithmeticRegex.Match(param);
                    if (match != null && match.Groups.Count == 3)
                    {
                        string operation = match.Groups[1].Value.Trim();
                        string numericValue = match.Groups[2].Value;

                        double number = 0;
                        if (double.TryParse(numericValue, out number)) // this should always succeed or our regex is broken
                        {
                            double valueAsDouble = (double)value;
                            double returnValue = 0;

                            switch (operation)
                            {
                                case "+":
                                    returnValue = valueAsDouble + number;
                                    break;

                                case "-":
                                    returnValue = valueAsDouble - number;
                                    break;

                                case "*":
                                    returnValue = valueAsDouble * number;
                                    break;

                                case "/":
                                    returnValue = valueAsDouble / number;
                                    break;
                            }

                            return returnValue;
                        }
                    }
                }
            }

            return null;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }

    public class TimeSpanValueConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.GetType() == typeof(TimeSpan))
            {
                TimeSpan t = (TimeSpan)value;
                return String.Format("{0} Hours:{1} Min:{2} Sec", t.Hours * -1, t.Minutes * -1, t.Seconds * -1);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class NotNullValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null || value.ToString() == "")
            {
               return "Null";
            }
            else
            {
                 return "Not Null";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DosageSourceConverter : IValueConverter
    {
        RMSDataAccessLayer.RMSModel db = new RMSDataAccessLayer.RMSModel();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != null)
            {
                var dosagelst = (from pe in db.TransactionEntryBase.OfType<PrescriptionEntry>()
                                where pe.Item == value
                                select pe.Dosage);
                return dosagelst;
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public enum SalesPadTransState
    {
        Transaction,
        Tender,
        Change,
        Receipt,
    }

    public enum ApplicationMode
    {
        Ticket,
        Pharmacy,
        POS
    }
}
