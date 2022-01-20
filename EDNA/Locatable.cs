// <copyright file="Locatable.cs" company="alterNERDtive">
// Copyright 2021 alterNERDtive.
//
// This file is part of EDNA.
//
// EDNA is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// EDNA is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with EDNA.  If not, see &lt;https://www.gnu.org/licenses/&gt;.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alterNERDtive.Edna
{
    /// <summary>
    /// A location in the galaxy, represented by coordinates and a value for
    /// precision (all in ly).
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1649:File name should match first type name", Justification = "Either this or wrong class/struct order 🤷")]
    public struct Location
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Location"/> struct.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">The z coordinate.</param>
        /// <param name="precision">The available precision.</param>
        public Location(double x, double y, double z, int precision)
            => (this.X, this.Y, this.Z, this.Precision) = (x, y, z, precision);

        /// <summary>
        /// Initializes a new instance of the <see cref="Location"/> struct.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">The z coordinate.</param>
        public Location(double x, double y, double z)
            => (this.X, this.Y, this.Z, this.Precision) = (x, y, z, 0);

        /// <summary>
        /// Initializes a new instance of the <see cref="Location"/> struct.
        /// </summary>
        /// <param name="location">An EDTS Location to convert.</param>
        public Location(Edts.Location location)
            => (this.X, this.Y, this.Z, this.Precision) = (location.X, location.Y, location.Z, location.Precision);

        /// <summary>
        /// Gets the x coordinate.
        /// </summary>
        public double X { get; }

        /// <summary>
        /// Gets the y coordinate.
        /// </summary>
        public double Y { get; }

        /// <summary>
        /// Gets the z coordinate.
        /// </summary>
        public double Z { get; }

        /// <summary>
        /// Gets the precision to which the location can be calculated. This is
        /// an actual ± for distance, not a precision _per axis_ as for a Location.
        /// </summary>
        public int Precision { get; }

        public static bool operator ==(Location a, Location b)
            => a.X == b.X && a.Y == b.Y && a.Z == b.Z && a.Precision == 0 && b.Precision == 0;

        public static bool operator !=(Location a, Location b)
            => !(a == b);

        /// <inheritdoc/>
        public override bool Equals(object o)
            => o is Location location && this == location;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return Tuple.Create(this.X, this.Y, this.Z, this.Precision).GetHashCode();
        }

        /// <summary>
        /// The distance to another location.
        /// </summary>
        /// <param name="location">The other location.</param>
        /// <returns>The distance between both locations.</returns>
        public Distance DistanceTo(Location location)
        {
            if (this == location && this.Precision == 0)
            {
                return new Distance(value: 0);
            }
            else
            {
                // Precision actually adds up weird. Since Location precision is
                // per each individual axis, not a radius, we’re dealing with a
                // cube, not a sphere.
                //
                // Therefore for a precision p the system can be √(p²+p²+p²) =
                // √(3p²) = √3*p light years from the exact coordinates.
                //
                // For a distance between two Locations with precision p,q these
                // add up as √3(p+q). This is only the worst case scenario and
                // can be less depending on the angle, but worst case is fine
                // here.
                return new Distance(
                    value: Math.Sqrt(
                        Math.Pow(this.X - location.X, 2)
                        + Math.Pow(this.Y - location.Y, 2)
                        + Math.Pow(this.Z - location.Z, 2)),
                    precision: (int)Math.Ceiling(Math.Sqrt(3) * (this.Precision + location.Precision)));
            }
        }
    }

    /// <summary>
    /// A distance between two objects in the galaxy, represented by the
    /// distance itself and a value for precision.
    /// </summary>
    public struct Distance
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Distance"/> struct.
        /// </summary>
        /// <param name="value">The distance, with absolute precision.</param>
        public Distance(double value)
            : this(value: value, precision: 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Distance"/> struct.
        /// </summary>
        /// <param name="value">The distance.</param>
        /// <param name="precision">The available precision.</param>
        public Distance(double value, int precision)
        {
            (this.Value, this.Precision) = (value, precision);
        }

        /// <summary>
        /// Gets the distance value in ly.
        /// </summary>
        public double Value { get; }

        /// <summary>
        /// Gets the available precision in ly.
        /// </summary>
        public double Precision { get; }
    }

    /// <summary>
    /// An object that can be located in the galaxy.
    /// </summary>
    public abstract class Locatable
    {
        /// <summary>
        /// Gets the object’s location in the galaxy.
        /// </summary>
        public Location Coordinates { get; private set; }

        /// <summary>
        /// The distance to another Locatable object.
        /// </summary>
        /// <param name="locatable">The other Locatable object.</param>
        /// <returns>The distance between both objects.</returns>
        public Distance DistanceTo(Locatable locatable)
        {
            return this.Coordinates.DistanceTo(locatable.Coordinates);
        }

        /// <summary>
        /// The distance to a given Location.
        /// </summary>
        /// <param name="location">The Location to compare to.</param>
        /// <returns>The distance to said Location.</returns>
        public Distance DistanceTo(Location location)
        {
            return this.Coordinates.DistanceTo(location);
        }
    }
}
