using System;
using System.Data;
using System.Data.SqlClient;

using HS.Framework;
using HS.Framework.Exceptions;
using HS.Entities;
using HS.Entities.Bases;
using HS.Entities.List;

namespace HS.DataAccess
{
    public partial class PackageCustomerDataAccess
    {
        public PackageCustomerDataAccess(string ConStr) : base(ConStr) { }
        public PackageCustomer GetPackageCustomerByCustomerId(Guid customerId)
        {
            string sqlQuery = @"select PC.*, 
                                ISNULL(SP.PackageName,'') as PackageName,
                                SP.NonConforming,
                                SP.PackageType,
								ISNULL(SP.MinCredit,0) MinCredit,
								ISNULL(SP.MaxCredit,0) MaxCredit,
								PC.NonConformingFee,
                                PC.ActivationFee,
                                ISNULL(SP.UserType,'') as UserType,
                                ISNULL(SP.PackageCode,'') as PackageCode,
                                ISNULL(SP.ActivationFee,0) as ActivationFee,
                                ISNULL(SIT.[Name],'') as SmartInstallTypeVal,
                                ISNULL(SST.[Name],'') as SmartSystemTypeVal,
                                ISNULL(Man.[Name],'') as ManufacturerName

                                from PackageCustomer PC 
                                left join SmartPackage SP on PC.PackageId = SP.PackageId
                                left join SmartInstallType SIT on SIT.Id = PC.SmartInstallTypeId
                                left join SmartSystemType SST on SST.Id = PC.SmartSystemTypeId
                                left join Manufacturer Man on Man.ManufacturerId = pc.ManufacturerId

                                where PC.CustomerId = '{0}'";
            PackageCustomer PackageCustomer = new PackageCustomer();
            try
            {
                sqlQuery = string.Format(sqlQuery, customerId);
                using (SqlCommand cmd = GetSQLCommand(sqlQuery))
                {
                    SqlDataReader reader;
                    long rows = SelectRecords(cmd, out reader);
                    using (reader)
                    {
                        if (reader.Read())
                        {
                            PackageCustomer = new PackageCustomer();
                            FillObject(PackageCustomer, reader);

                            PackageCustomer.PackageName = reader["PackageName"].ToString();
                            PackageCustomer.NonConforming = reader["NonConforming"] != DBNull.Value ? Convert.ToBoolean(reader["NonConforming"]) : false;
                            PackageCustomer.MinCredit = reader["MinCredit"] != DBNull.Value ? Convert.ToDouble(reader["MinCredit"]) : 0.0;
                            PackageCustomer.MaxCredit = reader["MaxCredit"] != DBNull.Value ? Convert.ToDouble(reader["MaxCredit"]) : 0.0;
                            PackageCustomer.NonConformingFee = reader["NonConformingFee"] != DBNull.Value ? Convert.ToDouble(reader["NonConformingFee"]) : 0.0;
                            PackageCustomer.ActivationFee = reader["ActivationFee"] != DBNull.Value ? Convert.ToDouble(reader["ActivationFee"]) : 0.0;
                            PackageCustomer.SmartInstallTypeVal = reader["SmartInstallTypeVal"].ToString();
                            PackageCustomer.SmartSystemTypeVal = reader["SmartSystemTypeVal"].ToString();
                            PackageCustomer.ManufacturerName = reader["ManufacturerName"].ToString();
                            PackageCustomer.UserType = reader["UserType"].ToString();
                            PackageCustomer.PackageCode = reader["PackageCode"].ToString();
                            PackageCustomer.PackageType = reader["PackageType"].ToString();
                            return PackageCustomer;
                        }
                        else
                        {
                            return null;
                        }
                    } 
                }
            }
            catch (Exception ex)
            {
                return PackageCustomer;
            }
        }
    }
}
