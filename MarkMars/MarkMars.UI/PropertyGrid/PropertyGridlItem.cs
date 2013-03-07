using System;
using System.Windows.Forms;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace MarkMars.UI
{
    public class PropertyGridTenderWay : ItemTypeConvert
    {
        public override void GetConvertHash()
        {
            _hash.Add(0, "");
            _hash.Add(1, "公开招标");
            _hash.Add(2, "邀请招标");
            _hash.Add(3, "邀请招标（代资格预审通过通知书）");
        }
    }

    public class PropertyGridTenderWay_PY_SG : ItemTypeConvert
    {
        public override void GetConvertHash()
        {
            _hash.Add(0, "");
            _hash.Add(1, "公开招标");
        }
    }

    public class PropertyGridTenderWay_MY_SG : ItemTypeConvert
    {
        public override void GetConvertHash()
        {
            _hash.Add(0, "");
            _hash.Add(1, "公开招标");
            _hash.Add(2, "邀请招标");
        }
    }

    public class PropertyGridTenderWay_SZ_SJ : ItemTypeConvert
    {
        public override void GetConvertHash()
        {
            _hash.Add(0, "");
            _hash.Add(1, "公开招标");
            _hash.Add(2, "邀请招标");
        }
    }

    public class PropertyGridExamineWay : ItemTypeConvert
    {
        public override void GetConvertHash()
        {
            _hash.Add(0, "");
            _hash.Add(1, "投标报名");
            _hash.Add(2, "资格预审");
            _hash.Add(3, "资格后审");
        }
    }

    public class PropertyGridTenderWay_BJ_SG : ItemTypeConvert
    {
        public override void GetConvertHash()
        {
            _hash.Add(0, "");
            _hash.Add(1, "公开招标");
            _hash.Add(2, "邀请招标");
        }
    }

    public class PropertyGridFileItem : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = String.Format("招标、答疑文件|*{0};*{1}", ".NZF", ".NCF");//招标文件
                ofd.AddExtension = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    return ofd.FileName;
                }
            }

            return value;
        }
    }

    public class PropertyGridFileNCFItem : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = String.Format("答疑文件(*{0})|*{0}", ".NCF");//招标文件
                ofd.AddExtension = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    return ofd.FileName;
                }
            }

            return value;
        }
    }

    public class PropertyGridFileNZFItem : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = String.Format("招标文件(*{0})|*{0}", ".NZF");//招标文件
                ofd.AddExtension = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    return ofd.FileName;
                }
            }

            return value;
        }
    }

    public class PropertyGridFileKZJQdItem : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = String.Format("招标控制价文件(*{0})|*{0}", ".ENTF");//控制价文件
                ofd.AddExtension = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    return ofd.FileName;
                }
            }

            return value;
        }
    }

    public class PropertyGridFileZBQdItem : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = String.Format("招标工程量清单文件(*{0})|*{0}", ".ENZF");
                ofd.AddExtension = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    return ofd.FileName;
                }
            }

            return value;
        }
    }

    public class PropertyGridFileZBQdItem_BJTL_SG : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = String.Format("招标工程量清单文件(*{0})|*{0}", ".TLZBS");
                ofd.AddExtension = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    return ofd.FileName;
                }
            }

            return value;
        }
    }

    public class PropertyGridDateTimeItem : UITypeEditor
    {
        DateTimePicker dateControl = new DateTimePicker();

        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.None;
        }

        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, System.IServiceProvider provider, object value)
        {
            IWindowsFormsEditorService edSvc = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            if (edSvc != null)
            {
                DateTimePicker hms = new DateTimePicker();
                hms.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
                hms.CustomFormat = "yyy-MM-dd HH:mm:ss";
                hms.ShowUpDown = true;
                hms.Value = DateTime.Now;
                return dateControl.Text;
            }

            return value;
        }
    }

    public class PropertyGridBoolItem : ItemTypeConvert
    {
        public override void GetConvertHash()
        {
            _hash.Add(0, "否");
            _hash.Add(1, "是");
        }
    }

    public class PropertyGridComboBoxItemQualification : ItemTypeConvert
    {
        public override void GetConvertHash()
        {
            _hash.Add(0, "资格预审");
            _hash.Add(1, "资格后审");
        }
    }

    public class PropertyGridComboBoxItemMethod : ItemTypeConvert
    {
        public override void GetConvertHash()
        {
            _hash.Add(0, "最低投标价法");
            _hash.Add(1, "综合评估法");
        }
    }

    public class PropertyGridComboBoxItemNumberType : ItemTypeConvert
    {
        public override void GetConvertHash()
        {
            _hash.Add(2, "区间分");//直接打分
            _hash.Add(6, "等级分");//分档打分
        }
    }

    public class PropertyGridComboBoxItemBZRZhiCheng : ItemTypeConvert
    {
        public override void GetConvertHash()
        {
            _hash.Add(101, "高级工程师");
            _hash.Add(102, "工程师");
            _hash.Add(103, "助理工程师");
            _hash.Add(104, "技术员");
            _hash.Add(201, "高级经济师");
            _hash.Add(202, "经济师");
            _hash.Add(203, "助理经济师");
            _hash.Add(301, "总工程师");
            _hash.Add(302, "副总工程师");
            _hash.Add(401, "教授");
            _hash.Add(402, "副教授");
            _hash.Add(501, "高级建筑师");
            _hash.Add(502, "建筑师");
            _hash.Add(601, "高级讲师");
            _hash.Add(602, "讲师");
            _hash.Add(701, "高级会计师");
            _hash.Add(702, "会计师");
            _hash.Add(703, "助理会计师");
            _hash.Add(801, "高级工艺美术师");
            _hash.Add(901, "高级审计师");
        }
    }
    public class PropertyGridComboBoxItemBZRZhuCeZiGe : ItemTypeConvert
    {
        public override void GetConvertHash()
        {
            _hash.Add(0, "无职称");
            _hash.Add(1, "注册造价师");
            _hash.Add(2, "造价员");
        }
    }
    public class PropertyGridComboBoxItemHeTong : ItemTypeConvert
    {
        public override void GetConvertHash()
        {
            _hash.Add(1, "示范合同");
            _hash.Add(2, "自拟合同");
        }
    }

    // ▽ 2010-11-17 NJ_SG_BS-443 牟瑞 追加 关于项目工程类别
    /// <summary>
    /// 工程项目类别
    /// </summary>
    public class PropertyGridComboBoxItemProjectType : ItemTypeConvert
    {
        public override void GetConvertHash()
        {
            _hash.Add(0, "建设工程项目");
            _hash.Add(1, "其他工程项目");
        }
    }

    /// <summary>
    /// 2011-09-26 Chenx 计价方式
    /// </summary>
    public class PropertyGridValuationWay_TY_SG : ItemTypeConvert
    {
        public override void GetConvertHash()
        {
            _hash.Add(0, "");
            _hash.Add(1, "定额计价");
            _hash.Add(2, "清单计价");
        }
    }

    public class PropertyGridTenderWay_TY_SG : ItemTypeConvert
    {
        public override void GetConvertHash()
        {
            _hash.Add(0, "");
            _hash.Add(1, "公开招标");
            _hash.Add(2, "邀请招标");
            _hash.Add(3, "邀请招标（代资格预审通过通知书）");
        }
    }

    public class PropertyGridTenderWay_SN_SG : ItemTypeConvert
    {
        public override void GetConvertHash()
        {
            _hash.Add(0, "");
            _hash.Add(1, "公开招标");
            _hash.Add(2, "邀请招标");
            _hash.Add(3, "邀请招标（代资格预审通过通知书）");
        }
    }

    public class PropertyGridTenderWay_RZ_JL : ItemTypeConvert
    {
        public override void GetConvertHash()
        {
            _hash.Add(0, "");
            _hash.Add(1, "公开招标");
            _hash.Add(2, "邀请招标");
        }
    }

    public class PropertyGridTenderWay_DY_SG : ItemTypeConvert
    {
        public override void GetConvertHash()
        {
            _hash.Add(0, "");
            _hash.Add(1, "公开招标");
            _hash.Add(2, "邀请招标");
            _hash.Add(3, "邀请招标（代资格预审通过通知书）");
        }
    }

    public class PropertyGridTenderWay_DZ_SG : ItemTypeConvert
    {
        public override void GetConvertHash()
        {
            _hash.Add(0, "");
            _hash.Add(1, "公开招标");
            _hash.Add(2, "邀请招标");
            _hash.Add(3, "邀请招标（代资格预审通过通知书）");
        }
    }

    public class PropertyGridTenderWay_TL_HW : ItemTypeConvert
    {
        public override void GetConvertHash()
        {
            _hash.Add(0, "");
            _hash.Add(1, "公开招标");
            _hash.Add(2, "邀请招标");
        }
    }

    public class PropertyGridOfferWay_TL_HW : ItemTypeConvert
    {
        public override void GetConvertHash()
        {
            _hash.Add(0, "");
            _hash.Add(1, "到站价");
            _hash.Add(2, "出厂价");
        }
    }

    public class PropertyGridTenderWay_AY_SG : ItemTypeConvert
    {
        public override void GetConvertHash()
        {
            _hash.Add(0, "");
            _hash.Add(1, "公开招标");
            _hash.Add(2, "邀请招标");
            _hash.Add(3, "邀请招标（代资格预审通过通知书）");
        }
    }

    public class PropertyGridTenderWay_BJTL_SG : ItemTypeConvert
    {
        public override void GetConvertHash()
        {
            _hash.Add(0, "");
            _hash.Add(1, "公开招标");
            _hash.Add(2, "邀请招标");
            _hash.Add(3, "邀请招标（代资格预审通过通知书）");
        }
    }

    public class PropertyGridTenderWay_TL_JL : ItemTypeConvert
    {
        public override void GetConvertHash()
        {
            _hash.Add(0, "");
            _hash.Add(1, "公开招标");
            _hash.Add(2, "邀请招标");
        }
    }

    public class PropertyGridQualExamWay_BJTL_YS : ItemTypeConvert
    {
        public override void GetConvertHash()
        {
            _hash.Add(0, "");
            _hash.Add(1, "合格制");
            _hash.Add(2, "有限数量制");
        }
    }
}