using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.Validation;
using xbase.umc;
using xbase.umc.attributes;

namespace xbase.sdk
{

    [WboAttr(Id = "ValidationAdmin", LifeCycle = LifeCycle.Global, Description = "校验器管理")]
    public class ValidationAdmin
    {
        [WboMethodAttr(Description = "返回所有的校验器到一个列表数据", Title = "获取校验器列表")]
        public List<ValidatorSchema> GetValidatorSchemaList()
        {
            List<ValidatorSchema> ret = new List<ValidatorSchema>();
            string[] Ids = ValidatorSchemaContainer.Instance().GetSchemaIds();
            for (int i = 0; i < Ids.Length; i++)
            {
                string id = Ids[i];
                ValidatorSchema vs = ValidatorSchemaContainer.Instance().GetItem(id);
                ret.Add(vs);
            }
            return ret;
        }
        [WboMethodAttr(Description = "根据ID返回一个校验器", Title = "获取校验器")]
        public ValidatorSchema GetValidatorSchema(string id)
        {
            return ValidatorSchemaContainer.Instance().GetItem(id);
        }

        [WboMethodAttr(Description = "返回全部校验器的ID到一个列表", Title = "获取校验器ID")]
        public string[] GetValidatorSchemaIds()
        {
            return ValidatorSchemaContainer.Instance().GetSchemaIds();
        }

    }

}
