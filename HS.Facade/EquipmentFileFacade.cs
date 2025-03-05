using HS.DataAccess;
using HS.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HS.Entities;

namespace HS.Facade
{
    public class EquipmentFileFacade:BaseFacade
    {
        public EquipmentFileFacade(ClientContext clientContext)
            : base(clientContext)
        {

        }
        EquipmentFileDataAccess _EquipmentFileDataAccess
        {
            get
            {
                return (EquipmentFileDataAccess)_ClientContext[typeof(EquipmentFileDataAccess)];
            }
        }

        public int InsertEquipmentFile(EquipmentFile EquipmentFile)
        {
           return (int)_EquipmentFileDataAccess.Insert(EquipmentFile);
        }
        public bool UpdateEquipmentFile(EquipmentFile EquipmentFile)
        {
            return _EquipmentFileDataAccess.Update(EquipmentFile)>0;
        }
        public List<EquipmentFile> GetEquipmentFilesByEquipmentId(Guid equipmentId)
        {
            return _EquipmentFileDataAccess.GetByQuery(string.Format("EquipmentId='{0}'", equipmentId));
        }
        public List<EquipmentFile> GetEquipmentFilesByEquipmentIdAndFileType(Guid equipmentId,string FileType)
        {
            return _EquipmentFileDataAccess.GetByQuery(string.Format("EquipmentId='{0}' AND FileType ='{1}' ", equipmentId,FileType));
        }

        public List<EquipmentFile> RemoveEquipmentFileByEquipmentIdAndFileType(Guid equipmentId, string FileType)
        {
            return _EquipmentFileDataAccess.RemoveEquipmentFileByEquipmentIdAndFileType(equipmentId, FileType);
        }

        public EquipmentFile GetById(int v)
        {
            return _EquipmentFileDataAccess.Get(v);
        }
      
        public bool DeleteById(int id)
        {
            return _EquipmentFileDataAccess.Delete(id)>0;
        }

        public List<EquipmentFile> GetAllPDFsByEquipmentIdList(List<Guid> lists)
        {
            string IDList = string.Join("','", lists); ;
            return _EquipmentFileDataAccess.GetByQuery(string.Format("[Filename] like '%.pdf' and EquipmentId in ('{0}')  ", IDList));
        }
    }
}
