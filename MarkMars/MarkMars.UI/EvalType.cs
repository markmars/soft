using System;

namespace MarkMars.UI
{
    public enum EvalType
    {
        /// <summary>
        /// 适用于施工技术标评审
        /// </summary>
        TechBidEval = 1,

        /// <summary>
        /// 适用于监理方案跟资信评审
        /// </summary>
        PreceptEval = 2,

        /// <summary>
        /// 适用于其它评审
        /// </summary>
        OtherEval = 3,

        /// <summary>
        /// 适用于综合标评审
        /// </summary>
        SynthesisEval = 4,

        /// <summary>
        /// 适用于施工组织设计评审
        /// </summary>
        DesignEval = 5,

        /// <summary>
        /// 适用于项目管理机构评审
        /// </summary>
        AgenciesEval = 6
    }
}
