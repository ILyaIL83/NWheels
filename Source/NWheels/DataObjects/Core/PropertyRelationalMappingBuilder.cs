﻿namespace NWheels.DataObjects.Core
{
    public class PropertyRelationalMappingBuilder : MetadataElement<IPropertyRelationalMapping>, IPropertyRelationalMapping
    {
        #region IMetadataElement Members

        public override string ReferenceName
        {
            get
            {
                return string.Format("{0}.{1}:{2}", TableName, ColumnName, DataTypeName);
            }
        }

        #endregion

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        #region IPropertyRelationalMapping Members

        public IStorageDataType StorageType { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string DataTypeName { get; set; }

        #endregion

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public override void AcceptVisitor(IMetadataElementVisitor visitor)
        {
            StorageType = visitor.VisitAttribute("StorageType", StorageType);
            TableName = visitor.VisitAttribute("TableName", TableName);
            ColumnName = visitor.VisitAttribute("ColumnName", ColumnName);
            DataTypeName = visitor.VisitAttribute("DataTypeName", DataTypeName);
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public override string ToString()
        {
            return 
                (string.IsNullOrEmpty(TableName) ? "" : "TABLE(" + TableName + ").") +
                (string.IsNullOrEmpty(ColumnName) ? "" : "COLUMN(" + ColumnName + ")") +
                (string.IsNullOrEmpty(DataTypeName) ? "" : ".DATATYPE(" + DataTypeName + ")");
        }
    }
}
