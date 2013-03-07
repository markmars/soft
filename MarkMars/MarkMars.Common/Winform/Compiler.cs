using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Text;

namespace MarkMars.Common
{
    public class Compiler
    {
        /// <summary>
        /// 执行字符串代码。
        /// </summary>
        /// <param name="stringCode">字符串代码。</param>
        /// <param name="paramNameString">执行参数名（格式：类型 参数名[, 类型 参数名]……）。</param>
        /// <param name="paramValues">执行参数值。</param>
        /// <returns>执行结果。</returns>
        public Object StringCodeExecution(String stringCode, String paramNameString, Object[] paramValues)
        {
            return this.StringCodeExecution(stringCode, paramNameString, paramValues, null, String.Empty);
        }

        /// <summary>
        /// 执行字符串代码。
        /// </summary>
        /// <param name="stringCode">字符串代码。</param>
        /// <param name="paramNameString">执行参数名（格式：类型 参数名[, 类型 参数名]……）。</param>
        /// <param name="paramValues">执行参数值。</param>
        /// <param name="assemblyFileFullNameList">引用的程序集文件全名列表。</param>
        /// <param name="usingNameSpaceList">引用的命名空间列表。</param>
        /// <returns>执行结果。</returns>
        public Object StringCodeExecution(String stringCode, String paramNameString, Object[] paramValues, String[] assemblyFileFullNameList, String usingNameSpaceList)
        {
            CompilerParameters compilerParameters = new CompilerParameters();
            compilerParameters.GenerateExecutable = false;
            compilerParameters.GenerateInMemory = true;
            compilerParameters.ReferencedAssemblies.Add("System.dll");
            compilerParameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");

            if (assemblyFileFullNameList != null)
            {
                compilerParameters.ReferencedAssemblies.AddRange(assemblyFileFullNameList);
            }

            StringBuilder source = new StringBuilder();
            source.AppendLine("using System;");
            source.AppendLine("using System.Windows.Forms;");
            source.AppendLine("using System.Collections.Generic;");

            if (!String.IsNullOrEmpty(usingNameSpaceList))
            {
                source.AppendLine(usingNameSpaceList);
            }

            source.AppendLine("public class Temp");
            source.AppendLine("{");
            source.AppendLine(String.Format("    public Object Execution({0})", paramNameString));
            source.AppendLine("    {");
            source.AppendLine("        " + stringCode);
            source.AppendLine("        return null;");
            source.AppendLine("    }");
            source.AppendLine("}");

            CompilerResults compilerResults = CodeDomProvider.CreateProvider("CSharp").CompileAssemblyFromSource(compilerParameters, source.ToString());
            Assembly assembly = compilerResults.CompiledAssembly;
            Object temp = assembly.CreateInstance("Temp");
            MethodInfo execution = temp.GetType().GetMethod("Execution");
            return execution.Invoke(temp, paramValues);
        }
    }
}