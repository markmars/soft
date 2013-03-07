using System;
using System.Windows.Forms;
using System.Reflection;

namespace MarkMars.Factory
{
    public class FormFactory
    {
        /// <summary>
        /// 根据不同的类名装载相应的窗体，主要是为了不同地区考虑。
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        public static Form CreateForm(String formAssemblyFile, String formFullName, String formArgs, String formName, String formText)
        {
            Form form = null;
            Assembly formAssembly = Assembly.LoadFrom(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" + formAssemblyFile);

            if (formArgs == "0")
            {
                form = formAssembly.CreateInstance(formFullName, false, BindingFlags.Default, null, null, null, null) as Form;
            }
            else
            {
                object[] args = new object[1];
                args[0] = formArgs;
                form = formAssembly.CreateInstance(formFullName, false, BindingFlags.Default, null, args, null, null) as Form;
            }

            if (form != null)
            {
                form.Name = formName;
                form.Text = formText;
            }

            return form;
        }
    }
}
