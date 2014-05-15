using SWN.TTR.Repository.Common;
using System;

namespace SWN.TTR.Repository.Common
{
    public sealed class DataAccessFactory
    {
        public static IOdsRepository CreateOdsRepository()
        {
            return (IOdsRepository)Activator.CreateInstance(Type.GetType(SqlHelper.OdsConnectionProvider));
        }

        public static ITravelAndTrainingRequestRepository CreateTTRRepository()
        {
            return (ITravelAndTrainingRequestRepository)Activator.CreateInstance(Type.GetType(SqlHelper.TTRConnectionProvider));
        }
    }
}