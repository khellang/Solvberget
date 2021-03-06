﻿using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Solvberget.Domain.Documents
{
    public class Book : Document
    {
        public string Isbn { get; set; }
        public string ClassificationNr { get; set; }
        public Person Author { get; set; }
        public Organization Organization { get; set; }
        public string StandarizedTitle { get; set; }
        public string StdOrOrgTitle { get; set; }
        public string Numbering { get; set; }
        public string PartTitle { get; set; }
        public string Edition { get; set; }
        public string NumberOfPages { get; set; }
        public string Content { get; set; }
        public IEnumerable<Person> ReferredPersons { get; set; }
        public IEnumerable<Organization> ReferredOrganizations { get; set; } 
        public IEnumerable<string> ReferencedPlaces { get; set; } 
        public IEnumerable<string> Subject { get; set; }
        public IEnumerable<string> Genre { get; set; }
        public IEnumerable<Person> InvolvedPersons { get; set; }
        public IEnumerable<Organization> InvolvedOrganizations { get; set; }

        public new static Book GetObjectFromFindDocXmlBsMarc(string xml)
        {
            var book = new Book();
            book.FillProperties(xml);
            return book;
        }

        public new static Book GetObjectFromFindDocXmlBsMarcLight(string xml)
        {
            var book = new Book();
            book.FillPropertiesLight(xml);
            return book;
        }

        protected override void FillProperties(string xml)
        {
            base.FillProperties(xml);
           
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {
                var nodes = xmlDoc.Root.Descendants("oai_marc");
                ClassificationNr = GetVarfield(nodes, "090", "c");
                StdOrOrgTitle = GetVarfield(nodes, "240", "a");
                Numbering = GetVarfield(nodes, "245", "n");
                Edition = GetVarfield(nodes, "250", "a");
                NumberOfPages = GetVarfield(nodes, "300", "a");
                Content = GetVarfield(nodes, "505", "a");
                ReferredPersons = GeneratePersonsFromXml(nodes, "600");
                ReferredOrganizations = GenerateOrganizationsFromXml(nodes, "610");
                ReferencedPlaces = GetVarfieldAsList(nodes, "651", "a");
                Subject = GetVarfieldAsList(nodes, "650", "a");
                Genre = GetVarfieldAsList(nodes, "655", "a");
                InvolvedPersons = GeneratePersonsFromXml(nodes, "700");
                InvolvedOrganizations = GenerateOrganizationsFromXml(nodes, "710");
            }
        }

        protected override void FillPropertiesLight(string xml)
        {
            base.FillPropertiesLight(xml);
            var xmlDoc = XDocument.Parse(xml);
            if (xmlDoc.Root != null)
            {
                var nodes = xmlDoc.Root.Descendants("oai_marc");

                Isbn = GetVarfield(nodes, "020", "a");

                //Author, check BSMARC field 100 for author
                var nationality = GetVarfield(nodes, "100", "j");
                string nationalityLookupValue = null;
                if (nationality != null)
                    NationalityDictionary.TryGetValue(nationality, out nationalityLookupValue);
                Author = new Person
                {
                    Name = GetVarfield(nodes, "100", "a"),
                    LivingYears = GetVarfield(nodes, "100", "d"),
                    Nationality = nationalityLookupValue ?? nationality,
                    Role = "Author"
                };
               

                //If N/A, check BSMARC field 110 for author
                if (Author.Name == null)
                {

                    Author.Name = GetVarfield(nodes, "110", "a");
                    
                    //Organization (110abq)
                    Organization = GenerateOrganizationsFromXml(nodes, "110").FirstOrDefault();

                }

                //If still N/A, check BSMARC field 130 for title when title is main scheme word
                if (Author.Name == null)
                    StandarizedTitle = GetVarfield(nodes, "130", "a");

                if (Author.Name != null)
                    Author.InvertName(Author.Name);
                MainResponsible = Author;

                PartTitle = GetVarfield(nodes, "245", "p");

                if (Title != null && PartTitle != null)
                {
                    Title = Title + " : " + PartTitle;
                }

            }
        }

        protected override string GetCompressedString()
        {

            var returnValue = Author.Name;

            if (string.IsNullOrEmpty(returnValue))
            {
                return base.GetCompressedString();
            }

            if (PublishedYear != 0)
            {
                returnValue += " (" + PublishedYear + ")";
            }

            return returnValue;

        }

    }

}