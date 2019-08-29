using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace XORM
{
    /// <summary>
    /// 存储属性映射信息
    /// </summary>
    internal class PropertyMappingInfo
    {
        private string _propertyName;
        private object _propertyDefaultValue;
        private string _columnName;
        private string _propertyTypeName;

        /// <summary>
        /// 设置或返回对象属性名称
        /// </summary>
        public string PropertyName
        {
            get { return this._propertyName; }
            set { this._propertyName = value; }
        }

        /// <summary>
        /// 设置或返回属性默认值
        /// </summary>
        public object PropertyDefaultValue
        {
            get { return this._propertyDefaultValue; }
            set { this._propertyDefaultValue = value; }
        }

        /// <summary>
        /// 设置或返回与属性对应的列名称
        /// </summary>
        public string ColumnName
        {
            get { return this._columnName; }
            set { this._columnName = value; }
        }

        /// <summary>
        /// 设置或返回属性类型
        /// </summary>
        public string PropertyTypeName
        {
            get { return this._propertyTypeName; }
            set { this._propertyTypeName = value; }
        }
    }

    /// <summary>
    /// 存储操作映射信息
    /// </summary>
    public class OperationMappingInfo
    {
        private string _operation;
        private string _storageProcedure;
        private string _propertyName;

        /// <summary>
        /// 设置或返回查询时输入的条件（属性）名称（多个属性之间以“,”隔开）
        /// </summary>
        public string PropertyNames
        {
            get { return this._propertyName; }
            set { _propertyName = value; }
        }

        /// <summary>
        /// 设置或返回操作名称
        /// </summary>
        public string Operation
        {
            get { return this._operation; }
            set { _operation = value; }
        }

        /// <summary>
        /// 设置或返回存储过程名称
        /// </summary>
        public string StorageProcedure
        {
            get { return _storageProcedure; }
            set { _storageProcedure = value; }
        }
    }

    public class MappingInfo
    {
        private ArrayList _properties;
        private ArrayList _operations;
        private string _crurrentOperation;
        private ArrayList _methods;
        private Hashtable _hashTable = new Hashtable();

        public Hashtable PropertiesHashTable
        {
            get
            {
                return _hashTable;
            }
        }

        public MappingInfo()
        {
            _properties = new ArrayList();
            _operations = new ArrayList();
            _methods = new ArrayList();
        }

        public ArrayList Methods
        {
            get { return _methods; }
            set { _methods = value; }
        }

        public ArrayList Properties
        {
            get { return _properties; }
            set
            {
                _properties = value;
                _hashTable.Clear();
                foreach (object obj in _properties)
                {
                    _hashTable.Add(((PropertyMappingInfo)obj).PropertyName.ToString(), obj);
                }
            }
        }

        public ArrayList Operations
        {
            get { return _operations; }
            set { _operations = value; }
        }

        public string CurrentOperation
        {
            get { return _crurrentOperation; }
            set { _crurrentOperation = value; }
        }
    }

    public class ColumnMappingAttribute : Attribute
    {
        private string _columnName;
        private string _defaultValue;

        public ColumnMappingAttribute(string columnname, string defaultvalue)
        {
            this._columnName = columnname;
            this._defaultValue = defaultvalue;
        }

        /// <summary>
        /// 设置或返回数据库中对应列的名称
        /// </summary>
        public string ColumnName
        {
            get { return this._columnName; }
            set { this._columnName = value; }
        }

        /// <summary>
        /// 设置或返回数据库中对应列的默认值（当取出值为Null的时候）
        /// </summary>
        public string DefaultValue
        {
            get { return this._defaultValue; }
            set { this._defaultValue = value; }
        }
    }
}
