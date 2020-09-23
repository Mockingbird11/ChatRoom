using System;
using System.Collections.Generic;
using System.Text;

namespace CharRoom.Entity.CommonModels
{
    public class ResultModel
    {
        public int Code { get; set; } = 1;
        public string Msg { get; set; } = "请求成功";
        public object Data { get; set; }

        public void NormalError(string errorMsg)
        {
            this.Code = -1;
            this.Msg = errorMsg;
            this.Data = null;
        }
    }
}
