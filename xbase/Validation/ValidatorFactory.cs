using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.umc;
using xbase.ObjectAction;
using xbase.BaseTypes;
using xbase.Exceptions;

namespace xbase.Validation
{

    public delegate object CheckVarHandle(string value);

    /// <summary>
    /// 校验器类工厂
    /// </summary>
    public static class ValidatorFactory
    {
        private static Dictionary<string, Validator> list = new Dictionary<string, Validator>();

        private static Validator CreateValidator(string name)
        {
            WboSchema objSchema = ValidatorSchemaContainer.Instance().GetItem(name);
            object obj = ObjectFactory.CreateObject(objSchema);
            if (!(obj is Validator))
                throw new XException("加载的对象没有继承校验接口，不是校验器类型");
            return (obj as Validator);
        }

        /// <summary>
        /// 获取校验器
        /// </summary>
        /// <param name="validatorName"></param>
        /// <returns></returns>
        public static Validator GetValidator(string validatorName)
        {
            Validator ret = null;
            if (list.ContainsKey(validatorName))
                ret = list[validatorName];
            else
            {
                ret = CreateValidator(validatorName);
                list.Add(validatorName, ret);
            }
            return ret;
        }

        /// <summary>
        /// 通过校验定义Schema, 执行校验
        /// </summary>
        /// <param name="validationSchema"></param>
        /// <param name="value"></param>
        /// <param name="errHint"></param>
        /// <returns></returns>
        public static bool InvokeValid(ValidationSchema validationSchema, string value, CheckVarHandle checkHandl, out string errHint)
        {
            List<ValidationItemSchema> items = validationSchema.Validators;



            for (int i = 0; i < items.Count; i++)
            {
                ValidationItemSchema item = items[i];
                IdsObjectList<IdValueObject> options = new IdsObjectList<IdValueObject>();
                for (int j = 0; j < item.Options.Count; j++)
                {
                    IdValueObject op = item.Options[j];
                    IdValueObject newOp = new IdValueObject();
                    newOp.Id = op.Id;
                    newOp.Value = op.Value;
                    if (checkHandl != null)
                        newOp.Value = checkHandl(newOp.Value).ToString();
                    options.Add(newOp);
                }



                Validator validator = GetValidator(item.ValidatorName);

                ActionUtils.SetObjProperties(validator, options);
                errHint = validator.ErrHint;
                if (!validator.Check(value)) return false;
            }

            errHint = "";
            return true;
        }

        /// <summary>
        /// 注册校验类
        /// </summary>
        /// <param name="type"></param>
        public static void RegisterClass(Type type)
        {
            ValidatorSchema os = WboSchemaRegisterUtils.BuildObjectSchema<ValidatorSchema>(type);

            if (!ValidatorSchemaContainer.Instance().Contains(os.Id))
                ValidatorSchemaContainer.Instance().AddItem(os.Id, os);

        }
    }
}
