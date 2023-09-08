using Derify.Core.Entities;
using Derify.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Derify.Core.Services
{
    public class DerifyService : IDerifyService
    {
        private readonly IBaseRepository _baseRepository;
        public DerifyService(IBaseRepository repository) 
        {
            _baseRepository = repository;
        }

        public string GetCode()
        {
            StringBuilder code = new StringBuilder();

            List<RelationEntity> relationEntities = _baseRepository.GetAll();
            IEnumerable<string> tableNames = relationEntities.Select(re => re.TableName).Distinct();
            
            int tableCount = 0;
            int topPos= 100;
            foreach (string tableName in tableNames)
            {
                IEnumerable<string> columns = relationEntities.Where(re => re.TableName == tableName).Select(re => {
                    List<string> constraints = new List<string>();
                    if (re.IsPrimaryKey) constraints.Add("<img src=\"data:image/png;base64, iVBORw0KGgoAAAANSUhEUgAAADIAAAAyCAYAAAAeP4ixAAAABGdBTUEAALGPC/xhBQAAAfFJREFUaEPtmO1RwzAMhjUCI3SEbgAbwAZlBDaADegGZAQ2gP9paTdINiAbpK9cFUhwck5jKYH6uXuvvcTWR21VTiiRSCSiU+/oqs7ptt7SY72htx96cdd2tJSh8wQBLlywG6oDVEArmTofsALPrUBDVcxihdw2Om4bX5Dh2tK9mLRHamHnDewcTZUMnIfWQ7istxn/et5AxqsQFzaww1YA8WS1xc5ajZze8bmG9o3rftmsChxlLcd9qtr7Hkk9ecY1ZVErcPT5y3GXELRMa4B7/SuzpQcZqofXcZdyupFpDXCPt5l/zlFrGaoDB+Zx2q2OwpWa8c9h4b4M1WFwItww0ThluiPIxuwSYR27/6r+oGusEJ+IQ2rsVVzq4XEaXx1/ElGBI71m+K07cacDHMQ/X3kk7nSAA5MkoExcxgfGrZLgrr4Qt3GBcbsktBohDNslodU7YNxyJfbtxhkFGLZMIvvrSZSQTr+A4XFJ8LstHMHxveuYXkH8PKPX8GB89EqIqS/c2Uwkl3RBEFG2k5ibBgQQrSbEpD1wHrOwSzFrCx+VPcGEqnTzT+IC1/gLDQEB9D9u9kn7CW4IKRHWv0nE4pk6lAGJVG7sSfxOSuuZ4RyCE8E4mTJPLi8R7deXY8E+X7pk+oSinqzRJRKJS4XoADZZ23FlSjAZAAAAAElFTkSuQmCC\" width=\"17\"/>");
                    if (re.IsForeignKey) constraints.Add("<img src=\"data:image/png;base64, iVBORw0KGgoAAAANSUhEUgAAADIAAAAyCAYAAAAeP4ixAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwgAADsIBFShKgAAAAlJJREFUaEPtmM1NAkEYhinAgzeuHizADqQDvZB4EsNPwnE70NiAdCAlcPCuBZhAB9CB2wE+H/kwMM7+sDszOxDe5A3L7vf3MvPN7NA6owSSJLkcDod38HkwGHzu8F3ujUajGzWNE+Px+EqKhesSXMKeusYDfuk3o9CyXEYxQjKNKEamja3I0kTMk4YMD+2Fua2wKmxMDMnL9kNpBp9m8uvZCnHApaYIA0loFOCMwaZYldGgl774nMDF7v0MhhkVEk2NxHlMzXmPqBeL3R6D9AqJfszEWZSi1W0PPMsdGfwSNfUHW+IsUlBH3fbAM5lmVh/lRE39QAqzJM1kVuMSR3rG6iOU52rqB4cKwX4uG6e6b1AmRnRChCKGzx6jc8u1vBEX9hh2M03pD7bErokQ6yLhFCTythnu8F7T+QEJnL9f2ajp/IAEQUTAqaZ0D4KHErGWk6amdQuCBxMB/WyEBA4mgpXKz95B8JAjsTA3TicgcEgR02MXsYJ+9gsC1xIhKw5zPeE66zU9hXKe8bfhEbz2SGioPyCqs6Xe8guKcDKdNFwzoABnPaEhw4PkLht7pWEzgU273++nRcTuQ12Kwbwt/BMghyvx3yEraPESil/biJPFg4TkHjfzKL4a5iDge5pCmEbfxHo0yf0HdSkGDlEI0UfVUVNIpTM1vo0KScV2S75Pqp4Z8G1OiNipS20QzxSyWWotvFaXYsQgJIfuhUBnpzZiuRci/3iLmALOXJ4VKPBfj8BXk91u90Jd4oRNiD46LpyFxIazkNhwMkLOOG20Wr80/s4PcuyfawAAAABJRU5ErkJggg==\" width=\"17\" />");
                    if (re.IsUnique) constraints.Add("<img src=\"data:image/png;base64, iVBORw0KGgoAAAANSUhEUgAAADIAAAAyCAYAAAAeP4ixAAAABGdBTUEAALGPC/xhBQAAAAlwSFlzAAAOwgAADsIBFShKgAAAAx1JREFUaEPtmD+LE0EYxmNhb2Mlgo2laKGgnOKVFoqHh20u/zZJmQ8gKhYKCnrfwBQWByKcFjZXqJXlXS1C8g1MYyeev1ffHLPjTLJJZiYbuAcesjvv3yc7s5NJ5RgF0Ov1TrVarbvwUbPZ/GTwtYxlWXZJXcuJbrd7TpqFhwU4gFsaWh7wTb+yGi3KQSmekEwjmpFp42qyMBFT05TpoWth39XYPFyaGIoXXQ+FmXyaybfnaiQAB1oiDaSg1UAwJpti8zwN1tJnPrfhgTnuYZqnQqG+VXgSR/a8R9Rjh1+OSdYKhX7YhX2UpjUsB2wTnwxxPXWNB1dhH2loXcNywCbTzBmj3FbXOJDGHEW99C1c8siaccYIxa6ucTCrEPz3ZePU8L8okqN0QoQihs8tns5NruUX8dQ1ht+ulowHV+HQRIjzJREUFIq2GRrc0HJxQIHgv69c1HJxQIEkImBfS4YHyVOJOJSTppYNC5InEwHjbIQkTiaCN1WcvYPkKZ/Egb1xBgGJU4ror7qIIYyzX5B4IRHyxmGu97j2/UwfQTnPxNvwSL7wk9BUR0DU+pg6FBc0EWQ6abrlgAaCrQlNmR4UD7mwh5r2PzQajU04GhNf58bH+JrlN31vYd5O/RNgAocSb5A3qP8VSlObVvwLNeXA+Jrlt6cmPyg+8bg5iRKraQrBFsL9MzXlgM0W8lFNfixTCPFP1JQDtpwQ4j6oyY9lCoEP1ZQD47aQd2ryY0EhM52pbSHcP1BTDthsITtq8mMGISPxHZP77VnPDPMKgW/U5EdRIeKnIXODxjfMnCsrhBy3zZw+IYxfM/1gOCFw4VMbOW6ZOX1Csiy7YvrB6ULkH28RM4W7Ic4KNH7ZbJD7l2rKoV6vX7f8nPvN0kCDp80G4Xs15cD4fdMPIQ01lQf09NNsslqtnlHTEfDZMX2YETfUVB7Q5FuzSe6/QjlgnajVame5fmra4bd/kSUD0+ui1eiYvx1j5ZxWYyDmuatpm2iI929jKDDvr9LsF/jLbF6IgO983lPX1UGn0zlP43fa7fYFeFKHj7GiqFT+AFAbJ31PpAvfAAAAAElFTkSuQmCC\" width=\"17\" />");

                    string constraint = string.Join(",", constraints);

                    return $@"
                                <tr>
                                    <td class=""gray"">{constraint}</td>
                                    <td>{re.FieldName}</td>
                                    <td class=""accent"">{re.FieldDataType}</td>
                                    <td>{re.Nulleable}</td>
                                </tr>
                    ";
                });

                string startEntity = $@"
                    <div class=""card"" id=""{tableName}"" style=""left: {(tableCount * 400)+100}px; top: {topPos}px;"">
                        <div class=""card--title"">
                            {tableName}
                        </div>
                        <div class=""card--body"">
                            <table>
                                <tr>
                                    <th>Key</th>
                                    <th>Property</th>
                                    <th>Type</th>
                                    <th>Nullable</th>
                                </tr>
                ";

                code.AppendLine(startEntity);

                foreach (string column in columns)
                    code.AppendLine(column);

                string endEntity = $@"
                            </table>
                        </div>
                    </div>
                ";
                code.AppendLine(endEntity);


                if (tableCount < 3)
                {
                    tableCount++;
                }
                else
                {
                    tableCount = 0;
                    topPos += 400;
                }
            }

            //Conecto las entidades
            foreach (string tableName in tableNames)
            {
                IEnumerable<string> relations = relationEntities.Where(re => re.TableName == tableName && !string.IsNullOrEmpty(re.ReferencedBy)).Select(re => {
                    return $"<div source=\"{re.ReferencedBy}\" target=\"{tableName}\" class=\"link\"></div>";
                });

                foreach (string relation in relations)
                    code.AppendLine(relation);
            }

            return code.ToString();
        }

        
    }
}
