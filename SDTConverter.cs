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
 * - Created ConvertMassDensity, ConvertMassFlowRate, ConvertSurfaceTension, ConvertVelocity, ConvertViscosity, ConvertVolumeFlowRate, ConvertVolumeFlowRatePerArea, ConvertPressure, ConvertPressureDrop, ConvertMolecularWeight, ConvertMomentumUnits and ConvertAreaUnits methods of SDTConverter class. 
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace ScaredFingers.UnitsConversion
{
    public class SDTConverter 
    {
        public double ConvertLength(double num, int codeFrom, int codeTo)
        {
            UnitTable table = UnitTable.LengthTable;
            return ConvertNum(num, codeFrom, codeTo, table);
        }

        public double ConvertTemperature(double num, int codeFrom, int codeTo)
        {
            UnitTable table = UnitTable.TemperatureTable;
            return ConvertNum(num, codeFrom, codeTo, table);
        }

        public double ConvertVolume(double num, int codeFrom, int codeTo)
        {
            UnitTable table = UnitTable.VolumeTable;
            return ConvertNum(num, codeFrom, codeTo, table);
        }

        public double ConvertWeight(double num, int codeFrom, int codeTo)
        {
            UnitTable table = UnitTable.WeightTable;
            return ConvertNum(num, codeFrom, codeTo, table);
        }

        public double ConvertMassDensity(double num, int codeFrom, int codeTo)
        {
            UnitTable table = UnitTable.MassDensityTable;
            return ConvertNum(num, codeFrom, codeTo, table);
        }

        public double ConvertMassFlowRate(double num, int codeFrom, int codeTo)
        {
            UnitTable table = UnitTable.MassFlowRateTable;
            return ConvertNum(num, codeFrom, codeTo, table);
        }

        public double ConvertSurfaceTension(double num, int codeFrom, int codeTo)
        {
            UnitTable table = UnitTable.SurfaceTensionTable;
            return ConvertNum(num, codeFrom, codeTo, table);
        }

        public double ConvertVelocity(double num, int codeFrom, int codeTo)
        {
            UnitTable table = UnitTable.VelocityTable;
            return ConvertNum(num, codeFrom, codeTo, table);
        }

        public double ConvertVolumeFlowRate(double num, int codeFrom, int codeTo)
        {
            UnitTable table = UnitTable.VolumeFlowRateTable;
            return ConvertNum(num, codeFrom, codeTo, table);
        }

        public double ConvertVolumeFlowRatePerArea(double num, int codeFrom, int codeTo)
        {
            UnitTable table = UnitTable.VolumeFlowRatePerAreaTable;
            return ConvertNum(num, codeFrom, codeTo, table);
        }
               
        public double ConvertViscosity(double num, int codeFrom, int codeTo)
        {
            UnitTable table = UnitTable.ViscosityTable;
            return ConvertNum(num, codeFrom, codeTo, table);
        }

        public double ConvertPressure(double num, int codeFrom, int codeTo)
        {
            UnitTable table = UnitTable.PressureTable;
            return ConvertNum(num, codeFrom, codeTo, table);
        }

        public double ConvertPressureDrop(double num, int codeFrom, int codeTo)
        {
            UnitTable table = UnitTable.PressureDrop;
            return ConvertNum(num, codeFrom, codeTo, table);
        }

        public double ConvertMolecularWeight(double num, int codeFrom, int codeTo)
        {
            UnitTable table = UnitTable.MolecularWeightTable;
            return ConvertNum(num, codeFrom, codeTo, table);
        }

        public double ConvertMomentumUnits(double num, int codeFrom, int codeTo)
        {
            UnitTable table = UnitTable.MomentumTable;
            return ConvertNum(num, codeFrom, codeTo, table);
        }

        public double ConvertAreaUnits(double num, int codeFrom, int codeTo)
        {
            UnitTable table = UnitTable.AreaTable;
            return ConvertNum(num, codeFrom, codeTo, table);
        }

        private double ConvertNum(double num, int codeFrom, int codeTo, UnitTable table)
        {
            //if (num == 0)
            //{
            //    return 0;
            //}
            //else 
            if (codeFrom == codeTo)
            {
                return num;
            }
            else
            {
                Unit unit = new Unit(codeFrom, /*float.Parse(num.ToString())*/num, table);
                //return Math.Round(Convert.ToDouble(unit.Convert(codeTo).Value), 10, MidpointRounding.AwayFromZero);
                
                return Convert.ToDouble(unit.Convert(codeTo).Value);
            }
        }
    }
}
