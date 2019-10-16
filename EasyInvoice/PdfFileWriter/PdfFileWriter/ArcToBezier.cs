﻿/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	ArcToBezier
//	Convert eliptical arc to Bezier segments.
//
///////////////////////////////////////////////////////////////////////
//	The PDF File Writer library was enhanced to allow drawing of graphic
//	artwork using Windows Presentation Foundation (WPF) classes.
//	These enhancements were proposed by Elena Malnati elena@yelleaf.com.
//	I would like to thank her for providing me with the source code
//	to implement them. Further I would like to thank Joe Cridge for
//	his contribution of code to convert elliptical arc to Bezier curve.
//	The source code was modified to be consistent in style to the rest
//	of the library. Developers of Windows Forms application can benefit
//	from all of these enhancements
//	For further information visit www.joecridge.me/bezier.pdf.
//	Also visit http://p5js.org/ for some coolness
///////////////////////////////////////////////////////////////////////
//
//	Granotech Limited
//	Author: Uzi Granot
//	Version: 1.0
//	Date: April 1, 2013
//	Copyright (C) 2013-2016 Granotech Limited. All Rights Reserved
//
//	PdfFileWriter C# class library and TestPdfFileWriter test/demo
//  application are free software.
//	They is distributed under the Code Project Open License (CPOL).
//	The document PdfFileWriterReadmeAndLicense.pdf contained within
//	the distribution specify the license agreement and other
//	conditions and notes. You must read this document and agree
//	with the conditions specified in order to use this software.
//
//	For version history please refer to PdfDocument.cs
//
/////////////////////////////////////////////////////////////////////

using System;

namespace PdfFileWriter
{
    /// <summary>
    ///     Arc type for DrawArc method
    /// </summary>
    public enum ArcType
    {
        /// <summary>
        ///     Small arc drawn in counter clock wise direction
        /// </summary>
        SmallCounterClockWise,

        /// <summary>
        ///     Small arc drawn in clock wise direction
        /// </summary>
        SmallClockWise,

        /// <summary>
        ///     Large arc drawn in counter clock wise direction
        /// </summary>
        LargeCounterClockWise,

        /// <summary>
        ///     Large arc drawn in clock wise direction
        /// </summary>
        LargeClockWise
    }

    /// <summary>
    ///     Convert eliptical arc to Bezier segments
    /// </summary>
    public static class ArcToBezier
    {
        ////////////////////////////////////////////////////////////////////
        /// <summary>
        ///     Create eliptical arc
        /// </summary>
        /// <param name="ArcStart">Arc start point</param>
        /// <param name="ArcEnd">Arc end point</param>
        /// <param name="Radius">RadiusX as width and RadiusY as height</param>
        /// <param name="Rotate">X axis rotation angle in radians</param>
        /// <param name="Type">Arc type enumeration</param>
        /// <returns>Array of points.</returns>
        ////////////////////////////////////////////////////////////////////
        public static PointD[] CreateArc
        (
            PointD ArcStart,
            PointD ArcEnd,
            SizeD Radius,
            double Rotate,
            ArcType Type
        )
        {
            PointD[] SegArray;
            var ScaleX = Radius.Width / Radius.Height;

            // circular arc
            if (Math.Abs(ScaleX - 1.0) < 0.000001)
            {
                SegArray = CircularArc(ArcStart, ArcEnd, Radius.Height, Type);
            }
            // eliptical arc
            else if (Rotate == 0.0)
            {
                var ScaleStart = new PointD(ArcStart.X / ScaleX, ArcStart.Y);
                var ScaleEnd = new PointD(ArcEnd.X / ScaleX, ArcEnd.Y);
                SegArray = CircularArc(ScaleStart, ScaleEnd, Radius.Height, Type);
                foreach (var Seg in SegArray) Seg.X *= ScaleX;
            }
            // eliptical arc rotated
            else
            {
                var CosR = Math.Cos(Rotate);
                var SinR = Math.Sin(Rotate);
                var ScaleStart = new PointD((CosR * ArcStart.X - SinR * ArcStart.Y) / ScaleX,
                    SinR * ArcStart.X + CosR * ArcStart.Y);
                var ScaleEnd = new PointD((CosR * ArcEnd.X - SinR * ArcEnd.Y) / ScaleX,
                    SinR * ArcEnd.X + CosR * ArcEnd.Y);
                SegArray = CircularArc(ScaleStart, ScaleEnd, Radius.Height, Type);
                foreach (var Seg in SegArray)
                {
                    var X = Seg.X * ScaleX;
                    Seg.X = CosR * X + SinR * Seg.Y;
                    Seg.Y = -SinR * X + CosR * Seg.Y;
                }
            }

            // replace start and end with original points to eliminate rounding errors
            SegArray[0].X = ArcStart.X;
            SegArray[0].Y = ArcStart.Y;
            SegArray[SegArray.Length - 1].X = ArcEnd.X;
            SegArray[SegArray.Length - 1].Y = ArcEnd.Y;
            return SegArray;
        }

        /// <summary>
        ///     Create circular arc
        /// </summary>
        /// <param name="ArcStart">Arc starting point</param>
        /// <param name="ArcEnd">Arc ending point</param>
        /// <param name="Radius">Arc radius</param>
        /// <param name="Type">Arc type</param>
        /// <returns>Array of points.</returns>
        internal static PointD[] CircularArc
        (
            PointD ArcStart,
            PointD ArcEnd,
            double Radius,
            ArcType Type
        )
        {
            // chord line from start point to end point
            var ChordDeltaX = ArcEnd.X - ArcStart.X;
            var ChordDeltaY = ArcEnd.Y - ArcStart.Y;
            var ChordLength = Math.Sqrt(ChordDeltaX * ChordDeltaX + ChordDeltaY * ChordDeltaY);

            // test radius
            if (2 * Radius < ChordLength) throw new ApplicationException("Radius too small.");

            // line perpendicular to chord at mid point
            // distance from chord mid point to center of circle
            var ChordToCircleLen = Math.Sqrt(Radius * Radius - ChordLength * ChordLength / 4);
            var ChordToCircleCos = -ChordDeltaY / ChordLength;
            var ChordToCircleSin = ChordDeltaX / ChordLength;
            if (Type == ArcType.SmallClockWise || Type == ArcType.LargeCounterClockWise)
            {
                ChordToCircleCos = -ChordToCircleCos;
                ChordToCircleSin = -ChordToCircleSin;
            }

            // circle center
            var CenterX = (ArcStart.X + ArcEnd.X) / 2 + ChordToCircleLen * ChordToCircleCos;
            var CenterY = (ArcStart.Y + ArcEnd.Y) / 2 + ChordToCircleLen * ChordToCircleSin;

            // arc angle
            var ArcAngle = 2 * Math.Asin(ChordLength / (2 * Radius));
            if (ArcAngle < 0.001) throw new ApplicationException("Angle too small");

            if (Type == ArcType.LargeCounterClockWise || Type == ArcType.LargeClockWise)
                ArcAngle = 2 * Math.PI - ArcAngle;

            // segment array
            PointD[] SegArray;

            // one segment equal or less than 90 deg
            if (ArcAngle < Math.PI / 2 + 0.001)
            {
                var K1 = 4 * (1 - Math.Cos(ArcAngle / 2)) / (3 * Math.Sin(ArcAngle / 2));
                if (Type == ArcType.LargeClockWise || Type == ArcType.SmallClockWise) K1 = -K1;

                SegArray = new PointD[4];
                SegArray[0] = ArcStart;
                SegArray[1] = new PointD(ArcStart.X - K1 * (ArcStart.Y - CenterY),
                    ArcStart.Y + K1 * (ArcStart.X - CenterX));
                SegArray[2] = new PointD(ArcEnd.X + K1 * (ArcEnd.Y - CenterY), ArcEnd.Y - K1 * (ArcEnd.X - CenterX));
                SegArray[3] = ArcEnd;
                return SegArray;
            }

            // 2, 3 or 4 segments
            var Segments = (int) (ArcAngle / (0.5 * Math.PI)) + 1;
            var SegAngle = ArcAngle / Segments;
            var K = 4 * (1 - Math.Cos(SegAngle / 2)) / (3 * Math.Sin(SegAngle / 2));
            if (Type == ArcType.LargeClockWise || Type == ArcType.SmallClockWise)
            {
                K = -K;
                SegAngle = -SegAngle;
            }

            // segments array
            SegArray = new PointD[3 * Segments + 1];
            SegArray[0] = new PointD(ArcStart.X, ArcStart.Y);

            // angle from cricle center to start point
            var SegStartX = ArcStart.X;
            var SegStartY = ArcStart.Y;
            var StartAngle = Math.Atan2(ArcStart.Y - CenterY, ArcStart.X - CenterX);

            // process all segments
            var SegEnd = SegArray.Length;
            for (var Seg = 1; Seg < SegEnd; Seg += 3)
            {
                var EndAngle = StartAngle + SegAngle;
                var SegEndX = CenterX + Radius * Math.Cos(EndAngle);
                var SegEndY = CenterY + Radius * Math.Sin(EndAngle);
                SegArray[Seg] = new PointD(SegStartX - K * (SegStartY - CenterY),
                    SegStartY + K * (SegStartX - CenterX));
                SegArray[Seg + 1] = new PointD(SegEndX + K * (SegEndY - CenterY), SegEndY - K * (SegEndX - CenterX));
                SegArray[Seg + 2] = new PointD(SegEndX, SegEndY);
                SegStartX = SegEndX;
                SegStartY = SegEndY;
                StartAngle = EndAngle;
            }

            return SegArray;
        }
    }
}