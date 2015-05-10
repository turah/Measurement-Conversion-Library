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
using System.Data;
using System.IO;
using System.Net;
using System.Xml;
using System.Text.RegularExpressions;
using System.Xml.Xsl;
using System.Reflection;

namespace ScaredFingers.UnitsConversion
{
  /// <summary>
  /// Converts one currency to another
  /// </summary>
  public class CurrencyConverter : Converter
  {
    #region Fields & Properties
    private double dbl_Rate;
    public double Rate
    {
      get { return dbl_Rate; }
      set { dbl_Rate = value; }
    }
    #endregion

    #region Creation
    private CurrencyConverter(double rate)
    {
      this.dbl_Rate = rate;
    }
    public CurrencyConverter(string srcCurr, string destCurr, ICurrencyExchangeRatesProvider provider)
    {
      dbl_Rate = provider.GetExchangeRate(srcCurr, destCurr);
    }
    #endregion

    #region Converter Overrides
    public override /*float*/double Convert(/*float*/double source)
    {
      return /*(float)*/(source * dbl_Rate);
    }

    public override bool AllowInverse
    {
      get { return true; }
    }

    public override IConverter Inverse
    {
      get { return new CurrencyConverter(1 / dbl_Rate); }
    }
    #endregion
  }

  public class CurrencyExchangeTable : BasicUnitTable
  {
    #region Fields & Properties
    Dictionary<string, int> dic_CurrencyCodes = new Dictionary<string, int>();
    ICurrencyExchangeRatesProvider prv_Provider;
    #endregion

    #region Creation
    public CurrencyExchangeTable(ICurrencyExchangeRatesProvider provider)
    {
      prv_Provider = provider;
      int unitCode = 1;
      foreach (string currency in provider.Currencies)
      {
        AddUnit(unitCode, provider.GetCurrencyName(currency), currency, provider.GetCurrencyPlural(currency));
        dic_CurrencyCodes[currency] = unitCode;
        unitCode++;
      }
    }
    #endregion

    #region Public Members
    public int CurrencyCode(string currencySymbol)
    {
      return dic_CurrencyCodes[currencySymbol];
    }
    public Unit CreateUnit(float value, string currencySymbol)
    {
      return new Unit(dic_CurrencyCodes[currencySymbol], value, this);
    }
    #endregion

    #region BasicUnitTable Overrides
    public override IConverter GetConversion(int srcCode, int destCode)
    {
      return new CurrencyConverter(GetUnitSymbol(srcCode), GetUnitSymbol(destCode), prv_Provider);
    }
    #endregion

    #region Cache
    /*public void SaveCache(string fileName)
    {
      prv_Provider.Save(fileName);
    }
    public void SaveCache(Stream output)
    {
      prv_Provider.Save(output);
    }*/
    #endregion
  }
  public interface ICurrencyExchangeRatesProvider
  {
    double GetExchangeRate(string srcCurr, string destCurr);
    bool CanConvert(string srcCurr, string destCurr);
    string GetCurrencyName(string symbol);
    string GetCurrencyPlural(string symbol);
    IEnumerable<string> Currencies { get; }
    /*void Save(string fileName);
    void Save(Stream output);*/
  }

  /// <summary>
  /// Restores a DataSet Exchange Rate Table from an XML file.
  /// </summary>
  public abstract class CurrencyExchangeRatesProvider : ICurrencyExchangeRatesProvider
  {
    #region ICurrencyExchangeRatesProvider Members
    public abstract double GetExchangeRate(string srcCurrSymbol, string destCurrSymbol);

    public abstract string GetCurrencyName(string symbol);
    public abstract string GetCurrencyPlural(string symbol);

    public abstract bool CanConvert(string srcCurr, string destCurr);
    public abstract IEnumerable<string> Currencies { get;}
    /*public abstract void Save(string fileName);
    public abstract void Save(Stream output);*/
    #endregion
  }

  public abstract class CachedExchageRatesProvider : CurrencyExchangeRatesProvider
  {
    #region Constants
    public const string COLUMN_EXCHANGE = "Exchange";
    public const string COLUMN_SYMBOL = "Symbol";
    public const string COLUMN_NAME = "Currency";
    #endregion

    #region Fields & Properties
    protected DataSet dst_EURRates;
    #endregion

    #region Creation
    protected CachedExchageRatesProvider() { }
    public CachedExchageRatesProvider(Stream stream)
    {
      dst_EURRates = new DataSet();
      dst_EURRates.ReadXml(stream);
    }
    public CachedExchageRatesProvider(string fileName)
    {
      dst_EURRates = new DataSet();
      dst_EURRates.ReadXml(fileName);
    }
    public CachedExchageRatesProvider(DataSet USDRates)
    {
      this.dst_EURRates = USDRates;
    }
    #endregion

    #region CurrencyExchangeProvider Overrides
    public override double GetExchangeRate(string srcCurrSymbol, string destCurrSymbol)
    {
      if (!CanConvert(srcCurrSymbol, destCurrSymbol))
        throw new ArgumentException(Resources.Errors.UnknownCurrencies);
      DataRow srce = GetCurrency(srcCurrSymbol);
      DataRow dest = GetCurrency(destCurrSymbol);

      return double.Parse(dest[COLUMN_EXCHANGE].ToString()) / double.Parse(srce[COLUMN_EXCHANGE].ToString());
    }

    public override string GetCurrencyName(string symbol)
    {
      return (string)GetCurrency(symbol)[COLUMN_NAME];
    }

    public override string GetCurrencyPlural(string symbol)
    {
      return (string)GetCurrency(symbol)[COLUMN_NAME];
    }

    public override bool CanConvert(string srcCurr, string destCurr)
    {
      return IsCurrencyInTable(srcCurr) && IsCurrencyInTable(destCurr);
    }

    public override IEnumerable<string> Currencies
    {
      get
      {
        foreach (DataRow row in dst_EURRates.Tables[0].Select())
          yield return row[COLUMN_SYMBOL].ToString();
      }
    }

    public void Save(Stream output)
    {
      dst_EURRates.WriteXml(output);
    }
    public void Save(string fileName)
    {
      dst_EURRates.WriteXml(fileName);
    }
    #endregion

    #region Tools
    protected bool IsCurrencyInTable(string symbol)
    {
      return dst_EURRates.Tables[0].Select(COLUMN_SYMBOL + "='" + symbol + "'").Length > 0;
    }

    protected DataRow GetCurrency(string symbol)
    {
      if (!IsCurrencyInTable(symbol))
        throw new ArgumentException(Resources.Errors.UnknownCurrencies);
      return dst_EURRates.Tables[0].Select(COLUMN_SYMBOL + "='" + symbol + "'")[0];
    }
    #endregion
  }

  /// <summary>
  /// Retrieves Exchange Rates from an XML File.
  /// 
  /// Uses XSLT Transforms to retreive data from an XML file. Web providers are suposed to be main source, 
  /// but local files are accepted.
  /// 
  /// Transform output must be:
  /// <code>
  ///  &lt;Rates&gt;<br />
  ///  &nbsp; &lt;Rate&gt;<br />
  ///  &nbsp; &nbsp; &lt;Currency&gt;Euro&lt;/Currency&gt;<br />
  ///  &nbsp; &nbsp; &lt;Symbol&gt;EUR&lt;/Symbol&gt;<br />
  ///  &nbsp; &nbsp; &lt;Exchange&gt;1&lt;/Exchange&gt;<br />
  ///  &nbsp; &lt;/Rate&gt;<br />
  ///  &nbsp; &lt;Rate&gt;<br />
  ///  &nbsp; &nbsp; &lt;Currency&gt;United States Dollar&lt;/Currency&gt;<br />
  ///  &nbsp; &nbsp; &lt;Symbol&gt;USD&lt;/Symbol&gt;<br />
  ///  &nbsp; &nbsp; &lt;Exchange&gt;1.4&lt;/Exchange&gt;<br />
  ///  &nbsp; &lt;/Rate&gt;<br />
  ///  &lt;/Rates&gt;
  /// </code>
  /// 
  /// Currency calculations must be all based on a same single currency. To make sure everything works fine a circular 
  /// definition must exists. That means, if rates are euro based there must be a line for euro itself with rate 1.
  /// 
  /// Default url:
  /// http://www.ecb.int/stats/eurofxref/eurofxref-daily.xml
  /// </summary>
  public class WebExchangeRatesProvider : CachedExchageRatesProvider
  {
    #region Fields
    XslTransform xsl_Transform;
    #endregion

    #region Creation
    public WebExchangeRatesProvider()
      : this("http://www.ecb.int/stats/eurofxref/eurofxref-daily.xml", Assembly.GetExecutingAssembly().
          GetManifestResourceStream("ScaredFingers.UnitsConversion.XmlRates.CreateRates.xslt"))
    {

    }
    public WebExchangeRatesProvider(string fileUrl)
      :
      this(fileUrl, Assembly.GetExecutingAssembly().
          GetManifestResourceStream("ScaredFingers.UnitsConversion.XmlRates.CreateRates.xslt"))
    {

    }
    public WebExchangeRatesProvider(string fileUrl, Stream XSLTFile)
    {
      dst_EURRates = BuildDataSet(fileUrl);
      XmlDocument xslDoc = new XmlDocument();
      xslDoc.Load(XSLTFile);

      xsl_Transform = new XslTransform();
      xsl_Transform.Load(xslDoc);
    }

    DataSet BuildDataSet(string url)
    {
      XmlDocument xmlDoc = new XmlDocument();
      DataSet result = new DataSet();

      if (url.Contains("http://"))
      {
        WebRequest request = HttpWebRequest.Create(url);
        using (WebResponse response = request.GetResponse())
        {
          using (Stream responseStream = response.GetResponseStream())
          {
            xmlDoc.Load(responseStream);
          }
        }
      }
      else
      {
        xmlDoc.Load(url);
      }

      XmlReader reader = xsl_Transform.Transform(xmlDoc, new XsltArgumentList());
      result.ReadXml(reader);

      return result;
    }
    #endregion
  }
}
