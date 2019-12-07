

namespace TeisterMask.DataProcessor.ImportDto
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using System.Xml.Serialization;

    [XmlType("Project")]
    public class ProjectImportDto
    {

        [Required]
        [MinLength(2)]
        [MaxLength(40)]
        [XmlElement("Name")]
        public string Name { get; set; }



        [Required]
        [XmlElement("OpenDate")]
        public string OpenDate { get; set; }


        [XmlElement("DueDate")]
        public string DueDate { get; set; }

        [XmlArray("Tasks")]
        public TaskImportDto[] Tasks { get; set; }

    }


    [XmlType("Task")]
    public class TaskImportDto
    {

        [MinLength(2)]
        [MaxLength(40)]
        [Required]
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("OpenDate")]
        [Required]
        public string OpenDate { get; set; }

        [XmlElement("DueDate")]
        [Required]
        public string DueDate { get; set; }

        [Required]
        [XmlElement("ExecutionType")]
        public string ExecutionType { get; set; }

        [Required]
        [XmlElement("LabelType")]
        public string LabelType { get; set; }

    }

}
