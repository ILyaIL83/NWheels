﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NWheels.DataObjects;

namespace NWheels.Core.DataObjects
{
    public class PropertyRelationalMappingBuilder : MetadataElement<IPropertyRelationalMapping>, IPropertyRelationalMapping
    {
        #region IPropertyRelationalMapping Members

        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string DataTypeName { get; set; }

        #endregion

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public override void AcceptVisitor(IMetadataElementVisitor visitor)
        {
            TableName = visitor.VisitAttribute("TableName", TableName);
            ColumnName = visitor.VisitAttribute("ColumnName", ColumnName);
            DataTypeName = visitor.VisitAttribute("DataTypeName", DataTypeName);
        }
    }
}
