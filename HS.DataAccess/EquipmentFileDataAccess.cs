using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;
using System.Collections.Generic;

namespace HS.DataAccess
{
    public partial class EquipmentFileDataAccess
    {
        public EquipmentFileDataAccess(string ConStr) : base(ConStr) { }
        public List<EquipmentFile> RemoveEquipmentFileByEquipmentIdAndFileType(Guid equipmentId, string fileType)
        {
            string SqlQuery = @" select * from EquipmentFile where EquipmentId ='{0}' 
                                and FileType ='{1}'

                                delete from EquipmentFile where EquipmentId ='{0}' 
                                and FileType ='{1}'
                                ";
            try
            {
                SqlQuery = string.Format(SqlQuery, equipmentId, fileType);

                using (SqlCommand cmd = GetSQLCommand(SqlQuery))
                {

                    return GetList(cmd, -1);
                }
            }
            catch (Exception ex)
            {
                return null;
            } 
        }
    }
}
