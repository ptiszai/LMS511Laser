/* This file is part of *Core*.
Copyright (C) 2015 Tiszai Istvan

*program name* is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Brace.Shared.Core.Configuration
{
    public class BaseDeviceConfiguration
    {
        /// <summary>
        /// saves the configuration into an XML string.
        /// </summary>
        /// <returns>The xml string.</returns>
        protected string SaveToXml<T>()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            StringWriter stringWriter = new StringWriter();
            xmlSerializer.Serialize(stringWriter, this);

            return stringWriter.ToString();
        }

        /// <summary>
        /// Loads the data from an xml string
        /// </summary>
        ///<param name="xmlconfiguration">The xmnl string</param>
        /// <returns>True if succesfull</returns>
        public static T LoadFromXml<T>(string xmlconfiguration)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            StringReader stringReader = new StringReader(xmlconfiguration);
            return (T)xmlSerializer.Deserialize(stringReader);
        }
    }
}
