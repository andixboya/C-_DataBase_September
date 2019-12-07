namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    using Data;
    using Newtonsoft.Json;
    using System.Xml.Serialization;
    using System.Xml;
    using System.IO;
    using TeisterMask.DataProcessor.ImportDto;
    using System.Text;
    using TeisterMask.Data.Models;
    using System.Globalization;
    using TeisterMask.Data.Models.Enums;
    using System.Linq;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {

            var importedProjects = XmlImport<ProjectImportDto>(xmlString, "Projects");

            StringBuilder sb = new StringBuilder();

            var validProjects = new List<Project>();
            foreach (var pr in importedProjects)
            {
                bool isValidObject = IsValid(pr);


                if (!isValidObject)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                var date = DateTime.TryParseExact(pr.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime resultDate);

                var validPro = new Project()
                {
                    DueDate = date == true ? (DateTime?)resultDate : null,
                    Name = pr.Name,
                    OpenDate = DateTime.ParseExact(pr.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                };

                validProjects.Add(validPro);


                foreach (var tr in pr.Tasks)
                {
                    DateTime tsOpenDate = DateTime.ParseExact(tr.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    DateTime tsDueDate = DateTime.ParseExact(tr.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                    bool validTask = IsValid(tr);
                    bool validPeriod = IsValidPeriod(tsOpenDate, tsDueDate, validPro.OpenDate, validPro.DueDate);
                    bool validExType = Enum.TryParse<ExecutionType>(tr.ExecutionType, out ExecutionType executionResult);
                    bool validLabelType = Enum.TryParse<LabelType>(tr.LabelType, out LabelType labelResult);

                    if (validTask is false || validPeriod is false || validExType is false || validLabelType is false)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var validTs = new Task()
                    {
                        DueDate = tsDueDate,
                        LabelType = labelResult,
                        ExecutionType = executionResult,
                        Name = tr.Name,
                        OpenDate = tsOpenDate,
                        Project = validPro
                    };

                    validPro.Tasks.Add(validTs);
                }

                sb.AppendLine(string.Format(SuccessfullyImportedProject, validPro.Name, validPro.Tasks.Count));
                validProjects.Add(validPro);
            }



            context.Projects.AddRange(validProjects);
            context.Tasks.AddRange(validProjects.SelectMany(p => p.Tasks));
            context.SaveChanges();

            var x = sb.ToString().TrimEnd();

            return x;
        }



        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {

            var importedEmployees = JsonConvert.DeserializeObject<EmployeeImportDto[]>(jsonString);


            StringBuilder sb = new StringBuilder();

            var validTaskIds = context.Tasks.Select(t => t.Id).Distinct().ToHashSet<int>();

            var validTaskConnections = new List<EmployeeTask>();
            var validEmployees = new List<Employee>();

            var uniqueRecords = new HashSet<string>();

            foreach (var em in importedEmployees)
            {

                bool isValidEm = IsValid(em);
                if (!isValidEm)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var validEmp = new Employee()
                {
                    Email = em.Email,
                    Phone = em.Phone,
                    Username = em.Username,
                };

                var uniqueTaskIds = em.Tasks.ToHashSet();

                foreach (var tskId in uniqueTaskIds)
                {
                    if (!validTaskIds.Contains(tskId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }


                    var currentConnection = new EmployeeTask()
                    {
                        Employee = validEmp,
                        TaskId = tskId
                    };
                    validTaskConnections.Add(currentConnection);
                    validEmp.EmployeesTasks.Add(currentConnection);

                }


                validEmployees.Add(validEmp);
                sb.AppendLine(string.Format(SuccessfullyImportedEmployee, validEmp.Username, validEmp.EmployeesTasks.Count));
            }

            context.Employees.AddRange(validEmployees);
            context.EmployeesTasks.AddRange(validTaskConnections);
            context.SaveChanges();

            var x = sb.ToString().TrimEnd();

            return x;
        }


        private static bool IsValidPeriod(DateTime tsOpenDate, DateTime tsDueDate, DateTime prOpenDate, DateTime? prDueDate)
        {
            if (prDueDate is null)
            {
                var dateCompare = DateTime.Compare(tsOpenDate, prOpenDate);


                return dateCompare > 0;
            }
            else
            {
                var firstDateCompare = DateTime.Compare(tsOpenDate, prOpenDate);
                var secondDateCompare = DateTime.Compare(tsDueDate, (DateTime)prDueDate);
               

                return firstDateCompare > 0 && secondDateCompare < 0;
            }
        }
        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }

        private static T[] XmlImport<T>(string xmlString, string rootName)
        {
            XmlRootAttribute root = new XmlRootAttribute(rootName);

            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            XmlSerializer serializer = new XmlSerializer(typeof(T[]), root);

            using (var stream = new StringReader(xmlString))
            {
                var importedProjections = (T[])serializer.Deserialize(stream);
                return importedProjections;
            }
        }

    }
}