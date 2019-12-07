

namespace TeisterMask.DataProcessor.ExportDto
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class EmployeeExportDto
    {

        public EmployeeExportDto()
        {
            this.Tasks = new List<TaskExportDto>();
        }
        public string Username { get; set; }


        public List<TaskExportDto> Tasks { get; set; }
    }

    public class TaskExportDto
    {
        public string TaskName { get; set; }

        public string OpenDate { get; set; }

        public string DueDate { get; set; }

        public string LabelType { get; set; }

        public string ExecutionType { get; set; }

    }

}
