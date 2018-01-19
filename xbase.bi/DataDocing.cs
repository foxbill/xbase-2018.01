using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using xbase.bi.schema;
using xbase.data;
using xbase.umc;
using xbase;
using xbase.Exceptions;
using xbase.umc.attributes;

namespace xbase.bi
{
    [WboAttr(Id = "DataDoc", Title = "分析报告", Version = 1.1, LifeCycle = LifeCycle.Session, ContainerType = typeof(DataDocSchemaContainer)
   , IsPublish = true)]
    public class DataDoc
    {
        public const char PathChar = '/';
        private DataDocSchema schema = null;
        //   private ListData<Subject> subjects;

        public DataDoc(string name)
        {
            if (DataDocSchemaContainer.Instance().Contains(name))
                this.schema = DataDocSchemaContainer.Instance().GetItem(name);
        }

        public xbase.ObjCatelog GetCatalog()
        {
            xbase.ObjCatelog ret = new xbase.ObjCatelog();
            ret.Id = schema.Id;
            ret.Title = schema.Title;
            ret.Description = schema.Description;
            ret.ObjType = this.GetType().Name;
            ret.Path = PathChar + "";
            for (int i = 0; i < schema.Subjects.Count; i++)
            {
                xbase.ObjCatelog subjectCata = new xbase.ObjCatelog();
                subjectCata.Path = PathChar + schema.Subjects[i].Id;
                ret.Children.Add(subjectCata);
                BuildSubjectCatalog(subjectCata, schema.Subjects[i]);
            }
            return ret;
        }

        private void BuildSubjectCatalog(xbase.ObjCatelog catalog, SubjectSchema subjectSchema)
        {
            catalog.Id = subjectSchema.Id;
            catalog.Title = subjectSchema.Title;
            catalog.Description = subjectSchema.Description;
            catalog.ObjType = typeof(Subject).Name;

            for (int i = 0; i < subjectSchema.ChildSubjects.Count; i++)
            {
                xbase.ObjCatelog childCate = new xbase.ObjCatelog();
                childCate.Path = catalog.Path + PathChar + subjectSchema.ChildSubjects[i].Id;
                catalog.Children.Add(childCate);
                BuildSubjectCatalog(childCate, subjectSchema.ChildSubjects[i]);
            }
        }

        public Subject GetSubject(string path)
        {
            if (path == null) path = "";

            if (path.StartsWith(PathChar + ""))
                path = path.Remove(0, 1);

            string[] ids = path.Split(new char[] { PathChar });

            xbase.SchemaList<SubjectSchema> ssList = schema.Subjects;

            string sPath = "";
            SubjectSchema ss = null;
            Subject subj = null;
            for (int i = 0; i < ids.Length; i++)
            {
                string id = ids[i];
                sPath += PathChar + id;
                ss = ssList.FindItem(id);
                if (ss == null)
                    throw new XException("分析文档，不能按路径找到指定的主题。" + sPath);
                ssList = ss.ChildSubjects;

            }
            subj = new Subject(ss, ids.Length, sPath);
            return subj;

        }

        public bool SaveSubject(string path, string title, string text, string chartId)
        {
            Subject subj = GetSubject(path);
            subj.Save(title, text, chartId);
            DataDocSchemaContainer.Instance().UpdateItem(schema.Id, schema);
            return true;
        }



        public List<Subject> DrillFirstChartSubject(string subjectPath)
        {
            List<Subject> ret = new List<Subject>();
            Subject subj = GetSubject(subjectPath);
            DrillFirstChartSubject(subj, ret);
            return ret;
        }
        private bool DrillFirstChartSubject(Subject subject, List<Subject> DrillRet)
        {
            DrillRet.Add(subject);

            if (subject.HasChart)
                return true;

            List<Subject> childSubjects = subject.GetChildSubject();
            for (int i = 0; i < childSubjects.Count; i++)
            {
                Subject child = childSubjects[i];
                if (DrillFirstChartSubject(child, DrillRet))
                    return true;
            }
            return false;
        }

        public List<Subject> DrillDown(string subjectPath)
        {

            List<Subject> ret = new List<Subject>();
            if (string.IsNullOrEmpty(subjectPath) || subjectPath.Equals("/") || subjectPath.Equals("\\"))
            {
                for (int i = 0; i < schema.Subjects.Count; i++)
                {
                    Subject subjl=GetSubject(schema.Subjects[i].Id);
                    DrillDown(subjl, ret);

                }
                return ret;
            }
            Subject subj = GetSubject(subjectPath);
           
            DrillDown(subj, ret);
            return ret;
        }

        private void DrillDown(Subject subject, List<Subject> DrillRet)
        {
            DrillRet.Add(subject);
            List<Subject> childSubjects = subject.GetChildSubject();
            for (int i = 0; i < childSubjects.Count; i++)
            {
                Subject child = childSubjects[i];
                DrillDown(child, DrillRet);
            }
        }

        public DataDocSchema GetSchema()
        {
            return this.schema;
        }

        public string Title
        {
            get
            {
                return schema.Title;
            }
        }

    }


    public class Subject
    {
        private SubjectSchema schema;
        private int level;

        public Subject(SubjectSchema schema, int level, string path)
        {
            this.schema = schema;
            this.level = level;
            this.path = path;
        }

        public string PageLink
        {
            get
            {
                return schema.PageLink;
            }
        }

        public bool HasChart
        {
            get
            {
                return (!string.IsNullOrEmpty(schema.ChartId));
                //&& string.IsNullOrEmpty(schema.Text)
                //&& string.IsNullOrEmpty(schema.Description)
                // );
            }
        }

        public string TableId
        {
            get { return schema.TableId; }
        }

        public string Text
        {
            get { return schema.Text; }
        }

        public string Description
        {
            get { return schema.Description; }
        }

        public string Title
        {
            get { return schema.Title; }
        }

        public string ChartId
        {
            get { return schema.ChartId; }
        }

        public int Level
        {
            get { return level; }
            set { level = value; }
        }
        private string path;

        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        internal void Save(string title, string text, string chartId)
        {
            schema.Title = title;
            schema.Text = text;
            schema.ChartId = chartId;
        }

        public SubjectSchema GetSubjectSchema()
        {
            return schema;
        }

        public List<Subject> GetChildSubject()
        {
            List<Subject> ret = new List<Subject>();
            for (int i = 0; i < schema.ChildSubjects.Count; i++)
            {
                SubjectSchema ss = schema.ChildSubjects[i];
                Subject sub = new Subject(ss, level + 1, path + DataDoc.PathChar + ss.Id);
                ret.Add(sub);
            }
            return ret;
        }


    }

}
