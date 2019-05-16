using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfGridExtensions
{
  // Definition einer Row
  [TypeConverter(typeof(AreaRowConverter))]
  public class AreaRow
  {
    public string AreaNames { get; set; }
  }

  // Liste von Rows
  public class AreaRows : List<AreaRow> { }

  // Type converter zur Umwandlung von String -> AreaRow
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
