using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace TDSDispatcher.Converters
{
    class DevideConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double devider = GetDevider(parameter);

            if(devider <= 0)
            {
                return value;
            }

            return System.Convert.ToDouble(value) / devider;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double devider = GetDevider(parameter);

            if (devider <= 0)
            {
                return value;
            }

            return System.Convert.ToDouble(value) * devider;
        }

        private double GetDevider(object parameter)
        {
            double devider = 0;
            switch (parameter)
            {
                case string param:
                    double.TryParse(param, out devider);
                    break;
                case double param:
                    devider = param;
                    break;
            }
            return devider;
        }
    }
}
