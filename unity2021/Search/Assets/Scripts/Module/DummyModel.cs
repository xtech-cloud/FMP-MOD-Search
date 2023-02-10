
using System;
using LibMVCS = XTC.FMP.LIB.MVCS;

namespace XTC.FMP.MOD.Search.LIB.Unity
{
    /// <summary>
    /// 虚拟数据
    /// </summary>
    public class DummyModel : DummyModelBase
    {

        public class Record
        {
            public string initials;
            public string uri;
            public string alias;
        }

        public class DummyStatus : DummyStatusBase
        {
        }

        public DummyModel(string _uid) : base(_uid)
        {
        }
    }
}

