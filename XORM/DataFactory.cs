using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using XORM;

namespace XORM
{
    public class DataFactory
    {
        #region FillEntity

        /// <summary>
        /// 返回满足查询条件的实体，如果没有满足要求的实体，则返回null
        /// </summary>
        /// <typeparam name="T">目标实体类型</typeparam>
        /// <param name="filterentity">查询对象（也可以用null代替）</param>
        /// <param name="filterclause">查询条件（多个条件的情况下，可以用“,”隔开，也可以用null代替）</param>
        /// <param name="storageprocedure">存储过程名称</param>
        /// <returns></returns>
        public static T FillEntity<T>(object filterentity, string filterclause, string storageprocedure)
            where T : class, new()
        {
            T entity = new T();
            XMLMapping xmlMapping = new XMLMapping();
            Type type = entity.GetType();

            //获取类型的映射信息
            MappingInfo mapInfo = xmlMapping.GetDataMapInfo(type);

            Database db = new Database();
            db.CommandText = storageprocedure;

            //获取查询条件的映射信息
            if (filterentity != null)   //如果查询条件不为null，则进行查询条件的映射，并且为查询语句增加查询参数
            {
                Type filterEntityType = filterentity.GetType();
                MappingInfo filterMapInfo = xmlMapping.GetDataMapInfo(filterEntityType);
                filterMapInfo.CurrentOperation = db.CommandText;
                addParameters(db, filterMapInfo, filterclause, filterentity);
            }

            IDataReader reader = db.GetDataReader();

            T entityout = null;

            if (reader.Read())
            {
                entityout = xmlMapping.CreateObject<T>(reader, mapInfo);
            }

            return entityout;
        }

        #endregion 

        #region FillEntities

        /// <summary>
        /// 查找满足条件的实体集合
        /// </summary>
        /// <typeparam name="T">实体对象参数类型</typeparam>
        /// <param name="filterentity">查询条件对象（也可以用null代替）</param>
        /// <param name="filterclause">查询条件（多个条件的情况下，可以用“,”隔开，也可以用null代替）</param>
        /// <param name="storageprocedure">存储过程名称</param>
        /// <returns></returns>
        public static List<T> FillEntities<T>(object filterentity, string filterclause, string storageprocedure)
            where T : class, new()
        {
            T entityType = new T();
            Type type = entityType.GetType();
            List<T> coll = new List<T>();

            XMLMapping xmlMapping = new XMLMapping();

            //获取类型的映射信息
            MappingInfo mapInfo = xmlMapping.GetDataMapInfo(type);

            Database db = new Database();
            db.CommandText = storageprocedure;

            //获取查询条件的映射信息
            if (filterentity != null)   //如果查询条件不为null，则进行查询条件的映射，并且为查询语句增加查询参数
            {
                Type filterEntityType = filterentity.GetType();
                MappingInfo filterMapInfo = xmlMapping.GetSearchConditionDataMapInfo(filterEntityType);
                filterMapInfo.CurrentOperation = db.CommandText;
                addParameters(db, filterMapInfo, filterclause, filterentity);
            }

            IDataReader reader = db.GetDataReader();

            List<T> entities = new List<T>();
            T entityout = null;

            while (reader.Read())
            {
                //T obj = new T();
                entityout = xmlMapping.CreateObject<T>(reader, mapInfo);
                //entities.Add(entityout);
                entities.Add(entityout);
            }
            db.Close();
            return entities;
        }

        #endregion 

        #region UpdateEntity
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">更新的对象</param>
        /// <param name="filterClause">更新的对象的属性（多个条件的情况下，可以用“,”隔开，也可以用null代替）</param>
        /// <param name="storageprocedure">存储过程</param>
        /// <returns>返回操作所影响的行数，如果为－1，则表示更新失败</returns>
        public static int UpdateEntity(object entity, string filterClause, string storageprocedure)
        {
            XMLMapping xmlMapping = new XMLMapping();

            //获取类型的映射信息
            MappingInfo mapInfo = xmlMapping.GetDataMapInfo(entity.GetType());
            Database db = new Database();
            db.CommandText = storageprocedure;
            mapInfo.CurrentOperation = db.CommandText;
            addParameters(db, mapInfo, filterClause, entity);
            IDbCommand command = db.GetCommand();
            command.CommandTimeout = 1000;
            int result = -1;
            StringBuilder errorMessages = new StringBuilder();

            try
            {
                result = command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    errorMessages.Append("Index #" + i + "\n" +
                        "Message: " + ex.Errors[i].Message + "\n" +
                        "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                        "Source: " + ex.Errors[i].Source + "\n" +
                        "Procedure: " + ex.Errors[i].Procedure + "\n");
                }//返回错误信息、行数、来源、程序

                //控制台抛出异常
                throw new Exception(errorMessages.ToString());
                //Console.WriteLine(errorMessages.ToString());
                //Console.ReadLine();

            }
            return result;

        }

        #endregion

        #region UpdateEntity

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="filterClause"></param>
        /// <param name="storageprocedure">存储过程</param>
        /// <returns>返回操作所影响的行数，如果为－1，则表示更新失败</returns>

        /// <summary>
        /// 具有返回的更新实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">更新的对象</param>
        /// <param name="filterClause">更新的对象的属性（多个条件的情况下，可以用“,”隔开，也可以用null代替）</param>
        /// <param name="returnClause">返回的对象属性（多个条件的情况下，可以用“,”隔开，也可以用null代替）</param>
        /// <param name="storageprocedure">使用的存储过程</param>
        /// <returns>返回的属性与return中的对象属性对应</returns>
        public static T UpdateEntityWithReturn<T>(object entity, string filterClause, string returnClause, string storageprocedure)
            where T : class, new()
        {
            XMLMapping xmlMapping = new XMLMapping();

            //获取类型的映射信息
            MappingInfo mapInfo = xmlMapping.GetDataMapInfo(entity.GetType());
            Database db = new Database();
            db.CommandText = storageprocedure;
            mapInfo.CurrentOperation = db.CommandText;
            addParameters(db, mapInfo, filterClause, returnClause, entity);
            IDbCommand command = db.GetCommand();
            int result;
            StringBuilder errorMessages = new StringBuilder();

            T entityout = null;

            try
            {
                result = command.ExecuteNonQuery();
                //Type t = entity.GetType().
                entityout = xmlMapping.CreateObject<T>((SqlCommand)command, mapInfo);
            }
            catch (SqlException ex)
            {
                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    errorMessages.Append("Index #" + i + "\n" +
                        "Message: " + ex.Errors[i].Message + "\n" +
                        "LineNumber: " + ex.Errors[i].LineNumber + "\n" +
                        "Source: " + ex.Errors[i].Source + "\n" +
                        "Procedure: " + ex.Errors[i].Procedure + "\n");
                }


                throw new Exception(errorMessages.ToString());
                Console.WriteLine(errorMessages.ToString());
                Console.ReadLine();

            }
            return entityout;

        }

        #endregion 

        #region UpdateEntities<T>

        /// <summary>
        /// 根据条件更新实体集合
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entities">待更新的实体集合</param>
        /// <param name="filterclause">更新参数（多个条件的情况下，可以用“,”隔开，也可以用null代替）</param>
        /// <param name="storageprocedure">存储过程</param>
        /// <returns>返回受影响的记录数量</returns>
        public static int UpdateEntities<T>(List<T> entities, string filterclause, string storageprocedure)
            where T : class, new()
        {
            int result = -1;
            if (entities != null && entities.Count > 0)
            {
                XMLMapping xmlMapping = new XMLMapping();
                T entityType = new T();

                //获取类型的映射信息
                MappingInfo mapInfo = xmlMapping.GetDataMapInfo(entityType.GetType());
                Database db = new Database();
                mapInfo.CurrentOperation = storageprocedure;
                addParameters(db, mapInfo, filterclause);
                IDbCommand command = db.GetCommand();
                command.CommandText = storageprocedure;
                for (int i = 0; i < entities.Count; i++)
                {
                    addParametersValue(db, entities[i], mapInfo);
                    command.ExecuteNonQuery();
                    result++;
                }
            }
            return result;
        }

        #endregion 

        #region SetDefaulValueRow
        /// <summary>
        /// 获取一个默认实体集，可用于测试实体错误及显示表头
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> FillDefaulValueRow<T>()
            where T : class, new()
        {
            T entity = new T();
            XMLMapping xmlMapping = new XMLMapping();
            Type type = entity.GetType();
            MappingInfo mapInfo = xmlMapping.GetDataMapInfo(type);
            PropertyMappingInfo pmi = null;
            List<T> entities = new List<T>();

            for (int i = 0; i < mapInfo.Properties.Count; i++)
            {
                pmi = (PropertyMappingInfo)mapInfo.Properties[i];
                object propertyObj = Convert.ChangeType(pmi.PropertyDefaultValue, entity.GetType().GetProperty(pmi.PropertyName).PropertyType);
                entity.GetType().GetProperty(pmi.PropertyName).SetValue(entity, propertyObj, null);
            }

            entities.Add(entity);
            return entities;
        }

        private static DataTable CreateNoDataTable<T>()
            where T : class, new()
        {
            DataTable result = new DataTable();
            T entity = new T();
            PropertyInfo[] propertys = entity.GetType().GetProperties();
            foreach (PropertyInfo pi in propertys)
            {
                result.Columns.Add(pi.Name, pi.PropertyType);
            }
            ArrayList tempList = new ArrayList();
            foreach (PropertyInfo pi in propertys)
            {

                if (pi.PropertyType == typeof(string))
                {
                    string s = "暂无数据";
                    tempList.Add((object)s);
                }
                else
                {
                    object obj = pi.GetValue(entity, null);
                    tempList.Add(obj);
                }
            }
            object[] array = tempList.ToArray();
            result.LoadDataRow(array, true);
            return result;
        }

        #endregion

        #region 辅助函数

        private static void addParameters(Database db, MappingInfo mappingInfo, string filterClause, string returnClause, object entity)
        {
            db.CommandText = mappingInfo.CurrentOperation;

            if (filterClause != null && filterClause.Trim() != "")
            {
                StringCollection parameterNames = getParameterNames(filterClause);
                //Queue parms = new Queue(parameters);
                string columnName = null;
                foreach (string name in parameterNames)
                {
                    columnName = name;

                    object obj = null;

                    if (mappingInfo.PropertiesHashTable.Contains(name.ToLower()))
                    {
                        columnName = ((PropertyMappingInfo)mappingInfo.PropertiesHashTable[name.ToLower()]).ColumnName;
                        obj = entity.GetType().GetProperty(name).GetValue(entity, null);
                        if (obj == null)
                        {
                            obj = ((PropertyMappingInfo)mappingInfo.PropertiesHashTable[name.ToLower()]).PropertyDefaultValue;
                        }
                        db.AddParameters(columnName, obj);
                    }

                    //for (int i = 0; i < mappingInfo.Properties.Count; i++)
                    //{
                    //    if (name == ((PropertyMappingInfo)mappingInfo.Properties[i]).PropertyName)
                    //    {
                    //        columnName = ((PropertyMappingInfo)mappingInfo.Properties[i]).ColumnName;
                    //    }
                    //}
                    //obj = entity.GetType().GetProperty(name).GetValue(entity, null);

                    //db.AddParameters(columnName, obj);
                }
            }

            if (returnClause != null && returnClause.Trim() != "")
            {
                StringCollection parameterNames = getParameterNames(returnClause);
                //Queue parms = new Queue(parameters);
                string columnName = null;
                foreach (string name in parameterNames)
                {
                    columnName = name;
                    for (int i = 0; i < mappingInfo.Properties.Count; i++)
                    {
                        if (name == ((PropertyMappingInfo)mappingInfo.Properties[i]).PropertyName)
                        {
                            columnName = ((PropertyMappingInfo)mappingInfo.Properties[i]).ColumnName;
                        }
                    }
                    object obj = entity.GetType().GetProperty(name).GetValue(entity, null);

                    db.AddInOutParameters(columnName, obj);
                }
            }
        }

        private static void addParameters(Database db, MappingInfo mappingInfo, string filterClause)
        {
            db.CommandText = mappingInfo.CurrentOperation;

            if (filterClause != null && filterClause.Trim() != "")
            {
                StringCollection parameterNames = getParameterNames(filterClause);

                string columnName = null;

                foreach (string name in parameterNames)
                {
                    columnName = name;
                    if (mappingInfo.PropertiesHashTable.Contains(name.ToLower()))
                    {
                        columnName = ((PropertyMappingInfo)mappingInfo.PropertiesHashTable[name.ToLower()]).ColumnName;
                        db.AddParameters(columnName, null);
                    }
                    //db.AddParameters(columnName, null);
                }
            }
        }

        private static void addParametersValue(Database db, object obj, MappingInfo mappingInfo)
        {

            string propertyName = null;

            foreach (IDataParameter parm in db.Parameters)
            {
                propertyName = parm.ParameterName;
                for (int i = 0; i < mappingInfo.Properties.Count; i++)
                {
                    if (parm.ParameterName == "@" + ((PropertyMappingInfo)mappingInfo.Properties[i]).ColumnName)
                    {
                        parm.Value = obj.GetType().GetProperty(((PropertyMappingInfo)mappingInfo.Properties[i]).PropertyName).GetValue(obj, null);
                        if (parm.Value == null)
                        {
                            parm.Value = ((PropertyMappingInfo)mappingInfo.Properties[i]).PropertyDefaultValue;
                        }
                    }
                }
            }
        }

        private static void addParameters(Database db, MappingInfo mappingInfo, string filterClause, object[] parameters)
        {
            db.CommandText = mappingInfo.CurrentOperation;  //把注销取消

            if (filterClause != null && filterClause.Trim() != "")
            {
                StringCollection parameterNames = getParameterNames(filterClause);
                Queue parms = new Queue(parameters);
                string columnName = null;
                foreach (string name in parameterNames)
                {
                    //columnName = name;

                    if (mappingInfo.PropertiesHashTable.Contains(name.ToLower()))
                    {
                        columnName = ((PropertyMappingInfo)mappingInfo.PropertiesHashTable[name.ToLower()]).ColumnName;
                        db.AddParameters(columnName, parms.Dequeue());
                    }

                    //for (int i = 0; i < mappingInfo.Properties.Count; i++)
                    //{
                    //    if (name == ((PropertyMappingInfo)mappingInfo.Properties[i]).PropertyName)
                    //    {
                    //        columnName = ((PropertyMappingInfo)mappingInfo.Properties[i]).ColumnName;
                    //    }
                    //}

                }
            }
        }

        private static void addParameters(Database db, MappingInfo mappingInfo, string filterClause, object entity)
        {
            db.CommandText = mappingInfo.CurrentOperation;

            if (filterClause != null && filterClause.Trim() != "")
            {
                StringCollection parameterNames = getParameterNames(filterClause);

                //Queue parms = new Queue(parameters);
                string columnName = null;
                foreach (string name in parameterNames)
                {
                    columnName = name;
                    object obj = null;

                    if (mappingInfo.PropertiesHashTable.Contains(name.ToLower()))
                    {
                        columnName = ((PropertyMappingInfo)mappingInfo.PropertiesHashTable[name.ToLower()]).ColumnName;
                        obj = entity.GetType().GetProperty(name).GetValue(entity, null);
                        if (obj == null)
                        {
                            obj = ((PropertyMappingInfo)mappingInfo.PropertiesHashTable[name.ToLower()]).PropertyDefaultValue;
                        }
                        db.AddParameters(columnName, obj);
                    }

                    //for (int i = 0; i < mappingInfo.Properties.Count; i++)
                    //{
                    //    if (name == ((PropertyMappingInfo)mappingInfo.Properties[i]).PropertyName)
                    //    {
                    //        columnName = ((PropertyMappingInfo)mappingInfo.Properties[i]).ColumnName;
                    //        obj = entity.GetType().GetProperty(name).GetValue(entity, null);
                    //        if(obj == null)
                    //        {
                    //            obj = ((PropertyMappingInfo)mappingInfo.Properties[i]).PropertyDefaultValue;
                    //        }
                    //        break;
                    //    }
                    //}


                }
            }
        }

        /// <summary>
        /// Gets a list of parameters names from the where / filter clause
        /// </summary>
        /// <param name="filterClause">A SQL where clause without the WHERE</param>
        /// <returns></returns>
        private static StringCollection getParameterNames(string filterClause)
        {
            StringCollection names = new StringCollection();
            foreach (string piece in Regex.Split(filterClause, @",", RegexOptions.Compiled))
            {
                names.Add(piece.Trim());
            }
            return names;
        }

        #endregion


    }
}
