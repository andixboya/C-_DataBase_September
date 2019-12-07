namespace TeisterMask.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using TeisterMask.DataProcessor.ExportDto;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {

            var projectsToExport = context.Projects
                .Where(p => p.Tasks.Any())
                .Select(p => new ProjectExportDto()
                {
                    Name = p.Name,
                    TasksCount = p.Tasks.Count,
                    HasEndDate = p.DueDate == null ? "No" : "Yes",
                    Tasks = p.Tasks.Select(t => new TaskExport()
                    {
                        Name = t.Name,
                        Label = t.LabelType.ToString()
                    })
                    .OrderBy(t => t.Name)
                    .ToList()
                })
                .ToArray()
                .OrderByDescending(p => p.Tasks.Count)
                .ThenBy(p => p.Name)
                .ToArray();

            var result = XmlExport("Projects", projectsToExport);


            return result;
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var validEmployees =
                context.Employees
                .Where(e => e.EmployeesTasks.Any(et => DateTime.Compare(et.Task.OpenDate, date) >= 1))
                .Select(e => new EmployeeExportDto()
                {
                    Username = e.Username,
                    Tasks = e.EmployeesTasks.Where(et => DateTime.Compare(et.Task.OpenDate, date) >= 1)
                    .Select(et => new TaskExportDto()
                    {
                        DueDate = et.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                        ExecutionType = et.Task.ExecutionType.ToString(),
                        LabelType = et.Task.LabelType.ToString(),
                        OpenDate = et.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                        TaskName = et.Task.Name
                    })
                    .OrderByDescending(t => DateTime.ParseExact(t.DueDate, "MM/dd/yyyy", CultureInfo.InvariantCulture))
                    .ThenBy(t => t.TaskName)
                    .ToList()
                })
                .ToArray()
                .OrderByDescending(e => e.Tasks.Count)
                .ThenBy(e => e.Username)
                .Take(10)
                .ToList();


            var result = JsonConvert.SerializeObject(validEmployees, Formatting.Indented);

            return result;
        }




        private static string XmlExport<T>(string rootName, T[] collectionToExport)
        {
            var namespaces = new XmlSerializerNamespaces(new[] { new XmlQualifiedName() });

            XmlRootAttribute root = new XmlRootAttribute(rootName);
            var serializer = new XmlSerializer(typeof(T[]), root);

            StringBuilder sb = new StringBuilder();

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, collectionToExport, namespaces);
            }

            return sb.ToString().TrimEnd();
        }
    }
}