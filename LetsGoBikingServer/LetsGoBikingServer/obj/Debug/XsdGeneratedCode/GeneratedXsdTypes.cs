﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil.
//     Version du runtime :4.0.30319.42000
//
//     Les modifications apportées à ce fichier peuvent provoquer un comportement incorrect et seront perdues si
//     le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LetsGoBikingServer.ContractTypes
{
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "17.4.0+18d5aef85920037c9d6ff49b1215a4daf515197f")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.datacontract.org/2004/07/System.Device.Location")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://schemas.datacontract.org/2004/07/System.Device.Location", IsNullable=true)]
    public partial class GeoCoordinate
    {
        
        private double altitudeField;
        
        private bool altitudeFieldSpecified;
        
        private double courseField;
        
        private bool courseFieldSpecified;
        
        private double horizontalAccuracyField;
        
        private bool horizontalAccuracyFieldSpecified;
        
        private double latitudeField;
        
        private bool latitudeFieldSpecified;
        
        private double longitudeField;
        
        private bool longitudeFieldSpecified;
        
        private double speedField;
        
        private bool speedFieldSpecified;
        
        private double verticalAccuracyField;
        
        private bool verticalAccuracyFieldSpecified;
        
        /// <remarks/>
        public double Altitude
        {
            get
            {
                return this.altitudeField;
            }
            set
            {
                this.altitudeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AltitudeSpecified
        {
            get
            {
                return this.altitudeFieldSpecified;
            }
            set
            {
                this.altitudeFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public double Course
        {
            get
            {
                return this.courseField;
            }
            set
            {
                this.courseField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool CourseSpecified
        {
            get
            {
                return this.courseFieldSpecified;
            }
            set
            {
                this.courseFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public double HorizontalAccuracy
        {
            get
            {
                return this.horizontalAccuracyField;
            }
            set
            {
                this.horizontalAccuracyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool HorizontalAccuracySpecified
        {
            get
            {
                return this.horizontalAccuracyFieldSpecified;
            }
            set
            {
                this.horizontalAccuracyFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public double Latitude
        {
            get
            {
                return this.latitudeField;
            }
            set
            {
                this.latitudeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LatitudeSpecified
        {
            get
            {
                return this.latitudeFieldSpecified;
            }
            set
            {
                this.latitudeFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public double Longitude
        {
            get
            {
                return this.longitudeField;
            }
            set
            {
                this.longitudeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LongitudeSpecified
        {
            get
            {
                return this.longitudeFieldSpecified;
            }
            set
            {
                this.longitudeFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public double Speed
        {
            get
            {
                return this.speedField;
            }
            set
            {
                this.speedField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SpeedSpecified
        {
            get
            {
                return this.speedFieldSpecified;
            }
            set
            {
                this.speedFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public double VerticalAccuracy
        {
            get
            {
                return this.verticalAccuracyField;
            }
            set
            {
                this.verticalAccuracyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool VerticalAccuracySpecified
        {
            get
            {
                return this.verticalAccuracyFieldSpecified;
            }
            set
            {
                this.verticalAccuracyFieldSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "17.4.0+18d5aef85920037c9d6ff49b1215a4daf515197f")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.datacontract.org/2004/07/ProxyService")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://schemas.datacontract.org/2004/07/ProxyService", IsNullable=true)]
    public partial class ArrayOfStation
    {
        
        private Station[] stationField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Station", IsNullable=true)]
        public Station[] Station
        {
            get
            {
                return this.stationField;
            }
            set
            {
                this.stationField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "17.4.0+18d5aef85920037c9d6ff49b1215a4daf515197f")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://schemas.datacontract.org/2004/07/ProxyService")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://schemas.datacontract.org/2004/07/ProxyService", IsNullable=true)]
    public partial class Station
    {
        
        private GeoCoordinate coordinateField;
        
        private string nameField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public GeoCoordinate Coordinate
        {
            get
            {
                return this.coordinateField;
            }
            set
            {
                this.coordinateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "17.4.0+18d5aef85920037c9d6ff49b1215a4daf515197f")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://tempuri.org/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://tempuri.org/", IsNullable=false)]
    public partial class GetStations
    {
        
        private string cityField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string city
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "17.4.0+18d5aef85920037c9d6ff49b1215a4daf515197f")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://tempuri.org/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://tempuri.org/", IsNullable=false)]
    public partial class GetStationsResponse
    {
        
        private Station[] getStationsResultField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute(IsNullable=true)]
        [System.Xml.Serialization.XmlArrayItemAttribute(Namespace="http://schemas.datacontract.org/2004/07/ProxyService")]
        public Station[] GetStationsResult
        {
            get
            {
                return this.getStationsResultField;
            }
            set
            {
                this.getStationsResultField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "17.4.0+18d5aef85920037c9d6ff49b1215a4daf515197f")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://tempuri.org/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://tempuri.org/", IsNullable=false)]
    public partial class ClosestStation
    {
        
        private GeoCoordinate coordinateField;
        
        private string cityField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public GeoCoordinate Coordinate
        {
            get
            {
                return this.coordinateField;
            }
            set
            {
                this.coordinateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string City
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("MSBuild", "17.4.0+18d5aef85920037c9d6ff49b1215a4daf515197f")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://tempuri.org/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://tempuri.org/", IsNullable=false)]
    public partial class ClosestStationResponse
    {
        
        private Station closestStationResultField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public Station ClosestStationResult
        {
            get
            {
                return this.closestStationResultField;
            }
            set
            {
                this.closestStationResultField = value;
            }
        }
    }
}
