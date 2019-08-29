using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace XORM
{
    
      /// 对象与数据库中的映射信息全部缓存在本地内存中
      /// </summary>
        public class MappingInfoCache
        {
            private static Hashtable _cache = new Hashtable();

            /// <summary>
            /// 返回缓存在本地内存中的对象与列的映射信息
            /// </summary>
            /// <param name="typename">对象的类型名称</param>
            /// <returns></returns>
            public static MappingInfo GetCache(string typename)
            {
                MappingInfo info = null;
                try
                {
                    info = (MappingInfo)_cache[typename];
                }
                catch
                {

                }

                return info;
            }

            public static void SetCache(string typename, MappingInfo mappinginfo)
            {
                _cache[typename] = mappinginfo;
            }

            public static void ClearCache()
            {
                _cache.Clear();
            }
        
    }
}
