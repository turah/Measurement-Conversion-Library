using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace ScaredFingers.UnitsConversion
{
  /// <summary>
  /// Internal Unit Representation
  /// 
  /// Uses a 64bit struct to keep both value and code. Most significant 32bits holds a Int32 meaning unit code.
  /// Less significant 32bits holds a Single meaning unit value. This structure uses unsafe code to convert itself to
  /// and from an Int64.
  /// </summary>
  [StructLayout(LayoutKind.Sequential)]
  internal struct UnitValue
  {
    public int Code;
    public /*float*/double Value;

    #region Object Overrides
    public override string ToString()
    {
      return "Unit Code: " + Code.ToString("X8") + "; Unit Value: " + Value;
    }
    public override bool Equals(object obj)
    {
      if (!(obj is UnitValue))
        return false;

      UnitValue unit = (UnitValue)obj;

      return unit.Code == Code && unit.Value == Value;
    }
    public override int GetHashCode()
    {
      return Code.GetHashCode() + Value.GetHashCode();
    }
    #endregion

    #region Creation
    public UnitValue(int code, float value)
    {
      this.Code = code;
      this.Value = value;
    }
    #endregion

    #region Converter
    public static implicit operator long(UnitValue x)
    {
      long result = 0 ;

      unsafe
      {
        byte* pX = (byte*)(void*)&x;
        byte* pResult = (byte*)(void*)&result;

        CopyBytes(pX, pResult, sizeof(UnitValue));
      }

      return result;
    }

    public static explicit operator UnitValue(long x)
    {
      UnitValue result;

      unsafe
      { 
        byte* pX = (byte*)(void*)&x;
        byte* pResult = (byte*)(void*)&result;

        CopyBytes(pX, pResult, sizeof(UnitValue));
      }

      return result;
    }

    private unsafe static void CopyBytes(byte* src, byte* dest, int count)
    {
      for (int iByte = 0; iByte < count; iByte++)
        dest[iByte] = src[iByte];
    }
    #endregion
  }

  /// <summary>
  /// Public Unit Representation
  /// 
  /// Uses a UnitTable to get unit's name and symbol and a UnitValue to keep actual unit's information. Suports float formating
  /// when converted to string using ToString. It can be stored or fetch from a DataBase as an Int64 using the property Int64Value.
  /// 
  /// All units are bound to a table. A Table holds a set of units and Conversions among them. For instance you can create a table
  /// holding Length units such as: Meters, Kilometers, Yards, Feet and Miles and their Conversions.
  /// <seealso cref="ScaredFingers.UnitsConversion.UnitTable"/>
  /// <seealso cref="ScaredFingers.UnitsConversion.XmlUnitTable"/>
  /// </summary>
  public class Unit
  {
    #region Fields & Properties
    private UnitTable tbl_UnitTable;
    /// <summary>
    /// Gets the Unit's Table
    /// </summary>
    public UnitTable UnitTable
    {
      get { return tbl_UnitTable; }
    }

    /// <summary>
    /// Gets the Unit's Name.
    /// </summary>
    public string UnitName
    {
      get { return tbl_UnitTable.GetUnitName(unt_Value.Code); }
    }

    /// <summary>
    /// Gets the Unit's Internation Symbol
    /// </summary>
    public string UnitSymbol
    {
      get { return tbl_UnitTable.GetUnitSymbol(unt_Value.Code); }
    }

    /// <summary>
    /// Unit's Name Plural Form
    /// </summary>
    public string UnitPlural
    {
      get { return tbl_UnitTable.GetUnitPlural(unt_Value.Code); }
    }

    private UnitValue unt_Value;

    /// <summary>
    /// Sets or gets unit's value
    /// </summary>
    public /*float*/double Value
    {
      get { return unt_Value.Value; }
      set { unt_Value.Value = value; }
    }

    /// <summary>
    /// Gets or sets unit's value and code.
    /// 
    /// This method was build to store and retreive units form DataBases as Int64. Set should be used carefully, because 
    /// if DataBase retreived value uses a unit code unknown to the table an UnknownUnitException will be thrown.
    /// </summary>
    public long Int64Value
    {
      get { return unt_Value; }
      set 
      { 
        UnitValue assign = (UnitValue)value;
        if (!tbl_UnitTable.IsKnownUnit(assign.Code))
          throw new UnknownUnitException();

        unt_Value = assign;
      }
    }

    /// <summary>
    /// Gets or sets unit's value.
    /// 
    /// Set should be used carefully, because if the value is a unit code unknow to the table an UnknownUnitException 
    /// will be thrown.
    /// </summary>
    public int UnitCode
    {
      get { return unt_Value.Code; }
      set
      {
        if (!tbl_UnitTable.IsKnownUnit(value))
          throw new UnknownUnitException();
        unt_Value.Code = value;
      }
    }

    /// <summary>
    /// Converts this instance to another unit
    /// </summary>
    /// <param name="destCode">destination unit code</param>
    /// <returns>Converted unit</returns>
    public Unit Convert(int destCode)
    {
      return tbl_UnitTable.Convert(this, destCode);
    }
    #endregion

    #region Creation
    /// <summary>
    /// Creates a new Unit
    /// </summary>
    /// <param name="unitCode">Unit's code</param>
    /// <param name="unitValue">Unit's value</param>
    /// <param name="table">Unit's table</param>
    public Unit(int unitCode, /*float*/double unitValue, UnitTable table)
    {
      unt_Value.Code = unitCode;
      unt_Value.Value = unitValue;
      tbl_UnitTable = table;
    }
    #endregion

    #region Object Overrides
    public override string ToString()
    {
      return Value + UnitSymbol;
    }
    public virtual string ToString(string format)
    {
      return Value.ToString(format) + UnitSymbol;
    }
    public virtual string ToString(IFormatProvider provider)
    { 
      return Value.ToString(provider) + UnitSymbol ;
    }
    public virtual string ToString(string format, IFormatProvider provider)
    {
      return Value.ToString(format, provider);
    }

    public override bool Equals(object obj)
    {
      if (!(obj is Unit))
        return false;

      return unt_Value.Equals(obj);
    }
    public override int GetHashCode()
    {
      return unt_Value.GetHashCode();
    }
    #endregion

  }
}
