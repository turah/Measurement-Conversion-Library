/*
 * This library was originally developed by Erich Ledesma.
 * The original version is available from:
 * http://www.codeproject.com/Articles/23087/Measurement-Unit-Conversion-Library
 * 
 * Licensed under the CPOL License.
 * 
 * This file was modified by Radix Engenharia e Software.
 * All modifications are pointed below.
 * 
 * Changes List
 * - Created MassDensityTable, MassFlowRateTable, SurfaceTensionTable, VelocityTable, ViscosityTable, VolumeFlowRateTable, VolumeFlowRatePerAreaTable, PressureTable, PressureDrop, MolecularWeightTable, MomentumTable and AreaTable properties.
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Reflection;
using System.Resources;
using System.IO;

namespace ScaredFingers.UnitsConversion
{
  /// <summary>
  /// Represents a UnitTable.
  /// 
  /// This interface is just if you might need multiple inheritance.
  /// <seealso cref="ScaredFingers.UnitsConversion.UnitTable"/>
  /// </summary>
  public interface IUnitTable
  {
    string GetUnitName(int unitCode);
    string GetUnitName(Unit unit);
    string GetUnitSymbol(int unitCode);
    string GetUnitSymbol(Unit unit);
    string GetUnitPlural(int unitCode);
    string GetUnitPlural(Unit unit);
    /// <summary>
    /// Returns a Converter object able to convert a source unit to destination one.
    /// </summary>
    /// <param name="srcCode">Code for source unit</param>
    /// <param name="destCode">Code for destination unit</param>
    /// <returns>A Converter object</returns>
    IConverter GetConversion(int srcCode, int destCode);
    IConverter GetConversion(Unit srcUnit, Unit destUnit);
    /// <summary>
    /// Performs a Converter.
    /// 
    /// When implemented should return the result of applying GetConversion then Convert. Also the result unit's table
    /// should be the same as in srcUnit.
    /// </summary>
    /// <param name="srcUnit">Unit to be converted</param>
    /// <param name="destCode">Code for destination Unit</param>
    /// <returns></returns>
    Unit Convert(Unit srcUnit, int destCode);
    bool IsKnownUnit(int unitCode);
  }

  public abstract class UnitTable : IUnitTable
  {
    #region IUnitTable Members

    public abstract string GetUnitName(int unitCode);
    public string GetUnitName(Unit unit)
    {
      return GetUnitName(unit.UnitCode);
    }

    public abstract string GetUnitSymbol(int unitCode);
    public string GetUnitSymbol(Unit unit)
    {
      return GetUnitSymbol(unit.UnitCode);
    }

    public abstract string GetUnitPlural(int unitCode);
    public string GetUnitPlural(Unit unit)
    {
      return GetUnitPlural(unit.UnitCode);
    }

    public abstract IConverter GetConversion(int srcCode, int destCode);
    public IConverter GetConversion(Unit srcUnit, Unit destUnit)
    {
      return GetConversion(srcUnit.UnitCode, destUnit.UnitCode);
    }

    public Unit Convert(Unit srcUnit, int destCode)
    {
      return new Unit(destCode, GetConversion(srcUnit.UnitCode, destCode).Convert(srcUnit.Value), srcUnit.UnitTable);
    }

    public abstract bool IsKnownUnit(int unitCode);
    #endregion

    #region Built-in Tables
    static XmlUnitTable tbl_Length;
    public static UnitTable LengthTable
    {
      get
      {
        if (tbl_Length == null)
          tbl_Length = new XmlUnitTable(Assembly.GetExecutingAssembly().
            GetManifestResourceStream("ScaredFingers.UnitsConversion.LengthUnits.xml"));
        return tbl_Length;
      }
    }
    static XmlUnitTable tbl_Weight;
    public static UnitTable WeightTable
    { 
      get
      {
        if (tbl_Weight == null)
          tbl_Weight = new XmlUnitTable(Assembly.GetExecutingAssembly().
            GetManifestResourceStream("ScaredFingers.UnitsConversion.WeightUnits.xml"));
        return tbl_Weight;
      }
    }
    static XmlUnitTable tbl_Temperature;
    public static UnitTable TemperatureTable
    {
      get
      {
        if (tbl_Temperature == null)
          tbl_Temperature = new XmlUnitTable(Assembly.GetExecutingAssembly().
            GetManifestResourceStream("ScaredFingers.UnitsConversion.TemperatureUnits.xml"));
        return tbl_Temperature;
      }
    }
    static XmlUnitTable tbl_Volume;
    public static UnitTable VolumeTable
    {
      get
      {
        if (tbl_Volume == null)
          tbl_Volume = new XmlUnitTable(Assembly.GetExecutingAssembly().
            GetManifestResourceStream("ScaredFingers.UnitsConversion.VolumeUnits.xml"));
        return tbl_Volume;
      }
    }
    static XmlUnitTable tbl_MassDensity;
    public static UnitTable MassDensityTable
    {
        get
        {
            if (tbl_MassDensity == null)
                tbl_MassDensity = new XmlUnitTable(Assembly.GetExecutingAssembly().GetManifestResourceStream("ScaredFingers.UnitsConversion.MassDensityUnits.xml"));
            return tbl_MassDensity;
        }
    }
    static XmlUnitTable tbl_MassFlowRate;
    public static UnitTable MassFlowRateTable
    {
        get
        {
            if (tbl_MassFlowRate == null)
                tbl_MassFlowRate = new XmlUnitTable(Assembly.GetExecutingAssembly().
                  GetManifestResourceStream("ScaredFingers.UnitsConversion.MassFlowRateUnits.xml"));
            return tbl_MassFlowRate;
        }
    }

    static XmlUnitTable tbl_SurfaceTension;
    public static UnitTable SurfaceTensionTable
    {
        get
        {
            if (tbl_SurfaceTension == null)
                tbl_SurfaceTension = new XmlUnitTable(Assembly.GetExecutingAssembly().
                  GetManifestResourceStream("ScaredFingers.UnitsConversion.SurfaceTensionUnits.xml"));
            return tbl_SurfaceTension;
        }
    }

    static XmlUnitTable tbl_Velocity;
    public static UnitTable VelocityTable
    {
        get
        {
            if (tbl_Velocity == null)
                tbl_Velocity = new XmlUnitTable(Assembly.GetExecutingAssembly().
                  GetManifestResourceStream("ScaredFingers.UnitsConversion.VelocityUnits.xml"));
            return tbl_Velocity;
        }
    }

    static XmlUnitTable tbl_Viscosity;
    public static UnitTable ViscosityTable
    {
        get
        {
            if (tbl_Viscosity == null)
                tbl_Viscosity = new XmlUnitTable(Assembly.GetExecutingAssembly().
                  GetManifestResourceStream("ScaredFingers.UnitsConversion.ViscosityUnits.xml"));
            return tbl_Viscosity;
        }
    }

    static XmlUnitTable tbl_VolumeFlowRate;
    public static UnitTable VolumeFlowRateTable
    {
        get
        {
            if (tbl_VolumeFlowRate == null)
                tbl_VolumeFlowRate = new XmlUnitTable(Assembly.GetExecutingAssembly().
                  GetManifestResourceStream("ScaredFingers.UnitsConversion.VolumeFlowRateUnits.xml"));
            return tbl_VolumeFlowRate;
        }
    }

    static XmlUnitTable tbl_VolumeFlowRatePerArea;
    public static UnitTable VolumeFlowRatePerAreaTable
    {
        get
        {
            if (tbl_VolumeFlowRatePerArea == null)
                tbl_VolumeFlowRatePerArea = new XmlUnitTable(Assembly.GetExecutingAssembly().
                    GetManifestResourceStream("ScaredFingers.UnitsConversion.VolumeFlowRatePerAreaUnits.xml"));
            return tbl_VolumeFlowRatePerArea;
        }
    }

    static XmlUnitTable tbl_Pressure;
    public static UnitTable PressureTable
    {
        get
        {
            if (tbl_Pressure == null)
                tbl_Pressure = new XmlUnitTable(Assembly.GetExecutingAssembly().
                  GetManifestResourceStream("ScaredFingers.UnitsConversion.PressureUnits.xml"));
            return tbl_Pressure;
        }
    }

    static XmlUnitTable tbl_PressureDrop;
    public static UnitTable PressureDrop
    {
        get
        {
            if (tbl_PressureDrop == null)
                tbl_PressureDrop = new XmlUnitTable(Assembly.GetExecutingAssembly().
                    GetManifestResourceStream("ScaredFingers.UnitsConversion.PressureDropUnits.xml"));
            return tbl_PressureDrop;
        }
    }

    static XmlUnitTable tbl_MolecularWeight;
    public static UnitTable MolecularWeightTable
    {
        get
        {
            if (tbl_MolecularWeight == null)
                tbl_MolecularWeight = new XmlUnitTable(Assembly.GetExecutingAssembly().
                  GetManifestResourceStream("ScaredFingers.UnitsConversion.MolecularWeightUnits.xml"));
            return tbl_MolecularWeight;
        }
    }

    static XmlUnitTable tbl_Momentum;
    public static UnitTable MomentumTable
    {
        get
        {
            if (tbl_Momentum == null)
                tbl_Momentum = new XmlUnitTable(Assembly.GetExecutingAssembly().
                  GetManifestResourceStream("ScaredFingers.UnitsConversion.MomentumUnits.xml"));
            return tbl_Momentum;
        }
    }
    
      static XmlUnitTable tbl_Area;
    public static UnitTable AreaTable
    {
        get
        {
            if (tbl_Area == null)
                tbl_Area = new XmlUnitTable(Assembly.GetExecutingAssembly().GetManifestResourceStream("ScaredFingers.UnitsConversion.AreaUnits.xml"));
            return tbl_Area;
        }
    }
   
    #endregion
  }

  /// <summary>
  /// Provides basic implentation for a Table.
  /// 
  /// Uses 2 Dictionaries to register units and their Converter.
  /// </summary>
  public abstract class BasicUnitTable : UnitTable
  {
    #region Heirs Tool Memebers
    /// <summary>
    /// Holds specifications for each unit.
    /// </summary>
    protected struct UnitSpec
    {
      public int Code;
      public string Name;
      public string Symbol;
      public string Plural;
    }
    protected Dictionary<int, UnitSpec> dic_Units = new Dictionary<int,UnitSpec>();
    protected Dictionary<string, IConverter> dic_ConversionTable = new Dictionary<string,IConverter>();
    protected void AddUnit(int unitCode, string Name, string Symbol, string Plural)
    {
      if (dic_Units.ContainsKey(unitCode))
        throw new DuplicatedUnitException();

      UnitSpec spec;
      spec.Code = unitCode;
      spec.Name = Name;
      spec.Symbol = Symbol;
      spec.Plural = Plural;

      dic_Units[unitCode] = spec;
    }
    protected void AddConversion(int srcCode, int destCode, Converter Converter)
    {
      dic_ConversionTable[FormatKey(srcCode, destCode)] = Converter;
      if (!dic_ConversionTable.ContainsKey(FormatKey(destCode, srcCode)) && Converter.AllowInverse)
        dic_ConversionTable[FormatKey(destCode, srcCode)] = Converter.Inverse;
    }
    protected static string FormatKey(int srcCode, int destCode)
    {
      return srcCode + ":" + destCode;
    }
    #endregion

    #region UnitTable Overrides
    public override IConverter GetConversion(int srcCode, int destCode)
    {
      if (!dic_ConversionTable.ContainsKey(FormatKey(srcCode, destCode)))
        throw new NoAvailableConversionException();
      return dic_ConversionTable[FormatKey(srcCode, destCode)];
    }
    public override string GetUnitName(int unitCode)
    {
      if (! dic_Units.ContainsKey(unitCode))
        throw new UnknownUnitException() ;
      return dic_Units[unitCode].Name;
    }
    public override string GetUnitSymbol(int unitCode)
    {
      if (!dic_Units.ContainsKey(unitCode))
        throw new UnknownUnitException();
      return dic_Units[unitCode].Symbol;
    }
    public override string GetUnitPlural(int unitCode)
    {
      if (!dic_Units.ContainsKey(unitCode))
        throw new UnknownUnitException();
      return dic_Units[unitCode].Plural;
    }
    public override bool IsKnownUnit(int unitCode)
    {
      return dic_Units.ContainsKey(unitCode);
    }
    #endregion
  }

  /// <summary>
  /// Xml Initializable Unit Table
  /// 
  /// Reads information about units and their Converter from a Xml document.
  /// </summary>
  public class XmlUnitTable : BasicUnitTable
  {
    #region Constants
    public const string ELEMENT_UNITTABLE = "UnitTable";
    public const string ELEMENT_UNITS = "Units";
    public const string ELEMENT_UNIT = "Unit";
    public const string ELEMENT_ConversionS = "Conversions";
    public const string ELEMENT_Conversion = "Converter";
    public const string ELEMENT_LINEAR = "Linear";
    public const string ELEMENT_DECIBEL = "Decibel";
    public const string ELEMENT_CUSTOM = "Custom";

    public const string ATTRIBUTE_NAME = "name";
    public const string ATTRIBUTE_SYMBOL = "symbol";
    public const string ATTRIBUTE_PLURAL = "plural";
    public const string ATTRIBUTE_CODE = "code";
    public const string ATTRIBUTE_FACTOR = "factor";
    public const string ATTRIBUTE_DELTHA = "deltha";
    public const string ATTRIBUTE_TYPENAME = "typeName";
    public const string ATTRIBUTE_SRCCODE = "srcCode";
    public const string ATTRIBUTE_DESTCODE = "destCode";
    public const string ATTRIBUTE_REFERENCE = "reference";
    #endregion

    #region Creation & Initialization
    public XmlUnitTable(Stream stream)
    {
      XmlDocument units = new XmlDocument();
      units.Load(stream);

      Initialize(units[ELEMENT_UNITTABLE]);
    }
    public XmlUnitTable(string fileName)
    {
      XmlDocument units = new XmlDocument();
      units.Load(fileName);

      Initialize(units[ELEMENT_UNITTABLE]);
    }
    public XmlUnitTable(XmlDocument units)
    {
      Initialize(units[ELEMENT_UNITTABLE]);
    }

    private void Initialize(XmlElement unitTableElement)
    {
      foreach (XmlElement unitElement in unitTableElement[ELEMENT_UNITS].ChildNodes)
      {
        int code ;
        string name, symbol, plural;

        CreateUnit(unitElement, out code, out name, out symbol, out plural);
        AddUnit(code, name, symbol, plural);
      }

      foreach (XmlElement ConversionElement in unitTableElement[ELEMENT_ConversionS].ChildNodes)
      {
        int srcCode, destCode;

        Converter Converter = CreateConversion(ConversionElement, out srcCode, out destCode);
        AddConversion(srcCode, destCode, Converter);
      }
    }

    protected static void CreateUnit(XmlElement unitElement, out int unitCode, out string unitName, out string unitSymbol, out string unitPlural)
    {
      try
      {
        unitPlural = (unitElement.Attributes[ATTRIBUTE_PLURAL] != null)? 
          unitElement.Attributes[ATTRIBUTE_PLURAL].Value : unitElement.Attributes[ATTRIBUTE_NAME].Value;

        unitCode = int.Parse(unitElement.Attributes[ATTRIBUTE_CODE].Value);
        unitName = unitElement.Attributes[ATTRIBUTE_NAME].Value;
        unitSymbol = unitElement.Attributes[ATTRIBUTE_SYMBOL].Value;
      }
      catch (Exception ex)
      {
        throw new UnitCreationException(global::ScaredFingers.UnitsConversion.Resources.Errors.UnitTagError, ex);
      }
    }

    protected static Converter CreateConversion(XmlElement ConversionElement, out int srcCode, out int destCode)
    {
      Converter result = null ;
      try
      {
        srcCode = int.Parse(ConversionElement.Attributes[ATTRIBUTE_SRCCODE].Value);
        destCode = int.Parse(ConversionElement.Attributes[ATTRIBUTE_DESTCODE].Value);
      }
      catch (Exception ex)
      {
        throw new ConversionCreationException(global::ScaredFingers.UnitsConversion.Resources.Errors.ConversionTagError, ex);
      }

      if (ConversionElement.Name == ELEMENT_LINEAR)
      {
        try
        {
          double factor = ConversionElement.Attributes[ATTRIBUTE_FACTOR] == null ? 
            1d : double.Parse(ConversionElement.Attributes[ATTRIBUTE_FACTOR].Value);
          double deltha = ConversionElement.Attributes[ATTRIBUTE_DELTHA] == null ? 
            0d : double.Parse(ConversionElement.Attributes[ATTRIBUTE_DELTHA].Value);

          result = new LinearConverter(factor, deltha);
        }
        catch (Exception ex)
        {
          throw new ConversionCreationException(global::ScaredFingers.UnitsConversion.Resources.Errors.ConversionTagError, ex);
        }
      }

      if (ConversionElement.Name == ELEMENT_DECIBEL)
      {
        try
        {
          double reference = double.Parse(ConversionElement.Attributes[ATTRIBUTE_REFERENCE].Value) ;

          result = new DecibelConverter(reference);
        }
        catch (Exception ex)
        { 
          throw new ConversionCreationException(global::ScaredFingers.UnitsConversion.Resources.Errors.ConversionTagError, ex);
        }
      }

      if (ConversionElement.Name == ELEMENT_CUSTOM)
      {
        try
        {
          string typeName = ConversionElement.Attributes[ATTRIBUTE_TYPENAME].Value;

          result = (Converter)Utilities.CreateInstance(typeName);
          if (result is CustomConverter)
          {
            ((CustomConverter)result).XmlInitialize(ConversionElement);
          }
        }
        catch (Exception ex)
        {
          throw new ConversionCreationException(global::ScaredFingers.UnitsConversion.Resources.Errors.ConversionTagError, ex);
        }
      }

      if (result == null)
        throw new ConversionCreationException();

      return result;
    }
    #endregion
  }

  /// <summary>
  /// Provides localized unit's name and plural.
  /// 
  /// To be used in localizable applications. Uses 'name' and 'plural' defined in the Xml document as Keys for a resource manager. 
  /// If no resource manager is specified a default one will be used.
  /// </summary>
  public class LocalizedXmlUnitTable : XmlUnitTable
  {
    #region Fields & Properties
    ResourceManager man_Resources;
    #endregion

    #region Creation & Initialization
    public LocalizedXmlUnitTable(string fileName, ResourceManager manager)
      : base(fileName)
    {
      if (manager == null)
        throw new ArgumentNullException();

      man_Resources = manager;
    }

    public LocalizedXmlUnitTable(XmlDocument units, ResourceManager manager) 
      : base(units)
    {
      if (manager == null)
        throw new ArgumentNullException();

      man_Resources = manager;
    }

    public LocalizedXmlUnitTable(string fileName)
      : this(fileName, global::ScaredFingers.UnitsConversion.Resources.Units.ResourceManager)
    { 
    
    }

    public LocalizedXmlUnitTable(XmlDocument units)
      : this(units, global::ScaredFingers.UnitsConversion.Resources.Units.ResourceManager)
    { 
    
    }
    #endregion

    #region BasicUnitTable Overrides
    public override string GetUnitName(int unitCode)
    {
      return man_Resources.GetString(base.GetUnitName(unitCode));
    }
    public override string GetUnitPlural(int unitCode)
    {
      return man_Resources.GetString(base.GetUnitPlural(unitCode));
    }
    #endregion
  }
}
