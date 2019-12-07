

namespace TeisterMask.DataProcessor.ExportDto
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml.Serialization;

    [XmlType("Project")]
    public class ProjectExportDto
    {

        public ProjectExportDto()
        {
            this.Tasks = new List<TaskExport>();
        }

        [XmlAttribute("TasksCount")]
        public int TasksCount { get; set; }

        [XmlElement("ProjectName")]
        public string Name { get; set; }

        [XmlElement("HasEndDate")]
        public string HasEndDate { get; set; }


        [XmlArray("Tasks")]
        public List<TaskExport> Tasks { get; set; }
    }

    [XmlType("Task")]
    public class TaskExport
    {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("Label")]
        public string Label { get; set; }
    }
}
