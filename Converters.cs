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
 * - Changed the type of value to be converted from float to double.
 * - Changed methods 'Convert' that use the value to be converted.
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ScaredFingers.UnitsConversion
{
  /// <summary>
  /// An interface to base the Converter objects.
  /// </summary>
  public interface IConverter
  {
    /// <summary>
    /// Converts a value
    /// </summary>
    /// <param name="source">Value to be converted</param>
    /// <returns>Converted value</returns>
    /*float*/double Convert(/*float*/double source);
    /// <summary>
    /// Returns whether this object has an inverse formula
    /// </summary>
    bool AllowInverse { get; }
    /// <summary>
    /// Returns a inverse formula Converter object. Usefull if don't want to write much.
    /// </summary>
    IConverter Inverse { get; }
  }
  /// <summary>
  /// Provides a base class for Unit Converter formulas
  /// </summary>
  public abstract class Converter : IConverter
  {
    public abstract /*float*/double Convert(/*float*/double source);
    public abstract bool AllowInverse { get; }
    public abstract IConverter Inverse { get; }
  }

  /// <summary>
  /// Basic implementation for linear unit Converter.
  /// 
  /// DestUnit = SrcUnit * Factor + Deltha.
  /// 
  /// Instances should always have an inverse, except for Factor==0, but what's the point. Factor cannot be 0, 
  /// ArgumentException will protect you from harm yourself
  /// </summary>
  public class LinearConverter : Converter
  {
    #region Fields & Properties
    private double dbl_Factor = 1d;
    public double Factor
    {
      get { return dbl_Factor; }
      set { dbl_Factor = value; }
    }

    private double dbl_Deltha = 0d;
    public double Deltha
    {
      get { return dbl_Deltha; }
      set { dbl_Deltha = value; }
    }
    #endregion

    #region Creation & Initialization
    public LinearConverter()
    {

    }

    public LinearConverter(double factor, double deltha)
    {
      if (factor == 0)
        throw new ArgumentException(global::ScaredFingers.UnitsConversion.Resources.Errors.FactorZeroError);
      this.dbl_Factor = factor;
      this.dbl_Deltha = deltha;
    }

    public LinearConverter(double factor)
    {
      if (factor == 0)
        throw new ArgumentException(global::ScaredFingers.UnitsConversion.Resources.Errors.FactorZeroError);
      this.dbl_Factor = factor;
    }
    #endregion

    #region Converter Overrides
    public override /*float*/double Convert(/*float*/double source)
    {
      return /*(float)*/(source * dbl_Factor + dbl_Deltha);
    }
    public override bool AllowInverse { get { return true; } }
    public override IConverter Inverse { get { return new LinearConverter(1 / dbl_Factor, -dbl_Deltha / dbl_Factor); } }
    #endregion
  }

  public class DecibelConverter : Converter
  {
    #region Fields
    private double dbl_Reference;
    public double Reference
    {
      get { return dbl_Reference; }
      set {
        if (value == 0)
          throw new ArgumentException();
        dbl_Reference = value; 
      }
    }
	
    #endregion
    
    #region Creation
    public DecibelConverter(double reference)
    {
      this.dbl_Reference = reference;
    }
    public DecibelConverter()
    {
      this.dbl_Reference = 1;
    }
    #endregion

    #region Converter Overrides
    public override /*float*/double Convert(/*float*/double source)
    {
      return /*(float)*/(10 * Math.Log10(source / dbl_Reference));
    }

    public override bool AllowInverse
    {
      get { return true ; }
    }

    public override IConverter Inverse
    {
      get { return new InverseDecibelConverter(dbl_Reference); }
    }
    #endregion
  }

  internal class InverseDecibelConverter : Converter
  {
    #region Fields
    private double dbl_Reference;
    public double Reference
    {
      get { return dbl_Reference; }
      set
      {
        if (value == 0)
          throw new ArgumentException();
        dbl_Reference = value;
      }
    }
    #endregion

    #region Creation
    public InverseDecibelConverter(double reference)
    {
      this.dbl_Reference = reference;
    }
    public InverseDecibelConverter()
    {
      this.dbl_Reference = 1;
    }
    #endregion

    #region Converter Overrides
    public override /*float*/double Convert(/*float*/double source)
    {
      return /*(float)*/(Math.Pow(10d, source / 10d) * dbl_Reference) ;
    }

    public override bool AllowInverse
    {
      get { return true; }
    }

    public override IConverter Inverse
    {
      get { return new DecibelConverter(dbl_Reference); }
    }
    #endregion
  }


  /// <summary>
  /// A Base class for other Conversions
  /// 
  /// Usefull to define other kind of formulas, not linear ones. For instance for logaritmic units like dB, 
  /// online calculation like Currencies, so on. Integrated Currency calculation will be included soon.
  /// </summary>
  public abstract class CustomConverter : Converter
  {
    public abstract void XmlInitialize(XmlElement element);
  }
}
