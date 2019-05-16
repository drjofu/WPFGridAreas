using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfGridAreaDemo
{
  [TypeConverter(typeof(AreaRowConverter))]
  public class AreaRow
  {
    public string AreaNames { get; set; }
  }

  public class AreaRows : List<AreaRow> { }

  internal class AreaRowConverter:TypeConverter
  {
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
      return sourceType == typeof(string);
    }
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {

      return new AreaRow { AreaNames = (string)value };
    }
  }
}
