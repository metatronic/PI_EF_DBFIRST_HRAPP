using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFDBFristHRApp
{
    class Program
    {
        static void Main(string[] args)
        {
            TRAININGEntities db = new TRAININGEntities();
            //1. LIST ALL THE DATA FROM THE EMPLOYEE TABLE 
            IEnumerable<EMP> query1 = from e in db.EMPs
                                         select e;
            IEnumerable<EMP> query2 = db.EMPs.Select(e => e);

            //LIST ALL THE DATA FROM THE EMPLOYEE TABLE ORDER BY JOB 

            IEnumerable<EMP> query3 = from e in db.EMPs
                                      orderby e.JOB
                                      select e;

            IEnumerable<EMP> query4 = db.EMPs.Select(e => e).OrderBy(e => e.JOB);

            //LIST ALL THE DATA FROM THE EMPLOYEE TABLE WHOSE NAME  START WITH S

            IEnumerable<EMP> query5 = from e in db.EMPs
                                      where e.ENAME.StartsWith("S")
                                      select e;

            IEnumerable<EMP> query6 = db.EMPs.Where(e => e.ENAME.StartsWith("S"));

            // LIST DISTINCT JOB
            IEnumerable<string> query7 = (from e in db.EMPs
                                          select e.JOB).Distinct();

            IEnumerable<string> query8 = db.EMPs.Select(e => e.ENAME).Distinct();

            //FIND THE DETAILS OF ALL MANAGER (IN ANY DEPT) AND ALL  CLERKS IN DEPT 10 
            IEnumerable<EMP> query9 = from e in db.EMPs
                                      where e.JOB == "MANAGER" || (e.JOB == "CLERK" && e.DEPTNO == 10)
                                      select e;

            IEnumerable<EMP> query10 = db.EMPs.Where(e => e.JOB == "MANAGER" || (e.JOB == "CLERK" && e.DEPTNO == 10));

            //FIND THE EMPLOYEES WHO DO NOT RECEIVE A COMMISSION 
            IEnumerable<EMP> query11 = from e in db.EMPs
                                       where e.COMM == null
                                       select e;

            IEnumerable<EMP> query12 = db.EMPs.Where(e => e.COMM == null);

            // FIND ALL EMPLOYEES WHOSE NET EARNINGS (SAL + COMM) IS  GREATER THAN RS. 2000

            IEnumerable<EMP> query13 = from e in db.EMPs
                                       where e.SAL + (e.COMM ?? 0) > 2000
                                       select e;

            IEnumerable<EMP> query14 = db.EMPs.Where(e => e.SAL + (e.COMM ?? 0) > 2000);

            //FIND ALL EMPLOYEE HIRED IN MONTH OF FEBUARY (OF ANY YEAR) 

            IEnumerable<EMP> query15 = from e in db.EMPs
                                       where string.Format("MMMM",e.HIREDATE) == "February"
                                       select e;

            IEnumerable<EMP> query16 = db.EMPs.Where(e => string.Format("MMMM", e.HIREDATE) == "February");

            //LIST TOTAL SALARY FOR THE ORGANIZATION

            decimal query17 = (from e in db.EMPs
                               select e.SAL??0).Sum();

            decimal query18 = db.EMPs.Select(e => e.SAL ?? 0).Sum();

            //LIST TOTAL EMPLOYEES WORKS IN EACH DEPARTMENT 

            var query19 = from e in db.EMPs
                          group e by e.DEPTNO into grp
                          select new { DEPTNO = grp.Key, COUNT = grp.Count() };

            var query20 = db.EMPs.GroupBy(e => e.DEPTNO, (key, g) => new { DEPTNO = key, COUNT = g.Count() });

            //LIST FIRST THREE HIGHEST PAID EMPLOYEES 

            IEnumerable<EMP> query21 = (from e in db.EMPs
                                        orderby e.SAL
                                        select e).Take(3);

            IEnumerable<EMP> query22 = db.EMPs.OrderBy(e => e.SAL).Take(3);

            //DISPLAY THE NAMES, JOB AND SALARY OF ALL EMPLOYEES,  SORTED ON DESCENDING ORDER OF JOB AND WITHIN JOB,  SORTED ON THE DESCENDING ORDER OF SALARY 

            var query23 = from e in db.EMPs
                          orderby e.SAL descending
                          orderby e.JOB
                          select e;

            var query24 = db.EMPs.OrderByDescending(e => e.SAL).OrderBy(e => e.JOB);

            // LIST DEPARTMENT NAME, EMPLOYEE NAME AND JOB
            var query25 = from e in db.EMPs
                          join d in db.DEPTs on e.DEPTNO equals d.DEPTNO
                          select new { d.DNAME, e.ENAME, e.JOB };

            var query26 = db.EMPs.Join(db.DEPTs, e => e.DEPTNO, d => d.DEPTNO, (e, d) => new { d.DNAME, e.ENAME, e.JOB });

            //LIST DEPARTMENT NAME AND MAX SALARY FOR EACH DEPARTMENT 

            var query27 = from d in db.DEPTs
                          join e in db.EMPs on d.DEPTNO equals e.DEPTNO into gj                          
                          from sub in gj.DefaultIfEmpty()
                          group sub by d.DNAME into grp
                          select new { DNAME = grp.Key, MAX = grp.Max(e => e.SAL??0) };

            var query28 = db.DEPTs.GroupJoin(db.EMPs, d => d.DEPTNO, e => e.DEPTNO, (d, e) => new { EMP = e, DEPT = d }).SelectMany(e => e.EMP.DefaultIfEmpty(), (d, e) => new { DNAME = d.DEPT.DNAME, e }).GroupBy(e => e.DNAME, (key, g) => new { DNAME = key, MAX = g.Max(e=>e.e.SAL) });

            //LIST DEPARTMENT NAME AND TOTAL EMPLOYEE WORKING IN EACH  DEPARTMENT ALSO INCLUDE DEPARTMENT WHERE NO EMPLOYEES  ARE WORKING 

            var query29 = from d in db.DEPTs
                          join e in db.EMPs on d.DEPTNO equals e.DEPTNO into gj
                          from sub in gj.DefaultIfEmpty()
                          group sub by d.DNAME into grp
                          select new { DNAME = grp.Key, MAX = grp.Count(e=>e != null) };

            var query30 = db.DEPTs.GroupJoin(db.EMPs, d => d.DEPTNO, e => e.DEPTNO, (d, e) => new { EMP = e, DEPT = d }).SelectMany(e => e.EMP.DefaultIfEmpty(), (d, e) => new { DNAME = d.DEPT.DNAME, e }).GroupBy(e => e.DNAME, (key, g) => new { DNAME = key, MAX = g.Count(e => e.e != null) });

            //SELECT Dept Name FROM Department TABLE 
            //WHILE DISPLAYING DATA ALSO DISPLAY Emp Name BASED ON Department

            var query31 = from d in db.DEPTs
                          select d;

            var query32 = db.DEPTs.Select(d => d);

            foreach (var item in query32)
            {
                Console.WriteLine(item.DNAME);
                foreach (var item2 in item.EMPs)
                {
                    Console.WriteLine("\t"+item2.ENAME);
                }
            }

            //INSERT NEW DEPARTMENT AND EMPLOYEE FOR THAT DEPARTMENT  DEPTNO=50, DEPTNAME=IT 
            //EMPNO = 1001, EMPNAME = RAHUL

            DEPT dep = new DEPT
            {
                DEPTNO = 50,
                DNAME = "IT"
            };

            EMP emp = new EMP
            {
                EMPNO = 1001,
                ENAME = "RAHUL",
                DEPTNO = 50,
            };
            db.DEPTs.Add(dep);
            db.EMPs.Add(emp);
            db.SaveChanges();

            //UPDATE Rahul RECORD WITH SAL = 20000
            EMP rahul = db.EMPs.SingleOrDefault(e => e.EMPNO == 1001);
            if(rahul != null)
            {
                rahul.SAL = 2000;
                int count = db.SaveChanges();
            }

            //Delete Record of Rahul
            EMP rahul1 = db.EMPs.SingleOrDefault(e => e.EMPNO == 1001);
            if (rahul1 != null)
            {
                db.EMPs.Remove(rahul1);
                int count1 = db.SaveChanges();
            }

            foreach (var item in query30)
            {
                Console.WriteLine($"{item.DNAME} {item.MAX}");
            }

            var returndata = db.JobWiseDetails("MANAGER");

            foreach (var item in query23)
            {
                Console.WriteLine(item.EMPNO+ " "+item.SAL+" "+item.JOB);
            }

            Console.ReadLine();
        }
    }
}
