

namespace Cinema.DataProcessor.ExportDto
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class MovieExportDto
    {
        [JsonProperty("MovieName")]
        public string Title { get; set; }


        [JsonProperty("Rating")]
        public string Rating { get; set; }

        public string TotalIncomes { get; set; }

        [JsonProperty("Customers")]
        public List<SingleCustomerView> Customers { get; set; }

    }


    public class SingleCustomerView
    {
        [JsonProperty("FirstName")]
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Balance { get; set; }
    }
}
