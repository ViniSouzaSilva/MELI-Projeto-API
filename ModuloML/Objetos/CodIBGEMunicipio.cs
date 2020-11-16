using System;
using System.Collections.Generic;
using System.Text;

namespace ModuloML.Objetos
{
    class CodIBGEMunicipio
    {

        // OBSERVAÇÃO: o código gerado pode exigir pelo menos .NET Framework 4.5 ou .NET Core/Standard 2.0.
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class municipios
        {

            private municipiosMunicipio[] municipioField;

            /// <remarks/>
            [System.Xml.Serialization.XmlElementAttribute("municipio")]
            public municipiosMunicipio[] municipio
            {
                get
                {
                    return this.municipioField;
                }
                set
                {
                    this.municipioField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class municipiosMunicipio
        {

            private uint iD_MunicipioField;

            private string mun_DescField;

            private string ufField;

            /// <remarks/>
            public uint ID_Municipio
            {
                get
                {
                    return this.iD_MunicipioField;
                }
                set
                {
                    this.iD_MunicipioField = value;
                }
            }

            /// <remarks/>
            public string Mun_Desc
            {
                get
                {
                    return this.mun_DescField;
                }
                set
                {
                    this.mun_DescField = value;
                }
            }

            /// <remarks/>
            public string UF
            {
                get
                {
                    return this.ufField;
                }
                set
                {
                    this.ufField = value;
                }
            }
        }


    }
}
