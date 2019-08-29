using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Data;
using System.Collections;
using System.Reflection;
using System.Data.SqlClient;

namespace XORM
{
    class XMLMapping
    {
        #region GetDataMapInfo(string operation, Type type)

        public MappingInfo GetDataMapInfo(Type type)
        {
            MappingInfo mapInfo = GetMappingInfo(type);
            return mapInfo;
        }

        public MappingInfo GetSearchConditionDataMapInfo(Type type)
        {
            MappingInfo mapInfo = GetSearchConditionMappingInfo(type);
            return mapInfo;
        }

        #endregion 

        #region CreateObject(IDataReader reader, MappingInfo mappinginfo, object obj)

        public T CreateObject<T>(IDataReader reader, MappingInfo mappinginfo) where T : class, new()
        {
            T obj = new T();

            PropertyMappingInfo pmi = null;
            object val = null;
            bool stringOrNot = false;

            string fieldName = "";
            //从数据库中读取出的列为reader.FieldCount
            for (int j = 0; j < reader.FieldCount; j++)
            {
                fieldName = reader.GetName(j).ToLower().Trim();

                for (int i = 0; i < mappinginfo.Properties.Count; i++)
                {

                    pmi = (PropertyMappingInfo)mappinginfo.Properties[i];

                    if (pmi.ColumnName.ToLower().Trim() != fieldName)
                        continue;

                    if (reader[pmi.ColumnName] != DBNull.Value)
                    {
                        val = reader[pmi.ColumnName];
                    }
                    else
                    {
                        val = pmi.PropertyDefaultValue;
                    }
                    PropertyInfo ppp = obj.GetType().GetProperty(pmi.PropertyName);
                    if (ppp == null)
                        continue;
                    if (obj.GetType().GetProperty(pmi.PropertyName).PropertyType.Equals(typeof(System.Enum)))
                    {
                        obj.GetType().GetProperty(pmi.PropertyName).SetValue(obj,
                                System.Enum.ToObject(obj.GetType().GetProperty(pmi.PropertyName).PropertyType, val), null);

                    }
                    else
                    {
                        obj.GetType().GetProperty(pmi.PropertyName).SetValue(obj,
                            Convert.ChangeType(val, obj.GetType().GetProperty(pmi.PropertyName).PropertyType), null);

                    }
                }
            }


            return obj;
        }

        public T CreateObject<T>(SqlCommand command, MappingInfo mappinginfo) where T : class, new()
        {
            T obj = new T();

            PropertyMappingInfo pmi = null;
            object val = null;
            bool stringOrNot = false;

            string fieldName = "";
            //从数据库中读取出的列为reader.FieldCount
            for (int j = 0; j < command.Parameters.Count; j++)
            {

                if (command.Parameters[j].Direction != ParameterDirection.Output)
                    continue;

                fieldName = command.Parameters[j].ParameterName.ToLower().Trim().Replace("@", "");

                for (int i = 0; i < mappinginfo.Properties.Count; i++)
                {

                    pmi = (PropertyMappingInfo)mappinginfo.Properties[i];

                    if (pmi.ColumnName.ToLower().Trim() != fieldName)
                        continue;

                    if (command.Parameters[j].Value != DBNull.Value)
                    {
                        val = command.Parameters[j].Value;
                    }
                    else
                    {
                        val = pmi.PropertyDefaultValue;
                    }
                    PropertyInfo ppp = obj.GetType().GetProperty(pmi.PropertyName);
                    if (ppp == null)
                        continue;
                    if (obj.GetType().GetProperty(pmi.PropertyName).PropertyType.Equals(typeof(System.Enum)))
                    {
                        obj.GetType().GetProperty(pmi.PropertyName).SetValue(obj,
                                System.Enum.ToObject(obj.GetType().GetProperty(pmi.PropertyName).PropertyType, val), null);

                    }
                    else
                    {
                        obj.GetType().GetProperty(pmi.PropertyName).SetValue(obj,
                            Convert.ChangeType(val, obj.GetType().GetProperty(pmi.PropertyName).PropertyType), null);

                    }
                }
            }

            return obj;
        }

        #endregion 

        #region CreateObject(IDataReader reader, MappingInfo mappinginfo, object obj)

        public object CreateObject(IDataReader reader, MappingInfo mappinginfo, object obj)
        {
            PropertyMappingInfo pmi = null;
            object val = null;
            bool stringOrNot = false;

            string fieldName = "";
            //从数据库中读取出的列为reader.FieldCount
            for (int j = 0; j < reader.FieldCount; j++)
            {
                fieldName = reader.GetName(j).ToLower().Trim();

                for (int i = 0; i < mappinginfo.Properties.Count; i++)
                {

                    pmi = (PropertyMappingInfo)mappinginfo.Properties[i];

                    if (pmi.ColumnName.ToLower().Trim() != fieldName)
                        continue;

                    if (reader[pmi.ColumnName] != DBNull.Value)
                    {
                        val = reader[pmi.ColumnName];

                        stringOrNot = obj.GetType().GetProperty(pmi.PropertyName).PropertyType.Equals(typeof(System.String));
                        if (stringOrNot)
                        {
                            val = ((string)reader[pmi.ColumnName]).Trim();
                        }
                    }
                    else
                    {
                        val = pmi.PropertyDefaultValue;

                    }

                    if (obj.GetType().GetProperty(pmi.PropertyName).PropertyType.Equals(typeof(System.Enum)))
                    {
                        obj.GetType().GetProperty(pmi.PropertyName).SetValue(obj,
                                System.Enum.ToObject(obj.GetType().GetProperty(pmi.PropertyName).PropertyType, val), null);

                    }
                    else
                    {
                        obj.GetType().GetProperty(pmi.PropertyName).SetValue(obj,
                            Convert.ChangeType(val, obj.GetType().GetProperty(pmi.PropertyName).PropertyType), null);

                    }
                }
            }


            return obj;
        }
        #endregion 

        #region CreateObject (MappingInfo mappinginfo, object obj)

        public object CreateObject(MappingInfo mappinginfo, object obj)
        {
            PropertyMappingInfo pmi = null;
            object val = null;
            for (int i = 0; i < mappinginfo.Properties.Count; i++)
            {
                pmi = (PropertyMappingInfo)mappinginfo.Properties[i];

                val = pmi.PropertyDefaultValue;
                try
                {
                    obj.GetType().GetProperty(pmi.PropertyName).SetValue(obj, val, null);
                }
                catch
                {
                    if (obj.GetType().GetProperty(pmi.PropertyName).PropertyType.Equals(typeof(System.Enum)))
                    {
                        obj.GetType().GetProperty(pmi.PropertyName).SetValue(obj,
                                System.Enum.ToObject(obj.GetType().GetProperty(pmi.PropertyName).PropertyType, val), null);
                    }
                    else
                    {
                        obj.GetType().GetProperty(pmi.PropertyName).SetValue(obj,
                            Convert.ChangeType(val, obj.GetType().GetProperty(pmi.PropertyName).PropertyType), null);
                    }
                }

            }

            return obj;
        }

        #endregion 

        #region GetMappingInfo

        /// <summary>
        /// 获取映射信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private MappingInfo GetMappingInfo(Type type)
        {
            MappingInfo info = MappingInfoCache.GetCache(type.Name);

            //如果本地没有缓存映射信息，则从对应的XML文件中加载该对象的映射信息
            if (info == null)
            {
                info = loadMappinInfoFromXml(type);
                MappingInfoCache.SetCache(type.Name, info);
            }
            return info;
        }

        /// <summary>
        /// 获取映射信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private MappingInfo GetSearchConditionMappingInfo(Type type)
        {
            MappingInfo info = MappingInfoCache.GetCache(type.Name);

            //如果本地没有缓存映射信息，则从对应的XML文件中加载该对象的映射信息
            if (info == null)
            {
                info = loadMappinInfoFromXml(type);
                MappingInfoCache.SetCache(type.Name, info);
            }
            return info;
        }

        #endregion 

        #region loadMappinInfoFromXml

        /// <summary>
        /// 从XML文件中加载映射信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private MappingInfo loadMappinInfoFromXml(Type type)
        {
            string name = type.Name;
            PropertyInfo[] properties = type.GetProperties();
            if (properties == null)
                return null;
            MappingInfo mappingInfo = new MappingInfo();
            PropertyMappingInfo pmi;

            foreach (PropertyInfo pi in properties)
            {
                ColumnMappingAttribute columnMappingAttribute = (ColumnMappingAttribute)Attribute.GetCustomAttribute(pi, typeof(ColumnMappingAttribute));
                if (columnMappingAttribute == null)
                {
                    continue;
                }
                pmi = new PropertyMappingInfo();
                pmi.PropertyName = pi.Name;
                pmi.ColumnName = columnMappingAttribute.ColumnName;
                pmi.PropertyDefaultValue = columnMappingAttribute.DefaultValue;
                mappingInfo.Properties.Add(pmi);
                mappingInfo.PropertiesHashTable.Add(pmi.PropertyName.ToLower(), pmi);
            }

            //MethodInfo[] methods = type.GetMethods();
            ////type.GetMethods(BindingFlags.FlattenHierarchy);
            //if (methods == null)
            //    return null;
            //OperationMappingInfo operationMappingInfo = null;
            //foreach (MethodInfo mi in methods)
            //{
            //    StorageProcedureMappingAttribute storageProcedureMappingAttribute = (StorageProcedureMappingAttribute)Attribute.GetCustomAttribute(mi, typeof(StorageProcedureMappingAttribute));
            //    if (storageProcedureMappingAttribute == null)
            //    {
            //        continue;
            //    }
            //    operationMappingInfo = new OperationMappingInfo();
            //    operationMappingInfo.StorageProcedure = storageProcedureMappingAttribute.StorageProcedureName;
            //    operationMappingInfo.PropertyNames = storageProcedureMappingAttribute.PropertyNames;
            //    mappingInfo.Methods.Add(operationMappingInfo);
            //}

            return mappingInfo;
        }

        #endregion 
    }
}
