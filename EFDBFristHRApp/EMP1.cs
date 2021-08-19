using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDBFristHRApp
{
    public partial class EMP
    {
        public override string ToString()
        {
            return string.Format($"{EMPNO} {ENAME} {JOB} {MGR} {HIREDATE} {SAL} {COMM} {DEPTNO}");
        }
    }
}
